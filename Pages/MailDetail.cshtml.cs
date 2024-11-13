using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MakebA_Final.Pages
{
    [Authorize]
    public class MailDetailModel : PageModel
    {
        private readonly string connectionString = "Server=tcp:mailsystem436.database.windows.net,1433;Initial Catalog=MailSystem;Persist Security Info=False;User ID=Final;Password=MakebA436;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public Mail MailDetail { get; set; }

        public IActionResult OnGet(int id)
        {
            MailDetail = new Mail();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Subject, Sender, Receiver, Content, DateSent FROM Mails WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MailDetail.Subject = reader["Subject"].ToString();
                            MailDetail.Sender = reader["Sender"].ToString();
                            MailDetail.Receiver = reader["Receiver"].ToString();
                            MailDetail.Content = reader["Content"].ToString();
                            MailDetail.DateSent = (DateTime)reader["DateSent"];
                        }
                        else
                        {
                            return RedirectToPage("/Inbox");
                        }
                    }
                }
            }

            return Page();
        }

        public class Mail
        {
            public string Subject { get; set; }
            public string Sender { get; set; }
            public string Receiver { get; set; }
            public string Content { get; set; }
            public DateTime DateSent { get; set; }
        }
    }
}
