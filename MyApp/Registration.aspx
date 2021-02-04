<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="MyApp.Registrations" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-2.1.4.min.js"></script>  
    <script src="js/bootstrap.min.js"></script>  
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
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
        function validatecci() {
            var str = document.getElementById('<%=tb_cci.ClientID%>').value;
            if (str.search(/^[0-9]*$/) == -1) {
                document.getElementById("lbl_ccichecker").innerHTML = "Only digits are allowed";
                document.getElementById("lbl_ccichecker").style.color = "Red";
                return ("Only Numbers allowed");
            }
            else if (str.length < 13) {
                document.getElementById("lbl_ccichecker").innerHTML = "Invalid card number";
                document.getElementById("lbl_ccichecker").style.color = "Red";
                return ("Inavlid card number");

            } else {
                document.getElementById("lbl_ccichecker").innerHTML = "Valid";
                document.getElementById("lbl_ccichecker").style.color = "Green";
            }
        }

        function validatefirstname() {
            var str = document.getElementById('<%=tb_firstname.ClientID%>').value;
            if (str.search(/^[A-Z]+$/i) == -1) {
                document.getElementById("lbl_firstnamechecker").innerHTML = "Only alphabets are allowed";
                document.getElementById("lbl_firstnamechecker").style.color = "Red";
                return ("Only alphabets allowed");
            } else {
                document.getElementById("<%=lbl_firstnamechecker.ClientID%>").innerHTML = "Valid";
                document.getElementById("<%=lbl_firstnamechecker.ClientID%>").style.color = "Green";
            }
        }

        function validatelastname() {
            var str = document.getElementById('<%=tb_lastname.ClientID%>').value;
            if (str.search(/^[A-Z]+$/i) == -1) {
                document.getElementById("lbl_lastnamechecker").innerHTML = "Only alphabets are allowed";
                document.getElementById("lbl_lastnamechecker").style.color = "Red";
                return ("Only alphabets allowed");
            } else {
                document.getElementById("lbl_lastnamechecker").innerHTML = "Valid";
                document.getElementById("lbl_lastnamechecker").style.color = "Green";
            }
        }
        function validatedob() {
            var str = document.getElementById('<%=tb_dob.ClientID%>').value;
            if (str.search(/^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]|(?:Jan|Mar|May|Jul|Aug|Oct|Dec)))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2|(?:Feb))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9]|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep))|(?:1[0-2]|(?:Oct|Nov|Dec)))\4(?:(?:1[6-9]|[2-9]\d)\d{2})$/) == -1) {
                document.getElementById("lbl_dobchecker").innerHTML = "Invalid Date";
                document.getElementById("lbl_dobchecker").style.color = "Red";
                return ("invalid date");
            } else {
                document.getElementById("lbl_dobchecker").innerHTML = "Valid";
                document.getElementById("lbl_dobchecker").style.color = "Green";
            }
        }
        function validateemail() {
            var str = document.getElementById('<%=tb_email.ClientID%>').value;
            if (str.search(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g) == -1) {
                document.getElementById("lbl_emailchecker").innerHTML = "Invalid Email";
                document.getElementById("lbl_emailchecker").style.color = "Red";
                return ("invalid email");
            } else {
                document.getElementById("lbl_emailchecker").innerHTML = "Valid";
                document.getElementById("lbl_emailchecker").style.color = "Green";
            }
        }
     </script>
    <style type="text/css">

        .auto-style1 {
            margin-left: 40px;
        }
        .indicator span{
            width:100%;
            height:100%;
            background:lightgrey;
            border-radius:5px;
            position:relative;
        }

        .auto-style3 {
            margin-left: 81px;
        }
        .auto-style4 {
            margin-left: 84px;
        }
        .auto-style5 {
            margin-left: 89px;
        }
        .auto-style6 {
            margin-left: 61px;
        }

        .auto-style7 {
            margin-left: 120px;
        }
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
  <a class="active">Register</a>
  <a href="Login.aspx" >Login</a>
</div>
    <form id="form1" runat="server">
        <div>
                <fieldset>
                    <legend>Registration</legend>
             <p>
        <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label>
        <asp:TextBox ID="tb_firstname" runat="server"   CssClass="auto-style3" onkeyup="javascript:validatefirstname()" ></asp:TextBox>
              <asp:Label runat="server" ID="lbl_firstnamechecker" Text="Invalid"></asp:Label>
        </p>
          <p>
        <asp:Label ID="Label2" runat="server" Text="Last Name"></asp:Label>
        <asp:TextBox ID="tb_lastname" runat="server"   CssClass="auto-style4" onkeyup="javascript:validatelastname()"></asp:TextBox>
                <asp:Label runat="server" ID="lbl_lastnamechecker" Text="Invalid"></asp:Label>
        </p>
        <p>
        <asp:Label ID="Label3" runat="server" Text="Credit Card Info"></asp:Label>
        <asp:TextBox ID="tb_cci" runat="server"   CssClass="auto-style1" onkeyup="javascript:validatecci()"></asp:TextBox>
            <asp:Label runat="server" ID="lbl_ccichecker" Text="Invalid"></asp:Label>
        </p>
        <p>
            <asp:Label runat="server" Text="Email"></asp:Label>
            <asp:TextBox runat="server" ID="tb_email" CssClass="auto-style7" onkeyup="javascript:validateemail()"></asp:TextBox>
            <asp:Label runat="server" ID="lbl_emailchecker" Text="Invalid"></asp:Label>
        </p>
        <asp:Label ID="password" runat="server" Text="Password"></asp:Label>
        <asp:TextBox ID="tb_password" runat="server"  TextMode="Password" onkeyup="javascript:validatepassword()" CssClass="auto-style5"></asp:TextBox>
        <asp:Label ID="lbl_pwdchecker" runat="server" Text="pwdchecker"></asp:Label>
        <p>
        <asp:Label ID="Label4" runat="server" Text="Date of Birth"></asp:Label>
         <asp:TextBox ID="tb_dob" runat="server"   CssClass="auto-style6" onkeyup="javascript:validatedob()"></asp:TextBox>
            <asp:Label runat="server" ID="lbl_dobchecker" Text="Invalid"></asp:Label>
        </p>
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" style="margin-left: 13px" Text="Register" Width="404px" />
        </p>
        <p>
            &nbsp;</p>
                    </fieldset>
        </div>
    </form>
      <script src="js/jquery-2.1.4.min.js"></script>  
    <script src="js/bootstrap.min.js"></script> 
</body>
</html>
