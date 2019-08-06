using System;
using Dapper;
using System.Linq;


namespace Contracts.Models
{
	[Table("Users")]
	public class User
	{
		[Key]
		public int ID { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public int IsActive { get; set; }
		public string FullName { get; set; }
		[NotMapped]
		public string Token { get; set; }

		public bool ExtendTokenTime(string userToken, int minutesToExtend)
		{
			var query = $"UPDATE dbo.UserSession SET EndSession = DATEADD(MINUTE,{minutesToExtend},EndSession) WHERE EndSession > GETDATE() AND DATEDIFF(MINUTE, GETDATE(), EndSession) < {minutesToExtend} AND Token = @Token";
			var connection = new DB().GetConnection(DB.DBKind.Permission);
			var rowCount = connection.Execute(query, new { Token = userToken });
			return rowCount > 0;
		}

		public bool IsUserTypeOf(UserType userType)
		{
			var query = $"SELECT COUNT(1) FROM dbo.UserRoles ur INNER JOIN dbo.Users u ON u.ID = ur.UserID INNER JOIN dbo.Roles r ON r.ID = ur.RoleID WHERE r.RoleName = @RoleName AND u.UserName = @UserName AND u.Password =@Password";
			return new DB().GetConnection(DB.DBKind.Permission).ExecuteScalar<int>(query, new { RoleName = userType.ToString(), UserName, Password }) == 1;
		}

		public string Authenticate(UserType userType)
		{
			var db = new DB();
			var connection = db.GetConnection(DB.DBKind.Permission);
			if (IsUserTypeOf(userType))
			{
				var user = connection.Query<User>("Select * from Users where IsActive = 1 AND UserName = @u and password = @p", new { u = UserName, p = Password }).FirstOrDefault();
				var guid = Guid.NewGuid().ToString();
				connection.Execute($"INSERT dbo.UserSession (Token,UserID,StartSession,EndSession,UserType)VALUES(N'{guid}',{user.ID},GETDATE(),DATEADD(MINUTE,30, GETDATE()),{(int)userType})");
				return guid;
			}
			else
			{
				return "";
			}
		}

		public bool IsTokenValid(string Token)
		{
			if (Token.Trim().Length == 0)
				return false;
			var query = "SELECT COUNT(1) FROM dbo.UserSession WHERE Token = @token AND EndSession > GETDATE()";
			var connection = new DB().GetConnection(DB.DBKind.Permission);
			var x = connection.ExecuteScalar<int>(query, new { token = Token });
			return x > 0;
		}
		public static User GetUserByToken(string token)
		{
			if (string.IsNullOrEmpty(token))
				return new User();
			var query = "SELECT u.* FROM dbo.USERS u INNER JOIN dbo.UserSession s ON s.UserID=u.ID WHERE s.Token=@token";
			var connection = new DB().GetConnection(DB.DBKind.Permission);
			var users = connection.Query<User>(query, new { token });
			if (users.Count() == 1)
				return users.First();
			return new User { ID = -1 };
		}

		public static UserType GetTokenUserType(string token)
		{
			var query = $"SELECT s.UserType FROM dbo.UserSession s WHERE s.Token = '{token}' AND s.EndSession > GETDATE()";
			try
			{
				return (UserType)new DB().GetConnection(DB.DBKind.Permission).ExecuteScalar<int>(query);
			}
			catch
			{
				return UserType.None;
			}
		}

		public bool LogOutByToken(string Token)
		{
			var query = $"UPDATE dbo.UserSession SET EndSession = GETDATE() WHERE Token ='{Token}' AND EndSession > GETDATE()";
			var connection = new DB().GetConnection(DB.DBKind.Permission);
			return connection.Execute(query) > 0;
		}
	}
}