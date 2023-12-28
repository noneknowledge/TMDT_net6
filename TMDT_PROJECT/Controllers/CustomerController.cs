using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TMDT_Project.ViewModel;
using TMDT_PROJECT.Data;
using TMDT_PROJECT.Models;

namespace TMDT_PROJECT.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Game_DBContext _ctx;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _envi;
        private readonly IEmailService _emailSender;

        public CustomerController(Game_DBContext context, IMapper mapper, IWebHostEnvironment envi, IEmailService emailService) {
            _ctx = context;
            _mapper = mapper;
            _envi = envi;
            _emailSender = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            var kh = _ctx.Clients.SingleOrDefault(p => p.UserName == username && p.IsActive.ToUpper() == "TRUE" && p.IsHide.ToUpper() == "FALSE");
            if (kh == null)
            {
                ViewBag.ThongBao = "User này không tồn tại hoặc chưa kích hoặc hoặc bị khóa";
                return View();
            }

            if (kh.PassWord != password.ToSHA512Hash(kh.RandomKey))
            {
                ViewBag.ThongBao = "Đăng nhập không thành công";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kh.UserName),
                new Claim("FullName", kh.FullName),
                new Claim("uid", kh.Uid),
                new Claim("Email", kh.Email),
                new Claim("PhoneNumber", kh.Phone)
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimPrincipal);

            return Redirect("/Home/Index");

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            var user = await _ctx.Clients.FirstOrDefaultAsync(a => a.UserName == vm.UserName);
            if (user != null)
            {


                ViewBag.ThongBao = "Tài khoản đã tồn tại";
                return View();
            }
            try
            {

                vm.IsActive = "true";
                vm.IsHide = "false";
                var khachhang = _mapper.Map<Client>(vm);
                khachhang.Uid = Guid.NewGuid().ToString();
                khachhang.RandomKey = Mytool.GetRandom();
                khachhang.PassWord = vm.PassWord.ToSHA512Hash(khachhang.RandomKey);


                string url = "https://localhost:7084/Customer/Confirm/" + khachhang.Uid;
                string subject = "Vui lòng xác thực email";
                _emailSender.sendConfirmMail(khachhang.Email, subject, url, khachhang.FullName);

                _ctx.Add(khachhang);
                await _ctx.SaveChangesAsync();
                return RedirectToAction("Login");


            }
            catch
            {
                return View();
            }

        }
        public IActionResult Confirm(string uid)
        {
            var user = _ctx.Clients.FirstOrDefault(a => a.UserName == uid);
            if (user != null)
            {
                user.IsActive = "true";
            }
            _ctx.SaveChanges();
            return RedirectToAction("Login");
        }





    }
}
