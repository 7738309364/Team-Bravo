using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elearning
{
    
    public partial class Admin : System.Web.UI.Page
    {
        string conStr = "Data Source=DESKTOP-TJIH6P1\\SQLEXPRESS;Initial Catalog=elearning;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCounts();
                LoadNewCourses();
                LoadCoursesDropdown();
                LoadYearsDropdown();
            }

        }
        private void LoadCounts()
        {
            lblTotalCourses.Text = ExecuteScalar("EXEC GetTotalCourses").ToString();
            lblTotalSubcourses.Text = ExecuteScalar("EXEC GetTotalSubcourses").ToString();
            lblTotalSubscriptions.Text = ExecuteScalar("EXEC GetTotalSubscriptions").ToString();
            lblActiveUsers.Text = ExecuteScalar("EXEC GetActiveUsers").ToString();
            lblInactiveUsers.Text = ExecuteScalar("EXEC GetInactiveUsers").ToString();
            lblSoldCourses.Text = ExecuteScalar("EXEC GetSoldCourses").ToString();
        }

        private void LoadNewCourses()
        {
            blNewCourses.Items.Clear();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "EXEC GetNewCourses";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    blNewCourses.Items.Add(reader["Course_Name"].ToString());
                }
            }
        }

        private void LoadCoursesDropdown()
        {
            ddlCourses.Items.Clear();
            ddlCourses.Items.Add(new ListItem("-- Select Course --", "0"));
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "SELECT Course_Id, Course_Name FROM Course";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ddlCourses.Items.Add(new ListItem(reader["Course_Name"].ToString(), reader["Course_Id"].ToString()));
                }
            }
        }

        private void LoadYearsDropdown()
        {
            ddlYears.Items.Clear();
            ddlYears.Items.Add(new ListItem("-- Select Year --", "0"));
            for (int year = DateTime.Now.Year; year >= 2020; year--)
            {
                ddlYears.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }

        private object ExecuteScalar(string query)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                return cmd.ExecuteScalar();
            }
        }

        protected void ddlCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            int courseId = int.Parse(ddlCourses.SelectedValue);
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = $"EXEC GetSubcourseSummary {courseId}";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvSubcourseSummary.DataSource = dt;
                gvSubcourseSummary.DataBind();
            }
        }

        protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = int.Parse(ddlYears.SelectedValue);
            string labels = "";
            string data = "";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = $"EXEC GetMonthlyTransactionSummaryByYear {year}";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    labels += $"'{reader["MonthName"]}',";
                    data += $"{reader["TotalAmount"]},";
                }
            }

            labels = labels.TrimEnd(',');
            data = data.TrimEnd(',');

            string script = $@"
                var ctx = document.getElementById('transactionChart').getContext('2d');
                new Chart(ctx, {{
                    type: 'bar',
                    data: {{
                        labels: [{labels}],
                        datasets: [{{
                            label: 'Total Amount',
                            data: [{data}],
                            backgroundColor: 'rgba(54, 162, 235, 0.7)'
                        }}]
                    }},
                    options: {{
                        responsive: true,
                        scales: {{
                            y: {{
                                beginAtZero: true
                            }}
                        }}
                    }}
                }});";

            ClientScript.RegisterStartupScript(this.GetType(), "transactionChart", script, true);
        }
    }
}
    
