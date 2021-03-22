using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrgChart.Areas.Identity.Data;
using OrgChart.Data;

[assembly: HostingStartup(typeof(OrgChart.Areas.Identity.IdentityHostingStartup))]
namespace OrgChart.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<OrgChartContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MyDbConnection")));

                services.AddDefaultIdentity<OrgChartUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<OrgChartContext>();
            });
        }
    }
}