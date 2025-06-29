    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    namespace eLearning.User
    {
        public partial class Certificate : System.Web.UI.Page
        {
            int userId=1;
            int subcourseId;
            SqlConnection conn;
            public string name;
            public string subcoursename;

            protected void Page_Load(object sender, EventArgs e)
            {
                userId = (int)(Session["userId"]);
                subcourseId = (int)(Session["SubcourseId"]);
                string cnf = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                conn = new SqlConnection(cnf);
                conn.Open();
                string userName = GetUserName(userId);
                string subcourseName = GetSubcourseName(subcourseId);
                GenerateCertificate(userName, subcourseName);
            }

            public string GetUserName(int userId)
            {
                string q=$"exec GetUserIdForCertificate_sb {userId}";
                SqlCommand cmd = new SqlCommand(q, conn);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                return result.ToString(); 
                }
                return "";
            }

            public string GetSubcourseName(int subcourseId)
            {
                string q = $"exec GetSubcourseForCertificate_sb {subcourseId}";
                SqlCommand cmd = new SqlCommand(q, conn);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                return result.ToString();
                }
                return "";
            }
            public void GenerateCertificate(string userName, string subcourseName)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
                    PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, BaseColor.BLUE);
                    Font subFont = FontFactory.GetFont(FontFactory.HELVETICA, 18, BaseColor.BLACK);
                    Font bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, BaseColor.DARK_GRAY);

                    Paragraph title = new Paragraph("Certificate of Completion", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);

                    doc.Add(new Paragraph("\n"));

                    Paragraph subTitle = new Paragraph("This certifies that", subFont);
                    subTitle.Alignment = Element.ALIGN_CENTER;
                    doc.Add(subTitle);

                    doc.Add(new Paragraph("\n"));

                    Paragraph name = new Paragraph(userName, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLACK));
                    name.Alignment = Element.ALIGN_CENTER;
                    doc.Add(name);

                    doc.Add(new Paragraph("\n"));

                    Paragraph course = new Paragraph("has successfully completed the subcourse:", subFont);
                    course.Alignment = Element.ALIGN_CENTER;
                    doc.Add(course);

                    doc.Add(new Paragraph("\n"));

                    Paragraph subcourse = new Paragraph(subcourseName, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLUE));
                    subcourse.Alignment = Element.ALIGN_CENTER;
                    doc.Add(subcourse);

                    doc.Add(new Paragraph("\n\n"));

                    Paragraph dateIssued = new Paragraph($"Date of Issue: {DateTime.Now:dd MMMM yyyy}", bodyFont);
                    dateIssued.Alignment = Element.ALIGN_CENTER;
                    doc.Add(dateIssued);

                    // FINALIZE PDF
                    doc.Close();

                    byte[] byteInfo = ms.ToArray();

                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Certificate.pdf");
                    Response.BinaryWrite(byteInfo);
                    Response.End();
                }
            }

        }
    }