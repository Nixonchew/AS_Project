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
    public partial class Change : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        bool flag = false;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed2_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE email=@USERID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@USERID", tb_email.Text);
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {   

                    if(reader.HasRows)
                    {
                        flag = true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Doesnt exist");
                        lbl_error.Text = "User does not exist";
                        lbl_error.ForeColor = System.Drawing.Color.Red;        
                    }
                    if (flag == true)
                    {
                        Response.Redirect("ChangePass.aspx?userID="+tb_email.Text,false);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
        }
    }
}