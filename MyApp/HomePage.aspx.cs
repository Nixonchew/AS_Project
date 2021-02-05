using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

namespace MyApp
{
    public partial class HomePage : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] cci = null;
        string userID = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            userID = (Session["LoggedIn"].ToString());
            if (Session["LoggedIn"]!=null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login,aspx", false);
                }
                else
                {
                   
                    lblMessage.Text = "Congratulations! You are logged in.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    btnLogout.Visible = true;
                    displayUserProfile(userID);
                }
            }
            else
            {
                Response.Redirect("Login.aspx",false);
            }
        }
        protected void LogoutMe(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Login.aspx", false);
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrpyt = new CryptoStream(msDecrypt,decryptTransform,CryptoStreamMode.Read)) {
                        using (StreamReader srDecrypt = new StreamReader(csDecrpyt)){
                            plainText = srDecrypt.ReadToEnd();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }
        protected void displayUserProfile(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM Account WHERE email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            DateTime password_changed = DateTime.Now;
            command.Parameters.AddWithValue("@userId", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["email"].ToString();
                        }
                        if (reader["creditCardInfo"] != DBNull.Value)
                        {
                            cci = Convert.FromBase64String(reader["creditCardInfo"].ToString());
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                        if (reader["firstName"] != DBNull.Value | reader["lastName"] != DBNull.Value)
                        {
                            string firstName = reader["firstName"].ToString();
                            string lastName = reader["lastName"].ToString();
                            string name = firstName + " " + lastName;
                            lbl_name.Text = name;
                        }
                        if (reader["dateOfBirth"] != DBNull.Value)
                        {
                            lbl_dob.Text = reader["dateOfBirth"].ToString();
                        }
                        if (reader["passwordChanged"] != DBNull.Value)
                        {
                             password_changed = (DateTime)reader["dateOfBirth"];
                        }
                    }
                    if ((DateTime.Now-password_changed).Minutes>1)
                    {
                        lbl_pwdchange.Text = "Your password has exceeded the password age of 15 minutes, please change your password";
                        lbl_pwdchange.ForeColor = Color.Red;
                        btn_changepwd.Visible = true;
                    }
                    lbl_cci.Text = decryptData(cci);
                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

        }

        protected void btn_changepwd_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePass.aspx?userID=" + userID, false);
        }
    }
}