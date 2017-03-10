<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebStateCenter.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False"
                ForeColor="#333333">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="服务名" ReadOnly="True" />
                    <asp:BoundField DataField="LastBeatTime" HeaderText="上一次心跳时间" />
                    <asp:BoundField DataField="LinkState" HeaderText="连接状态" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
