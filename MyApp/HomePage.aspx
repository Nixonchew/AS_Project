<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="MyApp.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-left: 43px;
        }
    </style>
</head>
     <script src="https://www.google.com/recaptcha/api.js"></script>
<body>
   <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>User Profile</legend>
                <h2>Welcome,<asp:Label runat="server" ID="lbl_name"></asp:Label></h2>
                <br />
                <asp:Label runat="server" Text="Email: "></asp:Label>
                <asp:Label runat="server" ID="lbl_email"></asp:Label>
                <br />
                <asp:Label runat="server" Text="Credit Card Information: "></asp:Label>
                <asp:Label runat="server" ID="lbl_cci"></asp:Label>
                 <br />
                <asp:Label runat="server" Text="Date of Birth: "></asp:Label>
                <asp:Label runat="server" ID="lbl_dob"></asp:Label>
                <br />
                <asp:Label runat="server" ID="lbl_pwdchange" Text=""></asp:Label>
                <asp:Button runat="server" Text="Change Password"  Visible="false" ID="btn_changepwd" CssClass="auto-style1" OnClick="btn_changepwd_Click"/>
                <br />
                <br />
                <asp:Label runat="server" EnableViewState="false" ID="lblMessage"></asp:Label>
                <br />
                <br />
                <asp:Button runat="server" Text="Logout" OnClick="LogoutMe" Visible="false" ID="btnLogout"/>
            </fieldset>
        </div>
    </form>
</body>
</html>
