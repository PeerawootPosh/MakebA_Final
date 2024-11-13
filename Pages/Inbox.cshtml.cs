using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MakebA_Final.Pages
{
    [Authorize]
    public class InboxModel : PageModel
    {
        private readonly string connectionString = "Server=tcp:mailsystem436.database.windows.net,1433;Initial Catalog=MailSystem;Persist Security Info=False;User ID=Final;Password=MakebA436;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<Mail> Mails { get; set; } = new List<Mail>();

        public void OnGet()
        {
            // ดึงอีเมลทั้งหมดจากฐานข้อมูล
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Id, Subject, Sender, Content, DateSent FROM Mails ORDER BY DateSent DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Mails.Add(new Mail
                        {
                            Id = (int)reader["Id"],
                            Subject = reader["Subject"].ToString(),
                            Sender = reader["Sender"].ToString(),
                            Content = reader["Content"].ToString(),
                            DateSent = (DateTime)reader["DateSent"]
                        });
                    }
                }
            }
        }

        // ฟังก์ชันลบอีเมล
        public IActionResult OnPostDelete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Mails WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            TempData["SuccessMessage"] = "Email deleted successfully.";
            return RedirectToPage();
        }

        public class Mail
        {
            public int Id { get; set; }
            public string Subject { get; set; }
            public string Sender { get; set; }
            public string Content { get; set; }
            public DateTime DateSent { get; set; }

            public string Preview
            {
                get
                {
                    return Content.Length > 50 ? Content.Substring(0, 50) + "..." : Content;
                }
            }
        }
    }
}
