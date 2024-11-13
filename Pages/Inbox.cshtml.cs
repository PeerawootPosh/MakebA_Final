using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MakebA_Final.Pages
{
    [Authorize]  // บังคับให้ผู้ใช้ต้องล็อกอินก่อนเข้าหน้านี้
    public class InboxModel : PageModel
    {
        public void OnGet()
        {
            // ตัวอย่างของการแสดงหน้า Inbox เมื่อผู้ใช้ล็อกอิน
        }
    }
}
