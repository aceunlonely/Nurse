using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace PerformanceReader
{
    public sealed class PerformanceCounterRetriever : IDisposable
    {
        private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
        private const int LOGON32_PROVIDER_WINNT50 = 3;

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern bool LogonUser(
          string lpszUserName,
          string lpszDomain,
          string lpszPassword,
          int dwLogonType,
          int dwLogonProvider,
          ref IntPtr phToken);

        private WindowsIdentity _identity;
        private WindowsImpersonationContext _context;
        private bool _disposed;
        private readonly string _server;
        public PerformanceCounterRetriever(string server, string domain, string user, string password)
        {
            if (string.IsNullOrWhiteSpace(server))
                throw new ArgumentException("Null/blank {nameof(server)} specified");
            if (string.IsNullOrWhiteSpace(domain))
                throw new ArgumentException("Null/blank {nameof(domain)} specified");
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("Null/blank {nameof(user)} specified");
            if (password == null)
                throw new ArgumentNullException(password);


            System.Diagnostics.Process process = new Process();
            System.Diagnostics.ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = string.Format("/C net use \\\\{0}\\ipc$ /user:{1}\\{2} {3}"
                    , server
                    , domain
                    , user
                    , password);
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            try
            {
                var userHandle = new IntPtr(0);
                var logonSuccess = LogonUser(
                  user,
                  domain,
                  password,
                  LOGON32_LOGON_NEW_CREDENTIALS,
                  LOGON32_PROVIDER_WINNT50,
                  ref userHandle
                );
                if (!logonSuccess)
                    throw new Exception("LogonUser failed");
                _identity = new WindowsIdentity(userHandle);
                _context = _identity.Impersonate();
                _server = server;
                _disposed = false;
            }
            finally
            {
                Dispose();
            }
        }
        ~PerformanceCounterRetriever()
        {
            Dispose(false);
        }

        public float Get(string categoryName, string counterName, string optionalInstanceName = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Null/blank {nameof(categoryName)} specified");
            if (string.IsNullOrWhiteSpace(counterName))
                throw new ArgumentException("Null/blank {nameof(counterName)} specified");

            var counters = new List<PerformanceCounter>();
            var category = new PerformanceCounterCategory(categoryName, _server);
            if (optionalInstanceName == null)
            {
                foreach (var counter in category.GetCounters())
                {
                    if (counter.CounterName == counterName)
                    {
                        return new PerformanceCounter(categoryName, counterName, null, _server).NextValue();
                    }
                }
            }
            else
            {
                return new PerformanceCounter(categoryName, counterName, optionalInstanceName, _server).NextValue();
            }
            return 0;
        }


        public PerformanceCounter GetCounter(string categoryName, string counterName, string optionalInstanceName = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Null/blank {nameof(categoryName)} specified");
            if (string.IsNullOrWhiteSpace(counterName))
                throw new ArgumentException("Null/blank {nameof(counterName)} specified");

            var counters = new List<PerformanceCounter>();
            var category = new PerformanceCounterCategory(categoryName, _server);

            if (optionalInstanceName == null)
            {
                foreach (var counter in category.GetCounters())
                {
                    if (counter.CounterName == counterName)
                    {
                        return new PerformanceCounter(categoryName, counterName, null, _server);
                    }
                }
            }
            else
            {
                if (ComputerInfo.Ping(_server))
                {
                    return new PerformanceCounter(categoryName, counterName, optionalInstanceName, _server);
                }
                
            }
            return null ;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (_identity != null)
            {
                _identity.Dispose();
                _identity = null;
            }

            if (_context != null)
            {
                _context.Undo();
                _context.Dispose();
                _context = null;
            }

            _disposed = true;
        }
    }
}