using FuncCountdown.Functions;
using FuncCountdown.Interfaces;
using FuncCountdown.Utilities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FuncStartup))]
namespace FuncCountdown.Functions
{
    public class FuncStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddLogging();
            builder.Services.AddAutoMapper(typeof(FuncStartup));
            builder.Services.AddScoped<IEventDetailsService, EventDetailsService>();
            builder.Services.AddScoped<IEventDetailsRepository, EventDetailsRepo>();
        }
    }
}