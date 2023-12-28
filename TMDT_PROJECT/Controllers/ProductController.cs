using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMDT_Project.ViewModel;
using TMDT_PROJECT.Data;

namespace TMDT_PROJECT.Controllers
{
    public class ProductController : Controller
    {
        private readonly Game_DBContext _ctx;

        public ProductController(Game_DBContext context) {
            _ctx = context;
        }


        public async Task<List<GameVM>?> GetProduct(string orderType, int page)
        {
            //ViewBag.OrderType = orderType;

            var data = await _ctx.Games.Where(a => a.IsActive.ToUpper() == "TRUE").Include(a => a.Dev).Select(a => new GameVM
            {
                Alias = a.Alias,
                GameId = a.GameId,
                GameDes = a.GameDes,
                GameCategories = a.GameCategories,
                GameImages = a.GameImages,
                GameName = a.GameName,
                Price = a.Price,
                Discount = a.Discount,
                StartDis = a.StartDis,
                EndDis = a.EndDis,
                View = a.View,
                Video = a.Video,
                Dev = a.Dev,
                Publisher = a.Publisher,
                DevId = a.DevId,
                PublisherId = a.PublisherId,
                IsActive = a.IsActive,
                Thumbnail = a.Thumbnail,
                Liked = a.Liked,
                Disliked = a.Disliked,
            }).ToListAsync();

            ViewBag.Max = data.Count();

            List<GameVM>? orderby = new List<GameVM>();

            if (orderType == "price")
            {
                orderby = data.OrderByDescending(a => a.CurrPrice).Skip((page - 1) * 9).Take(9).ToList();
            }
            else if (orderType == "view")
            {
                orderby = data.OrderByDescending(a => a.View).Skip((page - 1) * 9).Take(9).ToList();
            }
            else if (orderType == "like")
            {
                orderby = data.OrderByDescending(a => a.Liked).Skip((page - 1) * 9).Take(9).ToList();
            }
            return orderby;
        }

        public async Task<IActionResult> Index(string orderType = "price", int page = 1)
        {
            ViewBag.OrderType = orderType;
            var orderby = await GetProduct(orderType, page);
            return View(orderby);
        }

        public async Task<IActionResult> LoadMore(string orderType, int page)
        {
            var model = await GetProduct(orderType, page);
            if (model.Count() == 0)
            {
                throw new Exception("Sample exception.");
            }
            return PartialView("~/Views/Partials/_Product.cshtml", model);
        }

        public IActionResult OrderBy(string type)
        {
            return RedirectToAction("index", "Product", new { orderType = type });
        }

        [Route("pd/{id}/{alias}")]
        public async Task<IActionResult> Details(string id)
        {
            var a = await _ctx.Games.Include(a => a.Libraries).Include(a => a.Dev).Include(a => a.Publisher).Include(a => a.GameImages).FirstOrDefaultAsync(a => a.GameId == id);
            a.View += 1;
            _ctx.Update(a);
            await _ctx.SaveChangesAsync();
            var game = new GameVM
            {
                Alias = a.Alias,
                GameId = a.GameId,
                GameDes = a.GameDes,
                GameCategories = a.GameCategories,
                GameImages = a.GameImages,
                GameName = a.GameName,
                Price = a.Price,
                Discount = a.Discount,
                StartDis = a.StartDis,
                EndDis = a.EndDis,
                View = a.View,
                Video = a.Video,
                Dev = a.Dev,
                Publisher = a.Publisher,
                DevId = a.DevId,
                PublisherId = a.PublisherId,
                IsActive = a.IsActive,
                Thumbnail = a.Thumbnail,
                Liked = a.Liked,
                Disliked = a.Disliked,

            };
            var comment = await _ctx.Libraries.Where(a => a.GameId == id).Include(a => a.UidNavigation).ToListAsync();
            game.Libraries = comment;
            var cate = await _ctx.GameCategories.Where(a => a.GameId == game.GameId).Include(a => a.Cate).ToArrayAsync();
            ViewBag.Cate = cate;

            return View(game);
        }

