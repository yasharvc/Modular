using System;
using System.IO;

namespace ModulesFileUploader
{
	public class ModulesFileUploader
	{
		private readonly string BasePath = "__";
		private readonly string AuthenticationFolderPathInsideRoot = "Auth";

		public bool UploadToAuthenticationFolder(byte[] fileContent,string fileName,string path)
		{
			try
			{
				path = GetFullPathToAuthentication(path);
				Directory.CreateDirectory(path);
				File.WriteAllBytes(Path.Combine(path, fileName), fileContent);
				return true;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private string GetFullPathToAuthentication(string path) => Path.Combine(BasePath, AuthenticationFolderPathInsideRoot, path);

		public byte[] DownloadAuthenticationFile(string path, string fileName) => File.ReadAllBytes(Path.Combine(GetFullPathToAuthentication(path), fileName));
	}
}
