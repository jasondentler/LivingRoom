using System.Diagnostics;
using System.Web.Mvc;
using AutoMapper;
using LivingRoom.Models;
using LivingRoom.Models.Listings;
using LivingRoom.Models.Listings.Queries;
using LivingRoom.XmlTv;
using Newtonsoft.Json;

namespace LivingRoom.Controllers
{
    public class ListingsController : Controller
    {
        private readonly SearchByName _searchByNameQuery;
        private readonly ChannelIcon _channelIconQuery;
        private readonly NowPlaying _nowPlayingQuery;

        public ListingsController(
            SearchByName searchByNameQuery, 
            ChannelIcon channelIconQuery,
            NowPlaying nowPlayingQuery)
        {
            _searchByNameQuery = searchByNameQuery;
            _channelIconQuery = channelIconQuery;
            _nowPlayingQuery = nowPlayingQuery;
        }

        public ActionResult SearchByName(string name, int pageNumber)
        {
            var results = _searchByNameQuery.Query(name, pageNumber, 5);
            var viewModel = Mapper
                .Map<PagedResult<Program>, PagedResult<SearchByNameView>>(results);

            var json = JsonConvert.SerializeObject(viewModel);
            Debug.WriteLine(json);
            return Content(json, "application/json");
        }

        public ActionResult NowPlaying(int pageNumber)
        {
            var results = _nowPlayingQuery.Query(pageNumber, 5);
            var viewModel = Mapper
                .Map<PagedResult<Program>, PagedResult<SearchByNameView>>(results);
            var json = JsonConvert.SerializeObject(viewModel);
            Debug.WriteLine(json);
            return Content(json, "application/json");
        }

        public ActionResult ChannelIcon(string id)
        {
            var iconPath = _channelIconQuery.Query(id);
            if (!string.IsNullOrWhiteSpace(iconPath))
            {
                return File(iconPath, "image/gif");
            }
            return Content("");
        }

    }
}
