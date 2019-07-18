using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace TempFolderManager
{
	public sealed class Unzipper : IDisposable
	{
		private readonly string ModulesRootFolderName = TempFolderMaker.ModuleBasePath;
		private readonly string ModulesTempFolderName = TempFolderMaker.TempFolderBasePath;

		public string TempFolderName { get; private set; }

		public string GetFullTempFolderPath() => $"{TempFolderName}";
		public Unzipper(byte[] zipFile,string tempFolderName)
		{
			TempFolderName = tempFolderName;
			Directory.CreateDirectory(GetFullTempFolderPath());
			using (ZipArchive archive = new ZipArchive(new MemoryStream(zipFile)))
			{
				foreach (ZipArchiveEntry entry in archive.Entries)
				{
					var destinationPath = entry.FullName;
					var path = Path.Combine(GetFullTempFolderPath(), destinationPath);
					if (entry.FullName.EndsWith("/"))
						Directory.CreateDirectory(path);
					else
					{
						FileInfo info = new FileInfo(path.Replace("/", "\\"));
						Directory.CreateDirectory(info.DirectoryName);
						File.WriteAllBytes(path.Replace("/", "\\"), ZipEntryToBytes(entry.Open()));
					}
				}
			}
		}

		private byte[] ZipEntryToBytes(Stream stream)
		{
			byte[] bytes;
			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				bytes = ms.ToArray();
			}
			return bytes;
		}

		internal void CutToFolder(string destFolder)
		{
			CopyToFolder(destFolder);
			Directory.Delete(GetFullTempFolderPath(), true);
		}

		internal void Clean()
		{
			try
			{
				Directory.Delete(GetFullTempFolderPath(), true);
			}
			catch { }
		}

		public void Dispose()
		{
			Clean();
		}

		internal void CopyToFolder(string destFolder)
		{
			CopyFilesRecursively(new DirectoryInfo(GetFullTempFolderPath()), new DirectoryInfo(destFolder));
		}

		private void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
		{
			foreach (DirectoryInfo dir in source.GetDirectories())
				CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
			foreach (FileInfo file in source.GetFiles())
			{
				Directory.CreateDirectory(target.FullName);
				file.CopyTo(Path.Combine(target.FullName, file.Name), true);
			}
		}
	}
}
