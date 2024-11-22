using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {
        public List<EmailInfo> listEmails = new List<EmailInfo>();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=tcp:celestialfinalproject.database.windows.net,1433;Initial Catalog=Celestial;Persist Security Info=False;User ID=celestial;Password=Easy12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // ดึงชื่อผู้ใช้ที่ล็อกอิน
                    string username = User.Identity.Name ?? "";

                    // ดึงข้อมูล MobilePhone ของผู้ใช้ที่ล็อกอิน
                    string mobilePhone = null;
                    string sqlGetMobile = "SELECT MobilePhone FROM AspNetUsers WHERE UserName = @username";
                    using (SqlCommand command = new SqlCommand(sqlGetMobile, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                mobilePhone = reader.GetString(0);
                            }
                        }
                    }

                    // ถ้าไม่มี Mobile Phone ให้หยุด
                    if (string.IsNullOrEmpty(mobilePhone))
                    {
                        return;
                    }

                    // ตรวจสอบและแสดงข้อมูลจาก table emails ที่เกี่ยวข้องกับ Mobile Phone
                    string sql = "SELECT * FROM emails WHERE emailreceiver = @mobilePhone";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mobilePhone", mobilePhone);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmailInfo emailInfo = new EmailInfo
                                {
                                    EmailID = reader.GetInt32(0).ToString(),
                                    EmailSubject = reader.GetString(1),
                                    EmailMessage = reader.GetString(2),
                                    EmailDate = reader.GetDateTime(3).ToString(),
                                    EmailIsRead = reader.GetBoolean(4) ? "1" : "0",
                                    EmailSender = reader.GetString(5),
                                    EmailReceiver = reader.GetString(6)
                                };

                                listEmails.Add(emailInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching emails: {ErrorMessage}", ex.Message);
            }
        }
    }

    public class EmailInfo
    {
        public string EmailID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public string EmailDate { get; set; }
        public string EmailIsRead { get; set; }
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
    }
}
