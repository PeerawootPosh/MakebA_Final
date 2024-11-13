using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MakebA_Final.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProfileModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // ข้อมูลผู้ใช้จำลอง
        public string UserName { get; set; } = "John Doe";
        public string Email { get; set; } = "johndoe@example.com";
        public string DateJoined { get; set; } = "January 1, 2022";

        public void OnGet()
        {
            // เพิ่มข้อมูลที่ต้องการแสดงในหน้า Profile
        }

        // ฟังก์ชันสำหรับ Logout
        public async Task<IActionResult> OnPostAsync()
        {
            // ออกจากระบบ
            await _signInManager.SignOutAsync();
            // เปลี่ยนเส้นทางไปที่หน้า Login
            return RedirectToPage("/Login");
        }
    }
}
