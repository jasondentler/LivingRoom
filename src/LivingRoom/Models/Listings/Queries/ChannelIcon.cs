using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LivingRoom.XmlTv;
using NHibernate;

namespace LivingRoom.Models.Listings.Queries
{
    public class ChannelIcon
    {
        private readonly ISession _session;

        public ChannelIcon(ISession session)
        {
            _session = session;
        }

        public string Query(string channelId)
        {
            var channel = _session.Get<Channel>(channelId);
            return channel.Icon;
        }

    }
}