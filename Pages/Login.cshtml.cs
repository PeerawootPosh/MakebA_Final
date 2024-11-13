using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MakebA_Final.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string ErrorMessage { get; set; }

        private readonly string connectionString = "Server=tcp:mailsystem436.database.windows.net,1433;Initial Catalog=MailSystem;Persist Security Info=False;User ID=Final;Password=MakebA436;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public class InputModel
        {
            [Required(ErrorMessage = "Username is required")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet()
        {
            // สามารถใช้เพื่อจัดการการเริ่มต้นการเรียกหน้า เช่นลบข้อมูลการเข้าสู่ระบบเก่า
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND PasswordHash = HASHBYTES('SHA2_256', @Password)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", Input.Username);
                    command.Parameters.AddWithValue("@Password", Input.Password);

                    await connection.OpenAsync();
                    var result = (int)await command.ExecuteScalarAsync();

                    if (result == 1)
                    {
                        // เข้าสู่ระบบสำเร็จ
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Input.Username)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true // ตั้งค่าให้ session คงอยู่ในหลาย request
                        };

                        await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToPage("/Inbox");
                    }
                    else
                    {
                        // ข้อผิดพลาดเมื่อเข้าสู่ระบบไม่สำเร็จ
                        ErrorMessage = "Invalid username or password.";
                        return Page();
                    }
                }
            }
        }
    }
}
