using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nurse.Test
{
    class TestLog
    {

        public void Test()
        {

            //testMultiThredLog();
        }

        public void writeLogSingle(object logName)
        {
            DLog log = new DLog();
            log.Init(logName.ToString(), logName.ToString() + "/log");

            for (int i = 0; i < 10000; i++)
            {
                log.Warn(logName + " : " + (i + 1));
                Thread.Sleep(10);
            }
        }

        public void testMultiThredLog()
        {
            for (int i = 1; i <= 100; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(writeLogSingle));
                t.Start("log" + i);
            }
        }

    }
}
