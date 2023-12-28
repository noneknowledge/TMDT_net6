using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMDT_PROJECT.Data;

namespace TMDT_PROJECT.Controllers
{
    public class OrderController : Controller
    {
        private readonly Game_DBContext _ctx;

        public OrderController(Game_DBContext context) {
            _ctx = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var uid = User.FindFirst("uid").Value.ToString();
            var orders = _ctx.Orders.Where(a => a.Uid == uid).Include(a => a.OrderDetails);

            return View(orders);
        }

        public IActionResult OrderDetail(string Odetail)
        {
            var orderdetail = _ctx.OrderDetails.Where(a => a.OrderId == Odetail).Include(a => a.Game);
            return View(orderdetail);
        }
    }
}
