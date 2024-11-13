using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MakebA_Final.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        private readonly string connectionString = "Server=tcp:mailsystem436.database.windows.net,1433;Initial Catalog=MailSystem;Persist Security Info=False;User ID=Final;Password=MakebA436;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public string? ReturnUrl { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "First Name is required")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Last Name is required")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Username is required")]
            [Display(Name = "Username")]
            public string UserName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Phone Number is required")]
            [Phone(ErrorMessage = "Invalid phone number")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; } = string.Empty;

            [Required(ErrorMessage = "Department is required")]
            [Display(Name = "Department")]
            public string Department { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // ตรวจสอบว่า Username หรือ Email มีอยู่ในฐานข้อมูลหรือไม่
                var checkQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email OR Username = @Username";
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Email", Input.Email);
                    checkCommand.Parameters.AddWithValue("@Username", Input.UserName);
                    int exists = (int)checkCommand.ExecuteScalar();

                    if (exists > 0)
                    {
                        ModelState.AddModelError(string.Empty, "Username or Email already exists.");
                        return Page();
                    }
                }

                // ถ้าไม่ซ้ำ ให้เพิ่มข้อมูลลงในฐานข้อมูล
                var query = "INSERT INTO Users (FirstName, LastName, Username, Email, PhoneNumber, Department, PasswordHash) " +
                            "VALUES (@FirstName, @LastName, @Username, @Email, @PhoneNumber, @Department, HASHBYTES('SHA2_256', @Password));";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", Input.FirstName);
                    command.Parameters.AddWithValue("@LastName", Input.LastName);
                    command.Parameters.AddWithValue("@Username", Input.UserName);
                    command.Parameters.AddWithValue("@Email", Input.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", Input.PhoneNumber);
                    command.Parameters.AddWithValue("@Department", Input.Department);
                    command.Parameters.AddWithValue("@Password", Input.Password);  // รหัสผ่านจะถูกเข้ารหัสในฐานข้อมูล

                    command.ExecuteNonQuery();
                }
            }

            SuccessMessage = "Registration completed successfully!";
            return RedirectToPage("Register");
        }
    }
}
