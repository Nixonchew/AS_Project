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
    public partial class Logins : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LoginMe(object sender, EventArgs e)
        {
            string pwd = tb_password.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
            int count = 0;
            DateTime logintime = DateTime.Now;
            try
            { 
                string sql = "SELECT count,loginTime FROM Account WHERE email=@USERID";
                string sql1 = "UPDATE Account SET count=(SELECT count  FROM Account where email=@USERID)+1,loginTime=@loginTime WHERE email=@USERIDS";
                string sql2 = "UPDATE Account SET count=@count WHERE email=@USERID";
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString)) {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                        cmd.Parameters.AddWithValue("@USERID", userid);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader["count"] != DBNull.Value)
                                {
                                    count = (int)reader["count"];
                                }
                                if (reader["loginTime"] != DBNull.Value)
                                {
                                    logintime = (DateTime)reader["loginTime"];                                }
                                }
                        }
                    }
                }
                if (count < 3)
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        if (ValidateCaptcha())
                        {
                            string pwdWithSalt = pwd + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);
                            if (userHash.Equals(dbHash))
                            {
                                Session["LoggedIn"] = tb_userid.Text.Trim();
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                Response.Redirect("HomePage.aspx", false);
                            }
                            else
                            {  
                                    using (SqlCommand cmd1 = new SqlCommand(sql1,conn))
                                {
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        cmd1.Parameters.AddWithValue("@USERID",userid);
                                        cmd1.Parameters.AddWithValue("@USERIDS", userid);
                                        cmd1.Parameters.AddWithValue("@loginTime",DateTime.Now);
                                        cmd1.ExecuteNonQuery();
                                    }
                                  
                                }
                                    
                                lbl_message.Text = "Wrong username or password";
                                lbl_message.ForeColor = System.Drawing.Color.Red;
                            }

                        }
                    }
                    else
                    {
                        lbl_message.Text = "Wrong username or password.Please wait for 15 mins for your account to be unlocked";
                        lbl_message.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {

                    if ((DateTime.Now - logintime).Minutes >= 5)
                    {
                        using (SqlCommand cmd2 = new SqlCommand(sql2, conn))
                            {
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd2.Parameters.AddWithValue("@USERID", userid);
                                    cmd2.Parameters.AddWithValue("@count", 0);
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                            lbl_message.Text = "Your account has been reset.Please try again";
                            lbl_message.ForeColor = System.Drawing.Color.Green;
                        }
                    else
                    {
                        lbl_message.Text = "Your account has been locked out.Please wait 5 minutes and try again";
                        lbl_message.ForeColor = System.Drawing.Color.Red;
                    }
                }
                    conn.Close();
            }    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }
        public class MyObject
        {
            public string success { get; set; }
            public List<string> erorrMessage { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LfM-P4UAAAAAL_phfhZoY2p6q5uIgr5Utpu5Gdf &response=" + captchaResponse);
            try
            {
                using (WebResponse wReponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wReponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        lbl_message.Text = jsonResponse.ToString();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select passwordHash FROM Account WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["passwordHash"] != null)
                        {
                            if (reader["passwordHash"] != DBNull.Value)
                            {
                                h = reader["passwordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }

            return h;
        }
        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select passwordSalt FROM ACCOUNT WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

    }
}