using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LivingRoom.Models;

namespace LivingRoom.Controllers
{
    public class ChannelController : Controller
    {
        //
        // GET: /Channel/

        public ActionResult Icon(string id)
        {
            var iconPath = ChannelIconQuery.Query(id);
            if (!string.IsNullOrWhiteSpace(iconPath))
            {
                return File(iconPath, "image/gif");
            }
            return Content("");
        }

    }
}
