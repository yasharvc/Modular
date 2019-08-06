using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ModuloContracts.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace WebUtility
{
	public class MVCStartup
	{
		public void Configuration(IWebHostBuilder builder, WebApplicationData environment)
		{
			builder.ConfigureAppConfiguration((hostingContext, config) =>
			{
				environment.Environment = hostingContext.HostingEnvironment;
			});
		}
		public void ConfigureService(IWebHostBuilder builder,Action<IServiceCollection> serviceAdding)
		{
			builder.ConfigureServices(services =>
			{
				services.AddMvc().AddJsonOptions(options =>
				{
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
					options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
					options.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
				});
				//services.Configure<CookiePolicyOptions>(options =>
				//{
				//	options.CheckConsentNeeded = context => true;
				//	options.MinimumSameSitePolicy = SameSiteMode.None;
				//});

				//services.AddDistributedMemoryCache();

				//services.AddSession(options =>
				//{
				//	options.Cookie.Name = "CMMS";
				//	options.IdleTimeout = TimeSpan.FromSeconds(6000);
				//	options.Cookie.HttpOnly = true;
				//	options.Cookie.IsEssential = true;
				//});
				serviceAdding(services);
				services.AddRouting();
				services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			});
		}
	}
}
