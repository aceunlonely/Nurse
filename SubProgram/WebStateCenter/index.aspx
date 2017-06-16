<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebStateCenter.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <style type="text/css">
        .mGrid {
            width: 100%;
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }

            .mGrid td {
                padding: 2px;
                border: solid 1px #c1c1c1;
                color: #717171;
            }

            .mGrid th {
                padding: 4px 2px;
                color: #fff;
                background: #424242 url(grd_head.png) repeat-x top;
                border-left: solid 1px #525252;
                font-size: 0.9em;
            }

            .mGrid .alt {
                background: #fcfcfc url(grd_alt.png) repeat-x top;
            }

            .mGrid .pgr {
                background: #424242 url(grd_pgr.png) repeat-x top;
            }

                .mGrid .pgr table {
                    margin: 5px 0;
                }

                .mGrid .pgr td {
                    border-width: 0;
                    padding: 0 6px;
                    border-left: solid 1px #666;
                    font-weight: bold;
                    color: #fff;
                    line-height: 12px;
                }

                .mGrid .pgr a {
                    color: #666;
                    text-decoration: none;
                }

                    .mGrid .pgr a:hover {
                        color: #000;
                        text-decoration: none;
                    }
    </style>
    <form id="form1" runat="server">
        <h3>服务监控</h3>
        <div>

            <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False"
                ForeColor="#333333" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="ServerMac" HeaderText="服务器Mac地址" />
                    <asp:BoundField DataField="Name" HeaderText="服务名" ReadOnly="True" />
                    <asp:BoundField DataField="LastBeatTime" HeaderText="上一次心跳时间" />
                    <asp:BoundField DataField="LinkState" HeaderText="连接状态" />
                </Columns>
            </asp:GridView>
        </div>

        <h3>MQ监控</h3>
        <%--<h4>通道深度</h4>
        <div>
            <asp:GridView ID="gvMq" runat="server" AutoGenerateColumns="False"
                ForeColor="#333333" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="MQ通道" />
                    <asp:BoundField DataField="Count" HeaderText="通道深度" />
                    <asp:BoundField DataField="Remark" HeaderText="说明" />
                </Columns>
            </asp:GridView>
        </div>
        <h4>实时进出速率</h4>--%>

        <asp:LinkButton ID="btnConfig"  runat="server" OnClick="btnConfig_Click" >配置</asp:LinkButton>
        <asp:LinkButton ID="btnRefresh"  runat="server"  OnClick="btnRefresh_Click" >手动刷新</asp:LinkButton>
        
        <div>
            <asp:GridView ID="gvMqRate" runat="server" AutoGenerateColumns="False"
                ForeColor="#333333" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="Domain" HeaderText="IP" HeaderStyle-Width="15%" />
                    <asp:BoundField DataField="CounterName" HeaderText="监控项" HeaderStyle-Width="15%" />
                    <asp:BoundField DataField="Instance" HeaderText="实例" HeaderStyle-Width="15%" />
                    <asp:BoundField DataField="Result" HeaderText="结果" HeaderStyle-Width="15%" />
                    <asp:BoundField DataField="Remark" HeaderText="说明" />
                </Columns>
            </asp:GridView>
        </div>


       <asp:Button ID="autoRefresh" runat="server"  style ="display:none;" OnClick="autoRefresh_Click"/>
    </form>
</body>
</html>
<script type="text/javascript">
    function myrefresh() {
        // window.location.reload();

        window.document.getElementById('<%=autoRefresh.ClientID %>').click()
    }
    setTimeout('myrefresh()', 2000); //指定1秒刷新一次 

</script>
