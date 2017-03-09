using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurse.Common.CM
{
    public class Enums
    {
        public enum EnumAppType
        {
            服务 = 1,
            可执行程序 = 2
        }

        public enum EnumGuardType
        { 
            进程守护 =1,
            心跳守护 =2
        }

        public enum EnumHandleCondition
        {
            服务不可见 =1 ,
            服务可见心脏停止跳动 =2,
            服务进程停止 =3 
            
        }

        public enum EnumHandlePlan
        {
            停止服务或者进程 =1,
            重启服务或者进程 =2,
            等待Count满之后重启 =3
        }
    }

   

}
