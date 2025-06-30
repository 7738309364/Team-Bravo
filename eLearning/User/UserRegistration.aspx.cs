using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eLearning.User
{
    public partial class UserRegistration : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string conf = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(conf);
            conn.Open();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string firstName = TextBox1.Text;
            string lastName = TextBox2.Text;
            string username = TextBox3.Text;
            string password = TextBox4.Text;
            string emailId = TextBox5.Text;
            string status = DropDownList3.SelectedValue;
            int roleId = 2;
            string role = "User";
            string query = $"EXEC InsertUser '{firstName}', '{lastName}', '{username}', '{password}', '{emailId}', '{roleId}', '{role}', '{status}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int row = cmd.ExecuteNonQuery();
            if (row > 0)
            {
                Response.Write("<script>alert('User registered successfully!');</script>");
            }
            else
            {
                Response.Write("<script>alert('Error while registering the user! Try again!');</script>");
            }
        }
    }
}