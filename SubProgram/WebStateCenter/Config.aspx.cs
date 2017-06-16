using Dcjet.Framework.Helpers;
using Nurse.Common.DDD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebStateCenter.help;

namespace WebStateCenter
{
    public partial class Config : System.Web.UI.Page
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string ConfigPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mq.config");
            }
        }

        private MSMQConfig TempConfig
        {

            get
            {
                if (Session["_config"] == null)
                    return null;
                return Session["_config"] as MSMQConfig;
            }

            set
            {
                Session["_config"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        private void InitControl()
        {
            ddlCategory.DataSource = GetCategoryList();
            ddlCategory.DataTextField = "Text";
            ddlCategory.DataValueField = "Value";
            ddlCategory.DataBind();

            //加载数据
            //MSMQConfig
            if (File.Exists(ConfigPath))
            {
                MSMQConfig config = XmlHelper.Xml2Entity(ConfigPath, new MSMQConfig().GetType()) as MSMQConfig;
                TempConfig = config;
                BindGridView(config);
            }
        }

        private void BindGridView(MSMQConfig config)
        {
            //绑定数据
            List<RowData> list = new List<RowData>();
            config.Nodes.ForEach(p => { list.Add(new RowData(p)); });

            gvMq.DataSource = list;
            gvMq.DataBind();
            
        }


        /// <summary>
        /// 性能对象
        /// </summary>
        /// <returns></returns>
        private List<ListItem> GetCategoryList()
        {
            List<ListItem> categoryList = new List<ListItem>();
            categoryList.Add(new ListItem() { Text = "--请选择--", Value = "" });
            categoryList.Add(new ListItem() { Text = "MSMQ Service", Value = "MSMQ Service" });
            categoryList.Add(new ListItem() { Text = "MSMQ Queue", Value = "MSMQ Queue" });
            return categoryList;
        }

        /// <summary>
        /// 获取计数器
        /// </summary>
        /// <returns></returns>
        private List<ListItem> GetCounterList(string category)
        {

            List<ListItem> list = new List<ListItem>();
            switch (category)
            {
                case "MSMQ Queue":
                    list.Add(new ListItem() { Text = "队列深度", Value = "Messages in Queue" });
                    break;
                case "MSMQ Service":
                    list.Add(new ListItem() { Text = "传入信息数/秒", Value = "Incoming Messages/sec" });
                    list.Add(new ListItem() { Text = "传出信息数/秒", Value = "Outgoing Messages/sec" });
                    break;
            }
            return list;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectCate = ((DropDownList)sender).SelectedValue;
            //if (selectCate == "MSMQ Service")
            //{
            //    txtInstance.Enabled = false;
            //}
            //else
            //{
            //    txtInstance.Enabled = true;
            //}
            ddlCounter.DataSource = GetCounterList(selectCate);
            ddlCounter.DataTextField = "Text";
            ddlCounter.DataValueField = "Value";
            ddlCounter.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();
            ClearSelectedKeys();


        }

        private void Clear()
        {
            txtInstance.Text = "";
            txtIP.Text = "";
            txtDomain.Text = "WORKGROUP";
            txtAccount.Text = "";
            txtPwd.Text = "";

            ddlCategory.SelectedValue = "";
            //绑定数据
            ddlCounter.DataSource = GetCounterList("");
            ddlCounter.DataTextField = "Text";
            ddlCounter.DataValueField = "Value";
            ddlCounter.DataBind();

            ddlCounter.SelectedValue = "";
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            List<string> keys = GetSelectKeys();

            if (keys.Count == 0 || keys.Count > 1)
            {
                Response.Write("<script>alert('有仅只能选中一项进行编辑')</script>");
                return;
            }
            else
            {

                MSMQConfig config = this.TempConfig;
                foreach (MSMQConfigNode node in config.Nodes)
                {
                    if (keys[0] == node.ToString())
                    {
                        ddlCategory.SelectedValue = node.CategoryName;
                        //绑定数据
                        ddlCounter.DataSource = GetCounterList(node.CategoryName);
                        ddlCounter.DataTextField = "Text";
                        ddlCounter.DataValueField = "Value";
                        ddlCounter.DataBind();

                        ddlCounter.SelectedValue = node.CounterName;
                        txtInstance.Text = node.Instance;
                        //获取Domain
                        ConfigDomain cd = config.Domains.Find(p => (p.Name == node.Domain));
                        if (cd != null)
                        {
                            string[] strs = EncryptHelper.DecryptDES(cd.Value).Split('|');

                            txtIP.Text = strs[0];
                            txtDomain.Text = strs[1];
                            txtAccount.Text = strs[2];
                            txtPwd.Text = strs[3];
                        }

                    }
                }
            }
        }

        private List<string> GetSelectKeys()
        {
            List<string> keys = new List<string>();
            foreach (GridViewRow gvr in this.gvMq.Rows)
            {
                Control ctl = gvr.FindControl("ckb");
                CheckBox ck = (CheckBox)ctl;
                if (ck.Checked)
                {
                    keys.Add(((HiddenField)gvr.FindControl("hdKey")).Value);
                }
            }
            return keys;
        }

        private void ClearSelectedKeys()
        {
            foreach (GridViewRow gvr in this.gvMq.Rows)
            {
                Control ctl = gvr.FindControl("ckb");
                CheckBox ck = (CheckBox)ctl;
                ck.Checked = false;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //非空判断
            string ip = txtIP.Text.Trim();
            string domain = txtDomain.Text.Trim();
            string account = txtAccount.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string counter = ddlCounter.SelectedValue;
            string instance = txtInstance.Text.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                Response.Write("<script>alert('IP必须填写')</script>");
                return;
            }
            if (string.IsNullOrEmpty(domain))
            {
                Response.Write("<script>alert('所属域必须填写')</script>");
                return;
            }
            if (string.IsNullOrEmpty(account))
            {
                Response.Write("<script>alert('账号必须填写')</script>");
                return;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                Response.Write("<script>alert('密码必须填写')</script>");
                return;
            }
            if (string.IsNullOrEmpty(category))
            {
                Response.Write("<script>alert('监控类型必须选择')</script>");
                return;
            }
            if (string.IsNullOrEmpty(counter))
            {
                Response.Write("<script>alert('计数器必须选择')</script>");
                return;
            }
            if (category == "MSMQ Queue" && string.IsNullOrEmpty(instance))
            {
                Response.Write("<script>alert('实例必须填写')</script>");
                return;
            }

            MSMQConfig config = TempConfig;

            List<string> keys = GetSelectKeys();
            if (keys.Count == 0)
            {
                //新增

                string key = ip + category + counter + (instance ?? "");
                if (config.Nodes.Any(p => p.ToString() == key))
                {
                    //已经有这个配置
                    Response.Write("<script>alert('不能重复进行配置')</script>");
                    return;
                }
                else
                {
                    config.Nodes.Add(new MSMQConfigNode()
                    {
                        Domain = ip,
                        CategoryName = category,
                        CounterName = counter,
                        Instance = instance
                    });
                }

            }
            else if (keys.Count == 1)
            {
                //修改情况
                MSMQConfigNode node = config.Nodes.Find(p => { return p.ToString() == keys[0]; });
                if (node != null)
                {
                    node.Domain = ip;
                    node.CategoryName = category;
                    node.CounterName = counter;
                    node.Instance = instance;
                }
            }
            else
            {
                //多个时不处理
                Response.Write("<script>alert('不能同时修改多个值')</script>");
                return;
            }
            //更新domain
            var listDomains = config.Domains.Where(p => { return p.Name == ip; }).ToList<ConfigDomain>();
            string domainValue = EncryptHelper.EncryptDES(ip + "|" + domain + "|" + account + "|" + pwd);
            if (listDomains.Count == 0)
            {
                //新增情况
                config.Domains.Add(new ConfigDomain()
                {
                    Name = ip,
                    Value = domainValue
                });
            }
            else
            { 
                //修改
                listDomains[0].Value = domainValue;
            }
            //更新session
            TempConfig = config;
            //保存config
            XmlHelper.Enity2Xml(ConfigPath, config);
            //刷新表格
            BindGridView(config);
            //clear
            Clear();

            //更新服务配置文件时间
            MonitorExe.UpdateLastConfigTime();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/index.aspx");
        }


    }


    [Serializable]
    public class RowData : MSMQConfigNode
    {
        public RowData(MSMQConfigNode node)
        {
            this.CategoryName = node.CategoryName;
            this.CounterName = node.CounterName;
            this.Domain = node.Domain;
            this.Instance = node.Instance;
            this.Key = node.ToString();
        }
        public String Key { get; set; }

    }
}