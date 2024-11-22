using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

public class ComposeMailModel : PageModel
{
    [BindProperty]
    public string Sender { get; set; }

    [BindProperty]
    public string Recipient { get; set; }

    [BindProperty]
    public string Subject { get; set; }

    [BindProperty]
    public string Body { get; set; }

    public void OnGet(string sender)
    {
        Sender = sender; // รับค่าจาก URL พารามิเตอร์
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            try
            {
                string connectionString = "Server=tcp:celestialfinalproject.database.windows.net,1433;Initial Catalog=Celestial;Persist Security Info=False;User ID=celestial;Password=Easy12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO emails (emailsubject, emailmessage, emaildate, emailisread, emailsender, emailreceiver) VALUES (@subject, @body, @date, 0, @sender, @recipient)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@subject", Subject);
                        command.Parameters.AddWithValue("@body", Body);
                        command.Parameters.AddWithValue("@date", DateTime.Now);
                        command.Parameters.AddWithValue("@sender", Sender);
                        command.Parameters.AddWithValue("@recipient", Recipient);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

          
            return RedirectToPage("/Index");
        }

        return Page(); 
    }


}