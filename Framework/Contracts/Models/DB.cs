using System.Data.SqlClient;

namespace Contracts.Models
{
	internal class DB
	{
		public enum DBKind
		{
			Permission,
			Data
		}
		public string GetConnectionString(bool debugMode = false) => Hub.InvocationHub.GetConnectionString();

		public SqlConnection GetConnection(DBKind kind) => new SqlConnection(GetConnectionString());
		public SqlCommand GetCommand(string Query, DBKind kind) => new SqlCommand(Query, GetConnection(kind));

		public object ExecuteScalar(string Query, DBKind kind)
		{
			object res;
			using (var cmd = GetCommand(Query, kind))
			{
				try
				{
					cmd.Connection.Open();
					res = cmd.ExecuteScalar();
				}
				finally
				{
					cmd.Connection.Close();
				}
			}
			return res;
		}
	}
}