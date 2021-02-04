<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="MyApp.ChangePass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
           .topnav {
  background-color: #333;
  overflow: hidden;
}

/* Style the links inside the navigation bar */
.topnav a {
  float: right;
  color: #f2f2f2;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
  font-size: 17px;
}

/* Change the color of links on hover */
.topnav a:hover {
  background-color: #ddd;
  color: black;
}

/* Add a color to the active/current link */
.topnav a.active {
  background-color: #4CAF50;
  color: white;
}
        .auto-style1 {
            margin-left: 126px;
        }
    </style>
    <script type="text/javascript">
        function validatepassword() {
            var str = document.getElementById('<%=tb_password.ClientID%>').value;
            if (str.length < 8) {
                document.getElementById("lbl_pwdchecker").inner = "Password Length must be at least 8 characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("Too short");
            } else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password must require 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("No number");
            } else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require 1 uppercase letter";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("No Uppercase");
            } else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password must require 1 lowercase letter";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("No lowercase");
            } else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password must require 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("No special character");
            }
            document.getElementById("lbl_pwdchecker").innerHTML = "Valid";
            document.getElementById("lbl_pwdchecker").style.color = "Green";
        }
    </script>
</head>
<body>
    <div class="topnav">
  <a href="Registration.aspx" >Register</a>
  <a href="Login.aspx">Login</a>
</div>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>Change Password</legend>
                <asp:Label runat="server" Text="Password: "></asp:Label>
                <asp:TextBox ID="tb_password" runat="server" onkeyup="javascript:validatepassword()"></asp:TextBox>
                <asp:Label ID="lbl_pwdchecker" runat="server" Text="pwdchecker"></asp:Label>
                <br />
                <asp:Label runat="server" ID="lbl_error"></asp:Label>
                <br />
                <br />
                <asp:Button runat="server" Text="Submit" OnClick="Unnamed2_Click" /> <asp:Button runat="server" Text="Go to login" CssClass="auto-style1" Visible="False" ID="reroute" OnClick="reroute_Click"/>
            </fieldset>
        </div>
    </form>
</body>
</html>
