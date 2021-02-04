<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyApp.Logins" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script src="https://www.google.com/recaptcha/api.js?render=6LfM-P4UAAAAAEgdkJq56VO7IL77J5uC0S5paSzS"></script>
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
  <a class="active">Login</a>
</div>
    <form id="form1" runat="server">
         <div>
          
        <fieldset style="margin-left: 40px">
        <legend>Login</legend>
              
        <p>
           <asp:Label runat="server" Text="Username:"></asp:Label>
            <asp:TextBox runat="server" ID="tb_userid"></asp:TextBox>
        </p>
        <p>
            <asp:Label runat="server" Text="Password:"></asp:Label>
            <asp:TextBox runat="server" ID="tb_password"></asp:TextBox>
        </p>
        <asp:Button runat="server" ID="btn_login" Text="Login" OnClick="LoginMe"/>  
        <a href="Change.aspx" style="margin-left:140px;text-decoration:underline;">Forget Password</a>
        <p>
            <asp:Label ID="lbl_message" runat="server" EnableViewState="False">Error message label (lblMessage)</asp:Label>
        </p>

         
     </fieldset>
          </div>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LfM-P4UAAAAAEgdkJq56VO7IL77J5uC0S5paSzS', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>

</body>
</html>
