using Nurse.Common.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebStateCenter.DDD;

namespace WebStateCenter
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            gv.DataSource = MainExe.GetServiceState();
            gv.DataBind();

            //string mq = MainExe.GetMsg("msmq");
            //if (string.IsNullOrEmpty(mq) == false)
            //{
            //    string[] rows = mq.Split(new char[] { '|' });
            //    List<MqCount> mcs = new List<MqCount>();
            //    foreach (string row in rows)
            //    {

            //        if (string.IsNullOrEmpty(row) == false)
            //        {
            //            string[] fileds = row.Split(new char[] { '@' });
            //            if (fileds.Length > 2)
            //            {
            //                mcs.Add(new MqCount()
            //                {
            //                    Name = fileds[0],
            //                    Count = fileds[1],
            //                    Remark = fileds[2]
            //                });
            //            }
            //        }

            //        if (mcs.Count > 0)
            //        {
            //            //gvMq.DataSource = mcs;
            //            //gvMq.DataBind();

            //        }
            //    }
            //}


            gvMqRate.DataSource = MonitorExe.GetMsg();
            gvMqRate.DataBind();

        }

        protected void btnConfig_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Config.aspx");
        }

    }
}