<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>

        <table>
            <tr>
                <td>Bağlantı tipi : </td>
                <td>
                    <asp:RadioButton ID="local" runat="server" GroupName="secim" Text="Localhost" OnCheckedChanged="local_CheckedChanged" AutoPostBack="True" /></td>
                <td>
                    <asp:RadioButton ID="uzak" runat="server" GroupName="secim" Text="Uzak baglantı (kullanıcı " OnCheckedChanged="uzak_CheckedChanged" AutoPostBack="True" /></td>
            </tr>
                        <tr><td></td><td></td><td>adı ve şifre gerektirir)</td></tr>
            <tr><td>  </td></tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Bağlantı Adresi : "></asp:Label>
                    &nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtBaglanti" runat="server"></asp:TextBox></td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Kullanıcı Adı :"></asp:Label>
                    &nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Şifre : "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                </td>
            </tr>
                        <tr><td>
                            <asp:Button ID="Button3" runat="server" Text="Bağlan" OnClick="Button3_Click" />
                            </td><td>            
                            <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
</td></tr>
            <tr><td></td></tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Database Adı"></asp:Label>
                    &nbsp;&nbsp;</td>
                <td>
                    <asp:DropDownList ID="dbAd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dbAd_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="TabloAdı"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="tabloAd" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Dosya oluştur." />
            <asp:CheckBox ID="web" runat="server" Text="Web dosyalarını dahil et. (aspx dosyalarını da oluşturur)" />
        </p>


        <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>


    </form>
</body>
</html>
