using Contracts;
using Contracts.Exceptions.System;
using Contracts.Hub;
using Contracts.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modular.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Modular
{
	public class Startup : IInvocationHubProvider
	{
		public static Manager.Manager Manager { get; } = new Manager.Manager();
		private IHttpContextAccessor httpContextAccessor = null;
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.Strict;
				options.HttpOnly = HttpOnlyPolicy.None;
				options.Secure = true
				  ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
			});

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(60);
				options.Cookie.HttpOnly = true;
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddHttpContextAccessor();

			services.AddScoped<Configuration>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor accessor)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseMvc(routes =>
			{
				routes.MapRoute("areaRoute", "{area:exists}/{controller=Panel}/{action=Index}/{id?}");
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseMvcWithDefaultRoute();
			app.Use(async (context, next) =>
			{
				await next();
				if (context.Response.StatusCode == 404)
				{
					await Handle404(context, next);
				}
			});
			httpContextAccessor = accessor;
			SetupInvocationHubHandling();
		}

		private void SetupInvocationHubHandling()
		{
			InvocationHub.InvocationHubProvider = this;
		}

		private async Task Handle404(HttpContext context, Func<Task> next)
		{
			var req = context.Request;
			var actionContext = CreateActionContext(context);
			AddAdditionalInformationToContext(context);
			RequestHandler.RequestInformation res = GetRequestHandler(context);

			try
			{
				var routeData = Manager.RouterManager.GetRouteFor(req.Path, res);
				var Executer = Manager.GetExecuter(routeData.ModuleName);
				SetModuleNameInHttpContext(context, routeData.ModuleName);

				var x = routeData.GetAuthentcationType(res.Method);

				var auth = Manager.AuthenticationManager.GetAuthenticationByToken(x.Token);
				auth.HttpContext = context;

				if (auth.IsAuthenticated())
				{

					res.ParseAdditionalParameters(routeData.GetQueryString(req.Path));
					var actionResult = Executer.InvokeAction(res, routeData, context);
					context.Response.StatusCode = 200;
					await actionResult.ExecuteResultAsync(actionContext);
				}
				else
				{
					context.Response.Redirect(auth.LoginPagePath);
				}
			}
			catch(MethodRuntimeException ex)
			{
				context.Response.StatusCode = 406;
				await context.Response.WriteAsync($"{ex.Message} <b>{ex.RealStackTrace}</b>");
			}
			catch(AuthenticatonNotFoundException ex)
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync($"<div style='color:red;'>Authentication token not found : {ex.MissedToken}</div>");
			}
			catch (Exception ex)
			{
				if(ex is MethodRuntimeException)
					await context.Response.WriteAsync($"<b>{(ex as MethodRuntimeException).RealStackTrace}</b>");
				context.Response.StatusCode = 418;
				await context.Response.WriteAsync(ex.Message);//$"{res.ContentType} : {string.Join(" AND ", res.RequestParameters.Select(m => $"{m.Name} - {m.Value}"))} = {res.Method}");//string.Join(",",form.Select(m => $"{m.Key} = {m.Value[0]}")));
			}
		}

		private void SetModuleNameInHttpContext(HttpContext context, string moduleName) => context.Items[Consts.CONTEXT_ITEM_KEY_THEME_MODULE_NAME] = moduleName;

		#region Clean Code

		private RequestHandler.RequestInformation GetRequestHandler(HttpContext context)
		{
			var res = new RequestHandler.RequestInformation(context);
			res.ParseRequestData();
			return res;
		}

		private void AddAdditionalInformationToContext(HttpContext context)
		{
			context.Items[Consts.CONTEXT_ITEM_KEY_THEME_LAYOUT_PATH] = Manager.ThemeLayoutPath;
			context.Items[Consts.CONTEXT_ITEM_KEY_CREATOR] ="Yashar";
		}

		private ActionContext CreateActionContext(HttpContext context)
		{
			var routeData = context.GetRouteData() ?? new RouteData();
			return new ActionContext(context, routeData, getActionDescriptor(context));
		}

		//private ControllerExecuter.ControllerExecuter CreateControllerExecuter()
		//{
		//	var pathToDLL = @"G:\Modular\Test\TestWebApplication\bin\Debug\netcoreapp2.1\TestWebApplication.dll";
		//	Assembly assembly = Assembly.Load(File.ReadAllBytes(pathToDLL));
		//	return new ControllerExecuter.ControllerExecuter(assembly);
		//}

		private ActionDescriptor getActionDescriptor(HttpContext context)
		{
			return new ActionDescriptor();
		}

		public string GetConnectionString() => httpContextAccessor.HttpContext.RequestServices.GetRequiredService<Configuration>().DataBaseConnectionString();

		public IEnumerable<ModuleManifest> GetModuleList() => Manager.ModuleManager.GetModules().Select(m => m.Manifest);


		#endregion
	}
}
