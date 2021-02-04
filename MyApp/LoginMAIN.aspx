<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LoginMAIN.aspx.cs" Inherits="MyApp.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script src="https://www.google.com/recaptcha/api.js?render=6LfM-P4UAAAAAEgdkJq56VO7IL77J5uC0S5paSzS"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
        <p>
            <asp:Label ID="lbl_message" runat="server" EnableViewState="False">Error message label (lblMessage)</asp:Label>
        </p>

         <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
     </fieldset>
          </div>

      

</asp:Content>
