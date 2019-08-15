using Contracts.Hub;
using Contracts.Meta;
using Dapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using UserWidePermission.Services.Models;

namespace UserWidePermission.Services
{
	public class Services : IService
	{
		public string Name => "UserWidePermission";

		public string Description => "سرویس دسترسی دهی در سطح کاربر";

		[ServiceFunction]
		private bool CreatePermission(UserWidePermissions permissions)
		{
			var conn = new SqlConnection(InvocationHub.GetConnectionString());
			var count = conn.ExecuteScalar<int>("SELECT COUNT(1) FROM [UserWidePermissions] WHERE ModuleName = @ModuleName AND PermissionName=@PermissionName", permissions);
			if (count > 0)
				return false;
			var cnt = conn.Execute("insert into [UserWidePermissions](ModuleName,PermissionName,PermissionTitle) values (@ModuleName,@PermissionName,@PermissionTitle)"
				, permissions);
			return cnt > 0;
		}

		[ServiceFunction]
		private bool SetUserPermission(PermissionSavingData permissionSavingData)
		{
			var conn = new SqlConnection(InvocationHub.GetConnectionString());
			try
			{
				var permissionID = conn.ExecuteScalar<int>("SELECT ID FROM dbo.UserWidePermissions WHERE ModuleName = @ModuleName AND PermissionName = @PermissionName", permissionSavingData);

				var cnt = conn.Execute("INSERT dbo.UserWidePermissionData(PermissionID,UserID,Value)VALUES(@permissionID,@UserID,@Value)"
					, new { permissionID, permissionSavingData.UserID, permissionSavingData.Value });
				return cnt > 0;
			}
			catch
			{
				return false;
			}
		}

		[ServiceFunction]
		public string GetUserPermissions(string ModuleName, int userID)
		{
			var res = new Dictionary<string, bool>();
			var conn = new SqlConnection(InvocationHub.GetConnectionString());
			IEnumerable<dynamic> data = conn.Query("SELECT p.PermissionName,CAST(uwd.Value AS BIT) Value FROM dbo.UserWidePermissionData uwd INNER JOIN dbo.UserWidePermissions p ON p.ID = uwd.PermissionID WHERE uwd.UserID = @userID AND p.ModuleName = @ModuleName"
				, new { userID, ModuleName });
			foreach (var item in data)
				res[item.PermissionName] = item.Value;
			return JsonConvert.SerializeObject(res);
		}
	}
}
