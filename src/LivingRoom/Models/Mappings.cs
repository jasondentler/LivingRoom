using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using LivingRoom.Models.Listings;
using LivingRoom.XmlTv;

namespace LivingRoom.Models
{
    public class Mappings
    {

        public Mappings()
        {
            Mapper.CreateMap<PagedResult<Program>, PagedResult<SearchByNameView>>();
            Mapper.CreateMap<Program, SearchByNameView>()
                .ForMember(
                    c => c.HasIcon,
                    mo => mo.MapFrom(p => !string.IsNullOrEmpty(p.Channel.Icon)));
        }

    }
}