using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MakebA_Final.Pages
{
    [Authorize]
    public class SentModel : PageModel
    {
        private readonly string connectionString = "Server=tcp:mailsystem436.database.windows.net,1433;Initial Catalog=MailSystem;Persist Security Info=False;User ID=Final;Password=MakebA436;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        // เปลี่ยนเป็น connection string ของคุณ

        public List<Mail> SentMails { get; set; } = new List<Mail>();

        public async Task OnGetAsync()
        {
            var username = User.Identity.Name; // ดึงชื่อผู้ใช้ที่ล็อกอิน

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT Id, Subject, Receiver, Content, DateSent FROM MailDetails WHERE Sender = @Username";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            SentMails.Add(new Mail
                            {
                                Id = reader.GetInt32(0),
                                Subject = reader.GetString(1),
                                Receiver = reader.GetString(2),
                                Content = reader.GetString(3),
                                DateSent = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
        }

        public class Mail
        {
            public int Id { get; set; }
            public string Subject { get; set; }
            public string Receiver { get; set; }
            public string Content { get; set; }
            public DateTime DateSent { get; set; }

            // แสดงพรีวิวของเนื้อหาสั้น ๆ
            public string Preview
            {
                get { return Content.Length > 50 ? Content.Substring(0, 50) + "..." : Content; }
            }
        }
    }
}
