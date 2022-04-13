using AutoMapper;
using FuncCountdown.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncCountdown.Utilities
{
    /// <summary>
    /// NOT RECOMMENDED THIS NEEDS TO BE CHANGED
    /// </summary>
    public static class MapperDependencyResolver
    {

        public static readonly IMapper _Mapper;
        static MapperDependencyResolver()
        {
            _Mapper =  new MapperConfiguration(
                cfg => cfg.AddProfile(new MapperProfile())).CreateMapper();
        }

    }
}
