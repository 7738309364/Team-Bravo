using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eLearning.User
{
    public partial class UserTransactionDetails : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string conf = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(conf);
            conn.Open();
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            GridView1.Visible = false;
            if (!string.IsNullOrEmpty(TextBox1.Text))
            {
                int userId;
                if(int.TryParse(TextBox1.Text, out userId))
                {
                    string query = $"EXEC GetTransactionByUserId {userId}";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if(dt.Rows.Count > 0)
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        GridView1.Visible = true;
                    }
                    else
                    {
                        Response.Write("<script>alert('No record found!');</script>");
                    }
                }
            }
        }
    }
}