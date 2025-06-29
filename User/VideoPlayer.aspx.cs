using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace eLearning.User
{

    public partial class VideoPlayer : System.Web.UI.Page
    {
        int userId=1;
        int subcourseId;
        int courseId;

        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            userId = (int)(Session["userId"]);
            subcourseId = (int)(Session["SubcourseId"]);
            courseId = (int)(Session["CourseId"]);
            string cnf = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cnf);
            conn.Open();
            if (!IsPostBack)
            {

                int watched = GetWatchedVideo();
                //Set the watched video as 0 becuase it is storing 1 as in database in the user_course table as default
                Session["UnlockTopic"] = watched - 1; 
                //To execute LoadTopics
                LoadTopics();
                ShowOnlyPanel(null);
            }
 
        }

        //get watched_video fromm user_course
        public int GetWatchedVideo()
        {
            int watched = 0;
            string q = $"exec GetWatchedVideoNumber_sb @userId={userId},@subcourseId={subcourseId}";
            SqlCommand cmd = new SqlCommand(q, conn);
            watched = (int)cmd.ExecuteScalar();
            return watched;
        }

        //load topics to show in grid 
        public void LoadTopics()
        {
            string query = $"exec TopicForVideos_sb @subcourseId={subcourseId},@courseId={courseId}";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridViewTopics.DataSource = dt;
            GridViewTopics.DataBind();
            

        }

        
        public void GridViewTopics_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            if (e.CommandName == "PlayVideo")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                int topicId = int.Parse(args[0]);
                string input = args[1].Trim();
                int clickedIndex = int.Parse(args[2]);
                string embedUrl = input;
                if (input.Contains("youtube.com/embed/"))
                {
                     
                }
                else
                {
                    string videoId = input;

                    if (input.Contains("youtu.be/"))
                    {
                        videoId = input.Split(new[] { "youtu.be/" }, StringSplitOptions.None)[1].Split('?')[0];
                    }
                    else if (input.Contains("watch?v="))
                    {
                        videoId = input.Split(new[] { "watch?v=" }, StringSplitOptions.None)[1].Split('&')[0];
                    }


                    embedUrl = $"https://www.youtube.com/embed/{videoId}";
                }

                int watched = (int)Session["UnlockTopic"];
                if (clickedIndex > watched)
                {
                    Response.Write("<script>alert('Please watch previous videos first.')</script>");

                }
                else
                {
                    int newUnlocked = clickedIndex + 1;
                    if (newUnlocked > watched)
                    {
                        UpdateWatchedVideo(newUnlocked);
                        Session["UnlockTopic"] = newUnlocked;
                    }

                    LoadTopics();

                    embedUrl += "?autoplay=1";
                    string js = $"document.getElementById('videoPlayer').src = '{embedUrl}';";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PlayVideo", js, true);

                    HiddenTopicId.Value = topicId.ToString();
                    ShowOnlyPanel(null);
                }
            }
        }
        //update the watched_video in the user_course
        public void UpdateWatchedVideo(int newUnlocked)
        {
            
            string q = $"exec IncreaseWatchedVideoCount_sb @watched_video={newUnlocked},@subcourseId={subcourseId},@userId={userId} ";
            SqlCommand cmd1 = new SqlCommand(q,conn);            
            cmd1.ExecuteNonQuery();
            GetCertificate();
        }

        //When click on mcq button 
        public void btnMCQ_Click(object sender, EventArgs e)
        {
            
            if (int.TryParse(HiddenTopicId.Value, out int topicId) && topicId!=0)
            {
                LoadMCQ(topicId);
                ShowOnlyPanel(PanelMCQ);
            }
            else
            {
                Response.Write("<script>alert('Select the topic first')</script>");
            }
        }

        public void LoadMCQ(int topicId)
        {

            string query = $"exec LoadMCQForTopic_sb @topicId={topicId},@subcourseId={subcourseId},@courseId={courseId}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            RepeaterMCQ.DataSource = dt;
            RepeaterMCQ.DataBind();
            
        }

        //Show the answers when click on submit button in the mcq panel
        public void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in RepeaterMCQ.Items)
            {
                string userAns = ((TextBox)item.FindControl("txtAnswer")).Text.Trim().ToUpper();
                string correctAns = ((HiddenField)item.FindControl("hdnCorrect")).Value.Trim().ToUpper();
                var litResult = (Literal)item.FindControl("litResult");
                var color = userAns == correctAns ? "green" : "red";
                litResult.Text = $"<span style='color:{color}'>Your Answer: {userAns} | Correct: {correctAns}</span>";
            }

        }

        //show assignment panel when clicked on the assignment button 
        public void btnAssignment_Click(object sender, EventArgs e)
        {
            if (int.TryParse(HiddenTopicId.Value, out int topicId) && topicId != 0)
            {
                LoadAssignment(topicId);
                ShowOnlyPanel(PanelAssignment);
            }
            else
            {
                Response.Write("<script>alert('Select the topic first')</script>");
            }
        }

        public void LoadAssignment(int topicId)
        {

            string query = $"exec LoadAssignmentForTopic_sb @topicId={topicId},@subcourseId={subcourseId},@courseId={courseId}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                lblAssignmentName.Text = dr["Assignment_Name"].ToString();
                string filePath = ResolveUrl(dr["Assignment_File"].ToString());

                lnkView.NavigateUrl = ResolveUrl(filePath);
                lnkView.Target = "_blank";
                lnkDownload.NavigateUrl = ResolveUrl(filePath);

                HiddenAssignmentId.Value = dr["Assignment_Id"].ToString();
                lnkView.Visible = true;
                lnkDownload.Visible = true;
                FileUpload1.Visible = true;
                btnUpload.Visible = true;
                lblMessage.Visible = true;
            }
            else
            {
                lblAssignmentName.Text = "No assignment available.";
                lnkView.Visible = false;
                lnkDownload.Visible = false;
                FileUpload1.Visible = false;
                btnUpload.Visible = false;
                lblMessage.Visible = false;
            }
        }


        public void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {

                int assignmentId = int.Parse(HiddenAssignmentId.Value);

                string ckUserAssignmentOnce = $"exec CheckUserAssignmentCount_sb @userId={userId} , @assignmentId = {assignmentId}";
                SqlCommand checkCmd = new SqlCommand(ckUserAssignmentOnce, conn);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    Response.Write("<script>alert('You have already submitted this assignment.')</script>");
                    lblMessage.Visible = false;
                }
                else
                {
                    
                    FileUpload1.SaveAs(Server.MapPath("~/uploads/")+ Path.GetFileName(FileUpload1.FileName));
                    var solution = "~/uploads/" + Path.GetFileName(FileUpload1.FileName);
                    string q = $"exec UploadForUserAssignment_sb @userId={userId}, @assignmentId={assignmentId}, @solution='{solution}'";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.ExecuteNonQuery();
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "File uploaded and saved successfully.";

                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please select a file to upload.";
            }
        }

        public void btnReview_Click(object sender, EventArgs e)
        {
            ShowOnlyPanel(PanelReview);
        }

        public void ReviewSubmitbtn_Click(object sender, EventArgs e)
        {
            decimal rating = decimal.Parse(RatingId.Text);
            string comment = CommentId.Text;
            string ckReviewCount = $"exec CheckReviewCountForUser_sb @userId = {userId}, @subcourseId = {subcourseId}";
            SqlCommand checkCmd = new SqlCommand(ckReviewCount, conn);
            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                Response.Write("<script>alert('You have already submitted a review for this subcourse.');</script>");

            }
            else
            {
                string insertReview = $"exec SubmitReviewOfUser_sb @userId={userId}, @subcourseId={subcourseId}, @rating={rating}, @comment='{comment}', @status='Pending'";
                SqlCommand insertCmd = new SqlCommand(insertReview, conn);
                insertCmd.ExecuteNonQuery();
                Response.Write("<script>alert('Review submitted successfully.');</script>");
            }
        }
        public void ShowOnlyPanel(Panel showPanel)
        {
            PanelMCQ.Visible = false;
            PanelAssignment.Visible = false;
            PanelReview.Visible = false;
            if(showPanel!=null)
            { 
                showPanel.Visible = true; 
            }
            

        }

        public void GetCertificate()
        {
            string q = $"exec CompletedSubCourseCertificate_sb @userId={userId},@subcourseId={subcourseId}";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                int total = (int)rd[0];
                int watched = (int)rd[1];
                if (watched ==total && total > 0)
                {
                    Session["userId"] = userId;
                    Session["subcourseId"] = subcourseId;
                    Response.Redirect("Certificate.aspx");
                }
            }   
        }
    }
}