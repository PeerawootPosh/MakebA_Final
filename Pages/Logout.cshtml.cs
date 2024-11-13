using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MakebA_Final.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
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
