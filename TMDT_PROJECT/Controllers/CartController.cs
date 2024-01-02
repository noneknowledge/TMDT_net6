using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.WebSockets;
using TMDT_Project.ViewModel;
using TMDT_PROJECT.Data;
using TMDT_PROJECT.Models;
using TMDT_PROJECT.Service;

namespace TMDT_PROJECT.Controllers
{
    public class CartController : Controller
    {
        private readonly IVnPayService _vnpay;
        private readonly PaypalClient _paypal;
        private readonly Game_DBContext _ctx;

        public CartController(Game_DBContext context, PaypalClient paypal, IVnPayService Vnpay) 
        {
            _vnpay = Vnpay;
            _paypal = paypal;
            _ctx = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Remove(string gameid)
        {
            var uid = User.FindFirst("uid").Value.ToString();
            var data = await _ctx.ShoppingCarts.FirstOrDefaultAsync(a => a.Uid == uid && a.GameId == gameid);
            if (data != null)
            {
                _ctx.Remove(data);
                await _ctx.SaveChangesAsync();

            }

            return RedirectToAction("CustomerCart");

        }



        [Authorize]
        public async Task<IActionResult> CustomerCart()
        {
            ViewBag.ClientId = _paypal.ClientId;
            var id = User.FindFirst("uid").Value.ToString();
            var data = await _ctx.ShoppingCarts.Where(a => a.Uid == id).Select(a => new GameVM
            {
                Alias = a.Game.Alias,
                GameId = a.Game.GameId,
                GameDes = a.Game.GameDes,
                GameCategories = a.Game.GameCategories,
                GameImages = a.Game.GameImages,
                GameName = a.Game.GameName,
                Price = a.Game.Price,
                Discount = a.Game.Discount,
                StartDis = a.Game.StartDis,
                EndDis = a.Game.EndDis,
                View = a.Game.View,
                Video = a.Game.Video,
                Dev = a.Game.Dev,
                Publisher = a.Game.Publisher,
                DevId = a.Game.DevId,
                PublisherId = a.Game.PublisherId,
                IsActive = a.Game.IsActive,
                Thumbnail = a.Game.Thumbnail,
            }).ToListAsync();

            return View(data);
        }
        

        public async Task<string> AddToCart(string gameid)
        {
            if (User.Identity.IsAuthenticated)
            {
                var data = await _ctx.Games.FirstOrDefaultAsync(a => a.GameId == gameid);
                if (data != null)
                {
                    var UID = User.Claims.FirstOrDefault(a => a.Type == "uid")?.Value.ToString();
                    var sanpham = await _ctx.ShoppingCarts.FirstOrDefaultAsync(a => a.Uid == UID && a.GameId == gameid);
                    var lib = await _ctx.Libraries.FirstOrDefaultAsync(a => a.Uid == UID && a.GameId == gameid);

                    if (lib != null)
                    {
                        return "Bạn đã mua sản phẩm này";
                    }

                    if (sanpham != null)
                    {
                        return "sản phẩm đã có sản trong giỏ hàng";
                    }


                    var item = new ShoppingCart
                    {
                        GameId = data.GameId,
                        Uid = UID,
                    };

                    _ctx.Add(item);
                    await _ctx.SaveChangesAsync();

                    var tenGame = "Đã thêm thành công " + data.GameName + " vào giỏ hàng";
                    return tenGame;
                }
                return "Đã xảy ra lỗi vui lòng thử lại";
            }
            return "Vui lòng đăng nhập để sử dụng tính năng này";

        }

        [Authorize]
        public IActionResult VnPayCheckOut()
        {
            var uid = User.FindFirst("uid").Value.ToString();
            var cart = _ctx.ShoppingCarts.Where(a => a.Uid == uid).Select(a => new GameVM
            {
                Alias = a.Game.Alias,
                GameId = a.Game.GameId,
                GameDes = a.Game.GameDes,
                GameCategories = a.Game.GameCategories,
                GameImages = a.Game.GameImages,
                GameName = a.Game.GameName,
                Price = a.Game.Price,
                Discount = a.Game.Discount,
                StartDis = a.Game.StartDis,
                EndDis = a.Game.EndDis,
                View = a.Game.View,
                Video = a.Game.Video,
                Dev = a.Game.Dev,
                Publisher = a.Game.Publisher,
                DevId = a.Game.DevId,
                PublisherId = a.Game.PublisherId,
                IsActive = a.Game.IsActive,
                Thumbnail = a.Game.Thumbnail,
            }).ToList();
            var username = User.FindFirst("Fullname").Value;
            var VND = 24000;
            var Total = cart.Sum(a => a.CurrPrice.Value) * VND;
            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = Total,
                FullName = username,
                CreatedDate = DateTime.Now,
                Description = "Thanh toán bằng vn pay. Khách hàng " + username,
                OrderId = new Random().Next(1000,100000),
            };

            return Redirect(_vnpay.CreatePaymentUrl(HttpContext,vnPayModel));
        }

        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }
        [Authorize]
        public IActionResult success()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnpay.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["VnPayFail"] = $"Lỗi thanh toán vn pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            var uid = User.FindFirst("uid").Value.ToString();
            // Tạo đơn hàng (thông tin lấy từ Session???)
            var items =  _ctx.ShoppingCarts.Where(a => a.Uid == uid).Select(a => new GameVM
            {
                Alias = a.Game.Alias,
                GameId = a.Game.GameId,
                GameDes = a.Game.GameDes,
                GameCategories = a.Game.GameCategories,
                GameImages = a.Game.GameImages,
                GameName = a.Game.GameName,
                Price = a.Game.Price,
                Discount = a.Game.Discount,
                StartDis = a.Game.StartDis,
                EndDis = a.Game.EndDis,
                View = a.Game.View,
                Video = a.Game.Video,
                Dev = a.Game.Dev,
                Publisher = a.Game.Publisher,
                DevId = a.Game.DevId,
                PublisherId = a.Game.PublisherId,
                IsActive = a.Game.IsActive,
                Thumbnail = a.Game.Thumbnail,
            }).ToList();
            var tongTien = items.Sum(a => a.CurrPrice).ToString();

            var order = new Order()
            {
                OrderId = Guid.NewGuid().ToString(),
                Uid = uid,
                OrderDate = DateTime.Now,
                Amount = items.Sum(a => a.CurrPrice),
                Total = items.Sum(a => a.CurrPrice),
            };
            foreach (var element in items)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = order.OrderId,
                    GameId = element.GameId,
                    Prices = element.CurrPrice,

                };
                //var lib = new Library()
                //{
                //    GameId = element.GameId,
                //    Uid = uid,
                //};
                //_ctx.Add(lib);
                order.OrderDetails.Add(orderDetail);


            }
            _ctx.Add(order);
             _ctx.SaveChanges();


            var data = _ctx.ShoppingCarts.Where(a => a.Uid == uid);
            _ctx.ShoppingCarts.RemoveRange(data);
             _ctx.SaveChanges();


            return RedirectToAction("success");
        }

        public async Task<IActionResult> RemoveFromCart()
        {
            return View();
        }
    }
}