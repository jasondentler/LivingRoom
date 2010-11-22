using System.Diagnostics;
using System.Web.Mvc;
using LivingRoom.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LivingRoom.Controllers
{
    public class ListingsController : Controller
    {

        public ActionResult SearchByName(string name)
        {
            var programs = SearchByNameQuery.Query(name);
            var json = JsonConvert.SerializeObject(new { programs = programs });
            
            Debug.WriteLine(json);
            return Content(json, "application/json");

        }

    }
}
