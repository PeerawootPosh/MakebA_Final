using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MakebA_Final.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly string connectionString = "Server=tcp:mailsystem436.database.windows.net,1433;Initial Catalog=MailSystem;Persist Security Info=False;User ID=Final;Password=MakebA436;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        // เปลี่ยนเป็น connection string ของคุณ

        [BindProperty]
        public RegisterModel.InputModel Input { get; set; } = new RegisterModel.InputModel();

        public async Task OnGetAsync()
        {
            var username = User.Identity.Name;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT FirstName, LastName, Username, Email, PhoneNumber, Department
                              FROM Users 
                              WHERE Username = @Username";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Input.FirstName = reader["FirstName"].ToString();
                            Input.LastName = reader["LastName"].ToString();
                            Input.UserName = reader["Username"].ToString();
                            Input.Email = reader["Email"].ToString();
                            Input.PhoneNumber = reader["PhoneNumber"].ToString();
                            Input.Department = reader["Department"].ToString();
                        }
                    }
                }
            }
        }
    }
}