        [Route("/dev/{id}/{alias}")]
        public async Task<IActionResult> DevGame(string id)
        {
            var data = await _ctx.Developers.FirstOrDefaultAsync(a => a.DevId == id);
            var model = new Dev_CateVM();
            model.Name = data.Developer1;
            model.Description = data.Description;
            model.Alias = data.Alias;
            model.logo = data.Logo;

            var games = await _ctx.Games.Where(a => a.IsActive.ToUpper() == "TRUE" && (a.DevId == id || a.PublisherId == id)).Include(a => a.Dev).Select(a => new GameVM
            {
                Alias = a.Alias,
                GameId = a.GameId,
                GameDes = a.GameDes,
                GameCategories = a.GameCategories,
                GameImages = a.GameImages,
                GameName = a.GameName,
                Price = a.Price,
                Discount = a.Discount,
                StartDis = a.StartDis,
                EndDis = a.EndDis,
                View = a.View,
                Video = a.Video,
                Dev = a.Dev,
                Publisher = a.Publisher,
                DevId = a.DevId,
                PublisherId = a.PublisherId,
                IsActive = a.IsActive,
                Thumbnail = a.Thumbnail,
            }).ToListAsync();
            model.Games = games;

            return View("~/Views/Product/DevCateGame.cshtml", model);
        }

        [Route("/cate/{id}/{alias}")]
        public async Task<IActionResult> CateGame(string id)
        {
            var data = await _ctx.Categories.FirstOrDefaultAsync(a => a.CateId == id);
            var model = new Dev_CateVM();
            model.Name = data.CateName;
            model.Description = data.CateDes;
            model.Alias = data.Alias;

            var cate_games = await _ctx.GameCategories.Where(a => a.CateId == id).Select(a => a.GameId).ToListAsync();

            var games = await _ctx.Games.Where(a => a.IsActive.ToUpper() == "TRUE" && cate_games.Contains(a.GameId)).Include(a => a.Dev).Select(a => new GameVM
            {
                Alias = a.Alias,
                GameId = a.GameId,
                GameDes = a.GameDes,
                GameCategories = a.GameCategories,
                GameImages = a.GameImages,
                GameName = a.GameName,
                Price = a.Price,
                Discount = a.Discount,
                StartDis = a.StartDis,
                EndDis = a.EndDis,
                View = a.View,
                Video = a.Video,
                Dev = a.Dev,
                Publisher = a.Publisher,
                DevId = a.DevId,
                PublisherId = a.PublisherId,
                IsActive = a.IsActive,
                Thumbnail = a.Thumbnail,
            }).ToListAsync();
            model.Games = games;

            return View("~/Views/Product/DevCateGame.cshtml", model);
        }


        public IActionResult Search()
        {
            ViewBag.Cate = _ctx.Categories.ToList();
            return View();
        }

        public IActionResult SearchResult(string keyword, string filter)
        {
            if (keyword == null) keyword = "";
            if (filter == null)
            {
                var data = _ctx.Games.Where(a => a.GameName.Contains(keyword) && a.IsActive.ToUpper() == "TRUE").Include(a => a.Dev).Select(a => new GameVM
                {
                    Alias = a.Alias,
                    GameId = a.GameId,
                    GameDes = a.GameDes,
                    GameCategories = a.GameCategories,
                    GameImages = a.GameImages,
                    GameName = a.GameName,
                    Price = a.Price,
                    Discount = a.Discount,
                    StartDis = a.StartDis,
                    EndDis = a.EndDis,
                    View = a.View,
                    Video = a.Video,
                    Dev = a.Dev,
                    Publisher = a.Publisher,
                    DevId = a.DevId,
                    PublisherId = a.PublisherId,
                    IsActive = a.IsActive,
                    Thumbnail = a.Thumbnail,
                    Liked = a.Liked,
                    Disliked = a.Disliked,
                }).ToList();
                return PartialView("~/Views/Partials/_Product.cshtml", data);
            }

            var cates = filter.Split(",");

            var gameids = _ctx.GameCategories.Where(a => cates.Contains(a.CateId)).GroupBy(a => a.GameId)
                .Where(group => group.Count() == cates.Length).Select(group => group.Key).ToList();
            var model = _ctx.Games.Where(a => gameids.Contains(a.GameId) && a.GameName.Contains(keyword) && a.IsActive.ToUpper() == "TRUE").Include(a => a.Dev).Select(a => new GameVM
            {
                Alias = a.Alias,
                GameId = a.GameId,
                GameDes = a.GameDes,
                GameCategories = a.GameCategories,
                GameImages = a.GameImages,
                GameName = a.GameName,
                Price = a.Price,
                Discount = a.Discount,
                StartDis = a.StartDis,
                EndDis = a.EndDis,
                View = a.View,
                Video = a.Video,
                Dev = a.Dev,
                Publisher = a.Publisher,
                DevId = a.DevId,
                PublisherId = a.PublisherId,
                IsActive = a.IsActive,
                Thumbnail = a.Thumbnail,
                Liked = a.Liked,
                Disliked = a.Disliked,
            }).ToList();

            return PartialView("~/Views/Partials/_Product.cshtml", model);
        }
    }
}
