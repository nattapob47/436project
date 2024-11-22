using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
    public class DeleteEmailModel : PageModel
    {
        public IActionResult OnGet(int emailid)
        {
            try
            {
                string connectionString = "Server=tcp:celestialfinalproject.database.windows.net,1433;Initial Catalog=Celestial;Persist Security Info=False;User ID=celestial;Password=Easy12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM emails WHERE emailID = @EmailID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", emailid);
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToPage("/Error");
            }
        }
    }
}
