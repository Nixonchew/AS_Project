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
    public partial class ChangePass : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        bool password_checker = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(Request.QueryString["userID"]);
        }
        protected int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score += 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score += 1;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score += 1;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score += 1;
            }
            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score += 1;
            }
            return score;
        }
        protected void Unnamed2_Click(object sender, EventArgs e)
        {
            string userid = Request.QueryString["userID"];
            string pwd = tb_password.Text;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            int scores = checkPassword(tb_password.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
            }
            if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwdchecker.Text = "Status:" + status;
            lbl_pwdchecker.ForeColor = Color.Green;
            if (checkminimum(userid)) {
                if (checkpass(userid))
                {
                    changepass(userid);
                    lbl_error.Text = "Successfully changed password";
                    lbl_error.ForeColor = Color.Green;
                    reroute.Visible = true;
                }
                else
                {
                    lbl_error.Text = "Cannot reuse previous 2 passwords";
                    lbl_error.ForeColor = Color.Red;
                }
            }
            else
            {
                lbl_error.Text = "Please wait for a minimum of 5 minutes before changing your password";
                lbl_error.ForeColor = Color.Red;
            }
        }
        protected void changepass(string email)
        {

            string sql = "UPDATE Account SET passwordHash=@passwordHash,passwordSalt=@passwordSalt,password2=(SELECT password1 FROM Account WHERE email=@USERID),password1=@password1,passwordChanged=@passwordChanged WHERE email=@USERID";
            using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@USERID", email);
                        cmd.Parameters.AddWithValue("@passwordHash", finalHash);
                        cmd.Parameters.AddWithValue("@passwordSalt", salt);
                        cmd.Parameters.AddWithValue("@password1", tb_password.Text);
                        cmd.Parameters.AddWithValue("@passwordChanged", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        protected  bool checkpass(string email) {
            string password1 = "";
            string password2 = "";
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT password1,password2 from Account WHERE email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["password1"]!=DBNull.Value)
                        {
                            password1 = reader["password1"].ToString();
                        }
                        if (reader["password2"] != DBNull.Value)
                        {
                            password2 = reader["password2"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }

            if (tb_password.Text==password1 |tb_password.Text==password2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool checkminimum(string email)
        {
            DateTime datechanged = DateTime.Now;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT passwordChanged from Account WHERE email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["passwordChanged"] != DBNull.Value)
                        {
                             datechanged= (DateTime)reader["passwordChanged"];
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            if ((DateTime.Now - datechanged).Minutes < 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected void reroute_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }
    }
}