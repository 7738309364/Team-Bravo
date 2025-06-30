using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eLearning.Admin
{
    public partial class AdminTransactionDetails : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string conf = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(conf);
            conn.Open();
            if (!IsPostBack)
            {
                GetAllTransactions();
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string input = TextBox1.Text;
            if (string.IsNullOrEmpty(input))
            {
                GetAllTransactions();
            }
            else
            {
                int userId;
                if (int.TryParse(input, out userId))
                {
                    GetTransactionsByUserId();
                }
                else
                {
                    GetAllTransactions();
                }
            }
        }

        public void GetAllTransactions()
        {
            string query = "EXEC GetTransactionDetailsWithUser";
            SqlCommand cmd=new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            if (dt.Rows.Count == 0)
            {
                ShowAlert();
            }
        }

        public void GetTransactionsByUserId()
        {
            int userId = int.Parse(TextBox1.Text);
            string query = $"EXEC GetTransactionByUserId {userId}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            if (dt.Rows.Count == 0)
            {
                ShowAlert();
            }
        }

        public void ShowAlert()
        {
            Response.Write("<script>alert('No record found!');</script>");
        }
    }
}