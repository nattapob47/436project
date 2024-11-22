using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
    public class ReadEmailModel : PageModel
    {
        [BindProperty]
        public string EmailSender { get; set; }
        [BindProperty]
        public string EmailSubject { get; set; }
        [BindProperty]
        public string EmailMessage { get; set; }

        public IActionResult OnGet(int emailid)
        {
            try
            {
                string connectionString = "Server=tcp:celestialfinalproject.database.windows.net,1433;Initial Catalog=Celestial;Persist Security Info=False;User ID=celestial;Password=Easy12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT emailsender, emailsubject, emailmessage FROM emails WHERE emailid = @EmailID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", emailid);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                EmailSender = reader.GetString(0);
                                EmailSubject = reader.GetString(1);
                                EmailMessage = reader.GetString(2);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

            return Page();
        }
    }
}