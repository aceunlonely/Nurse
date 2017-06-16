<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="WebStateCenter.Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>配置界面</title>
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
        <table>
            <tr>
                <th>IP
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtIP"></asp:TextBox>
                </td>
                <th>所属域
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtDomain" Text="WORKGROUP"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>账号
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtAccount"></asp:TextBox>
                </td>
                <th>密码
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPwd"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>监控类型
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCategory" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </td>
                <th>计数器
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCounter"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>实例
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtInstance"></asp:TextBox>
                </td>
            </tr>
        </table>

        <div>
            <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click">新增</asp:LinkButton>
            <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click">修改</asp:LinkButton>
            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">保存</asp:LinkButton>
            <asp:LinkButton ID="btnDelete" runat="server">删除</asp:LinkButton>
        </div>
        <div>
            <asp:GridView ID="gvMq" runat="server" AutoGenerateColumns="False"
                ForeColor="#333333" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="选择">
                        <HeaderStyle HorizontalAlign="Center" Width="45px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb" runat="server" />
                            <asp:HiddenField  ID="hdKey" runat="server" Value='<%#Eval("Key") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Domain" HeaderText="IP" />
                    <asp:BoundField DataField="CategoryName" HeaderText="监控类型" />
                    <asp:BoundField DataField="CounterName" HeaderText="计数器"  />
                    <asp:BoundField DataField="Instance" HeaderText="实例"  />
                </Columns>
            </asp:GridView>
        </div>

        <asp:Button Text="返回" runat="server"  ID="btnBack" OnClick="btnBack_Click" />
    </form>
</body>
</html>
