using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMDT_Project.ViewModel;
using TMDT_PROJECT.Data;
using TMDT_PROJECT.Models;

namespace TMDT_PROJECT.Controllers
{
    public class CartController : Controller
    {
        private readonly PaypalClient _paypal;
        private readonly Game_DBContext _ctx;

        public CartController(Game_DBContext context, PaypalClient paypal) 
        {
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
        public IActionResult success()
        {
            return View();
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




        public async Task<IActionResult> RemoveFromCart()
        {
            return View();
        }
    }
}