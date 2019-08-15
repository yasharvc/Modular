using Contracts.Hub;
using Contracts.Models;
using Contracts.Module;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UserWidePermission.Services.Models;

namespace UserWidePermission.ViewModels
{
	public class ConfigViewModel
	{
		SqlConnection Connection => new SqlConnection(InvocationHub.GetConnectionString());
		public IEnumerable<ModuleManifest> Modules { get; private set; }
		public IEnumerable<User> Users { get; private set; }

		internal class UserPermissions
		{
			public UserWidePermissions Permission { get; set; }
			public UserWidePermissionData Data { get; set; }
		}
		public ConfigViewModel(bool prepareData)
		{
			if (prepareData)
			{
				Modules = InvocationHub.GetModules();
				Users = InvocationHub.GetUsers();
			}
		}
		public ConfigViewModel() : this(true) { }

		public IEnumerable<UserWidePermissions> GetModulePermissions(string moduleName)
		{
			return Connection.Query<UserWidePermissions>($"SELECT * FROM dbo.UserWidePermissions WHERE ModuleName = '{moduleName}'");
		}

		internal IEnumerable<UserPermissions> GetUserPermission(string moduleName, int userID)
		{
			string query = "SELECT d.*,p.* FROM dbo.UserWidePermissions p LEFT JOIN dbo.UserWidePermissionData d ON d.PermissionID=p.ID AND d.UserID = @userID WHERE ModuleName = @moduleName";
			return Connection.Query<UserWidePermissionData, UserWidePermissions, UserPermissions>(query,
				(d, p) =>
				{
					return new UserPermissions
					{
						Data = d,
						Permission = p
					};
				}
				, new { userID, moduleName });
		}

		internal bool SavePermissionData(IEnumerable<UserWidePermissionData> permissionDatas)
		{
			var delete = "DELETE [UserWidePermissionData] WHERE PermissionID=@PermissionID AND UserID = @UserID";
			var query = "INSERT dbo.UserWidePermissionData(PermissionID,UserID,Value)VALUES(@PermissionID,@UserID,@Value)";
			try
			{
				var conn = Connection;
				conn.Open();
				using (SqlTransaction tran = conn.BeginTransaction())
				{
					try
					{
						foreach (var perm in permissionDatas)
						{
							conn.Execute(delete, perm, tran);
							conn.Execute(query, perm, tran);
						}
						tran.Commit();
						return true;
					}
					catch (Exception ex)
					{
						var str = ex.Message;
						tran.Rollback();
						return false;
					}
				}
			}
			catch (Exception e)
			{
				var str = e.Message;
				return false;
			}
			finally
			{
				Connection.Close();
			}
		}
	}
}