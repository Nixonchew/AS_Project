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
    public partial class Registrations : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private int checkPassword(string password)
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
        protected int checkinput(string firstname,string lastname,string creditcard,string dob,string email)
        {
            int score = 0;
            if (Regex.IsMatch(firstname, "[^[A-Z]+$/i]"))
            {  
            }
            else
            {
                score += 1;
            }
            if (Regex.IsMatch(lastname, "[^[A-Z]+$/i]"))
            {
            }
            else
            {
                score += 1;
            }
            if (Regex.IsMatch(creditcard, "^[0-9]*$"))
            {
            }
            else
            {
                score += 1;
            }
            if (Regex.IsMatch(dob, "(((0|1)[0-9]|2[0-9]|3[0-1])\\/(0[1-9]|1[0-2])\\/((19|20)\\d\\d))$"))
            {
            }
            else
            {
                score += 1;
            }
            if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
            }
            else
            {
                score += 1;
            }
            return score;
        }
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
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
            int score = checkinput(tb_firstname.Text,tb_lastname.Text,tb_cci.Text,tb_dob.Text,tb_email.Text);
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
            if (checkemail(tb_email.Text))
            {
                lbl_emailchecker.Text = "User already exist";
                lbl_emailchecker.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                scores = score + scores;
                if (scores < 4)
                {
                    lbl_pwdchecker.ForeColor = Color.Red;
                    return;
                }
                lbl_pwdchecker.Text = "Status:" + status;

                lbl_pwdchecker.ForeColor = Color.Green;
                createAccount();
                Response.Redirect("Login.aspx"); 
            }
        }
        public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@firstName,@lastName, @creditCardInfo, @email, @passwordHash, @passwordSalt,@dateOfBirth,@IV,@Key,@count,@loginTime,@password1,@password2,@passwordChanged)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@firstName", tb_firstname.Text);
                            cmd.Parameters.AddWithValue("@lastName", tb_lastname.Text);
                            cmd.Parameters.AddWithValue("@creditCardInfo", Convert.ToBase64String(encryptData(tb_cci.Text)));
                            cmd.Parameters.AddWithValue("@email", tb_email.Text);
                            cmd.Parameters.AddWithValue("@passwordHash", finalHash);
                            cmd.Parameters.AddWithValue("@passwordSalt", salt);
                            cmd.Parameters.AddWithValue("@dateOfBirth", tb_dob.Text);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@count", 0);
                            cmd.Parameters.AddWithValue("@loginTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@password1", "");
                            cmd.Parameters.AddWithValue("@password2", "");
                            cmd.Parameters.AddWithValue("@passwordChanged", DateTime.Now);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public bool checkemail(string email)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * from Account WHERE email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return flag;
        }
    }
}
