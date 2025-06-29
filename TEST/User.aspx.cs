using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elearning
{
    public partial class User : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        int userId = 1; // You can dynamically fetch this based on login session

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }
        private void LoadDashboardData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_GetUserDashboardData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // 1. Selected Courses
                    int totalCourses = 0;
                    while (reader.Read())
                    {
                        totalCourses++;
                    }
                    ltTotalCourses.Text = $"{totalCourses}";

                    // 2. Completed Courses
                    if (reader.NextResult() && reader.Read())
                    {
                        lblCompleted.Text = $"{reader["CompletedCount"]}";
                    }

                    // 3. Incomplete Courses
                    if (reader.NextResult() && reader.Read())
                    {
                        lblIncomplete.Text = $"{reader["IncompleteCount"]}";
                    }

                    
                    // 4. Course Progress for Pie Chart
                    int watchedTotal = 0;
                    int remainingTotal = 0;

                    if (reader.NextResult() && reader.Read())
                    {
                        watchedTotal = reader["WatchedTotal"] != DBNull.Value ? Convert.ToInt32(reader["WatchedTotal"]) : 0;
                        remainingTotal = reader["RemainingTotal"] != DBNull.Value ? Convert.ToInt32(reader["RemainingTotal"]) : 0;
                    }

                    conn.Close();

                    ClientScript.RegisterStartupScript(this.GetType(), "pieChartScript",
                        $"renderPieChart({watchedTotal}, {remainingTotal});", true);
                }
            }

            LoadSubcourses();
        }

        private void LoadSubcourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand($"SELECT Course_Name AS SubcourseName, " +
                                                   $"(CAST(Watched_Video AS FLOAT) / Total_Video) * 100 AS CompletedPercentage " +
                                                   $"FROM User_Course WHERE User_Id = {userId}", conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                rptSubcourses.DataSource = dt;
                rptSubcourses.DataBind();
            }
        }
    }
}
