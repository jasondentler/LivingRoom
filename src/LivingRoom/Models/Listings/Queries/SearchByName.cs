using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using AutoMapper;
using LivingRoom.XmlTv;
using NHibernate;

namespace LivingRoom.Models.Listings.Queries
{
    public class SearchByName
    {
        private readonly ISession _session;

        public SearchByName(ISession session)
        {
            _session = session;
        }

        public PagedResult<Program> Query(string name, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new PagedResult<Program>(new Program[] {}, pageNumber, pageSize, 0);
            
            name = "%" + name + "%";

            var firstResult = (pageNumber - 1)*pageSize;

            var results = _session.GetNamedQuery("SearchByName")
                .SetAnsiString("name", name)
                .SetDateTime("now", DateTime.Now)
                .SetFirstResult(firstResult)
                .SetMaxResults(pageSize)
                .Future<Program>();

            _session.GetNamedQuery("SearchByName_Attributes")
                .SetAnsiString("name", name)
                .SetDateTime("now", DateTime.Now)
                .SetFirstResult(firstResult)
                .SetMaxResults(pageSize)
                .Future<Program>();

            var count = _session.GetNamedQuery("SearchByName_Count")
                .SetAnsiString("name", name)
                .SetDateTime("now", DateTime.Now)
                .FutureValue<long>();

            return new PagedResult<Program>(results, pageNumber, pageSize, count.Value);
        }

    }
}