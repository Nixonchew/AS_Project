<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change.aspx.cs" Inherits="MyApp.Change" %>

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
    </style>
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
                <asp:Label runat="server" Text="Username: "></asp:Label>
                <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                <br />
                <asp:Label runat="server" ID="lbl_error"></asp:Label>
                <br />
                <br />
                <asp:Button runat="server" Text="Submit" OnClick="Unnamed2_Click" />
            </fieldset>
        </div>
    </form>
</body>
</html>
