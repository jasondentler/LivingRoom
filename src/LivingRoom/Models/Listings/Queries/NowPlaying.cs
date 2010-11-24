using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LivingRoom.XmlTv;
using NHibernate;

namespace LivingRoom.Models.Listings.Queries
{
    public class NowPlaying
    {
        private readonly ISession _session;

        public NowPlaying(ISession session)
        {
            _session = session;
        }

        public PagedResult<Program> Query(int pageNumber, int pageSize)
        {
            var firstResult = (pageNumber - 1) * pageSize;

            var results = _session.GetNamedQuery("NowPlaying")
                .SetDateTime("now", DateTime.Now)
                .SetFirstResult(firstResult)
                .SetMaxResults(pageSize)
                .Future<Program>();

            var count = _session.GetNamedQuery("NowPlaying_Count")
                .SetDateTime("now", DateTime.Now)
                .FutureValue<long>();

            return new PagedResult<Program>(results, pageNumber, pageSize, count.Value);
        }

    }
}