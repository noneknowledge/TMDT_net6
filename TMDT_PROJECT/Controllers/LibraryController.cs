using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMDT_Project.ViewModel;
using TMDT_PROJECT.Data;

namespace TMDT_PROJECT.Controllers
{
    public class LibraryController : Controller
    {
        private readonly Game_DBContext _ctx;

        public LibraryController(Game_DBContext context) {
            _ctx = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var uid = User.FindFirst("uid").Value.ToString();
            var data = await _ctx.Libraries.Where(c => c.Uid == uid).Include(a => a.Game).ToListAsync();
            return View(data);
        }
        //
        public void Comment(string comment, string gameid)
        {
            if (User.Identity.IsAuthenticated)
            {

                var uid = User.FindFirst("uid").Value.ToString();
                var item = _ctx.Libraries.FirstOrDefault(a => a.GameId == gameid && a.Uid == uid);
                if (item != null)
                {
                    item.FeedBack = comment;
                    _ctx.Update(item);
                    _ctx.SaveChanges();
                }

            }
        }

        public void EmotionOnGame(string gameid, string value)
        {
            if (User.Identity.IsAuthenticated)
            {

                var uid = User.FindFirst("uid").Value.ToString();
                var item = _ctx.Libraries.FirstOrDefault(a => a.GameId == gameid && a.Uid == uid);
                if (item != null)
                {
                    item.IsLiked = value;
                    _ctx.Update(item);
                    _ctx.SaveChanges();
                }

            }
        }
        public async Task<IActionResult> PersonalGame(string gameid)
        {
            if (User.Identity.IsAuthenticated)
            {
                var uid = User.FindFirst("uid").Value.ToString();
                var model = await _ctx.Libraries.Include(a => a.Game).FirstOrDefaultAsync(a => a.GameId == gameid && a.Uid == uid);
                var comment = await _ctx.Libraries.Where(a => a.GameId == gameid && a.FeedBack.Length>0).Include(a => a.UidNavigation).ToListAsync();
                var data = new LibraryVM();
                data.Uid = model.Uid;
                data.IsLiked = model.IsLiked;
                data.FeedBack = model.FeedBack;
                data.Game = model.Game;
                data.GameId = model.GameId;
                data.libraries = comment;

                return PartialView("~/Views/Partials/_Library.cshtml", data);
            }
            return Json(null);
        }
    }
}
