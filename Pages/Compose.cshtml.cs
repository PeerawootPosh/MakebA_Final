using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MakebA_Final.Pages
{
    public class ComposeModel : PageModel
    {
        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Receiver { get; set; }

        [BindProperty]
        public string Content { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // โค้ดสำหรับบันทึกข้อมูลการส่งเมลไปยังฐานข้อมูล
            // (สมมติว่าการบันทึกสำเร็จ)

            // ตั้งค่าข้อความแจ้งเตือน
            ViewData["AlertMessage"] = "Your email has been sent successfully!";

            return Page();
        }
    }
}
