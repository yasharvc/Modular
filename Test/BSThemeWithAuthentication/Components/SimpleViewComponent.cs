using Contracts.ViewComponent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BSThemeWithAuthentication.Components
{
	[ViewComponent(Name ="KPI")]
	public class SimpleViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			//SqlConnection connection = new SqlConnection(InvocationHub.GetConnectionString());
			//SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM dbo.Machines", connection);
			string count = "10";
			//try
			//{
			//	connection.Open();
			//	count = command.ExecuteScalar().ToString();
			//}
			//catch
			//{
			//	count = "خطایی هنگام انجام عملیات رخ داد";
			//}
			//finally
			//{
			//	if (connection.State == System.Data.ConnectionState.Open)
			//		connection.Close();
			//}
			return await Task.FromResult(GetView("TestModule", model: count));
		}
	}
}
