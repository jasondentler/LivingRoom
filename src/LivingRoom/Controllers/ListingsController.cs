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

        public ListingsController(SearchByName searchByNameQuery, ChannelIcon channelIconQuery)
        {
            _searchByNameQuery = searchByNameQuery;
            _channelIconQuery = channelIconQuery;
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
