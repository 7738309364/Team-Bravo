using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Schema;

namespace eLearning.User
{
    
    public partial class CoursePurchasedView : System.Web.UI.Page
    {
        
        int userId=1;
       
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["userId"] = 1;
            string cnf = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cnf);
            conn.Open();

            if (!IsPostBack)
            {
                PurchasedCourses();
            }
        }


        public void PurchasedCourses()
        {
            userId = (int)Session["userId"];

            DataTable dt = new DataTable();

            string q = $"PurchasedCourse_sb {userId}";

            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            GridViewCourses.DataSource = dt;
            GridViewCourses.DataBind();
           


        }
        protected void GridViewCourses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Play")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                int subcourseId = int.Parse(args[0]);
                int courseId = int.Parse(args[1]);
                Session["SubcourseId"] = subcourseId;
                Session["CourseId"] = courseId;
                Response.Redirect("VideoPlayer.aspx");
                

            }
        }

    }
}