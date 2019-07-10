using System;
using System.IO;

namespace ModulesFileUploader
{
	public class ModulesFileUploader
	{
		private readonly string BasePath = "__";
		private readonly string AuthenticationFolderPathInsideRoot = "Auth"

		public bool UploadToAuthenticationFolder(byte[] fileContent,string fileName,string path)
		{
			try
			{
				path = Path.Combine(BasePath, AuthenticationFolderPathInsideRoot, path);
				Directory.CreateDirectory(path);
				File.WriteAllBytes(Path.Combine(path, fileName), fileContent);
				return true;
			}catch(Exception e)
			{
				throw e;
			}
		}
	}
}
