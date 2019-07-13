using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModulesFileUploader
{
	public class FileHelper
	{
		public IEnumerable<string> GetConfilctedFiles(string src,string dest)
		{
			DirectoryInfo dir1 = new DirectoryInfo(src);
			DirectoryInfo dir2 = new DirectoryInfo(dest);

			IEnumerable<FileInfo> list1 = dir1.GetFiles("*.*", SearchOption.AllDirectories);
			IEnumerable<FileInfo> list2 = dir2.GetFiles("*.*", SearchOption.AllDirectories);

			FileCompare myFileCompare = new FileCompare();
			var queryCommonFiles = list1.Intersect(list2, myFileCompare);
			return queryCommonFiles.Select(m => m.FullName);
		}

		public IEnumerable<string> MoveAll(string srcPath,string destPath)
		{
			var exceptions = new List<string>();
			var moved = new List<string>();
			DirectoryInfo srcDir = new DirectoryInfo(srcPath);
			try
			{
				srcDir.MoveTo(destPath);
			}
			catch
			{
				foreach (var file in srcDir.GetFiles("*.*", SearchOption.AllDirectories))
				{
					var relativePath = file.DirectoryName.Replace(srcPath, "");
					if (relativePath.StartsWith("\\"))
						relativePath = relativePath.Substring(1);
					var filePathInDest = Path.Combine(destPath, relativePath);
					Directory.CreateDirectory(filePathInDest);
					try
					{
						file.CopyTo(Path.Combine(filePathInDest, file.Name), true);
						file.Delete();
						moved.Add(Path.Combine(filePathInDest, file.Name));
					}
					catch
					{
						exceptions.Add(file.FullName);
					}
				}
			}
			if (exceptions.Count == 0)
				srcDir.Delete(true);
			else
			{
				DirectoryInfo destDir = new DirectoryInfo(destPath);
				foreach (var filePath in moved)
				{
					var file = new FileInfo(filePath);
					var relativePath = file.DirectoryName.Replace(destPath, "");
					if (relativePath.StartsWith("\\"))
						relativePath = relativePath.Substring(1);
					var filePathInSrc = Path.Combine(srcPath, relativePath);
					Directory.CreateDirectory(filePathInSrc);
					try
					{
						file.MoveTo(Path.Combine(filePathInSrc, file.Name));
					}
					catch
					{
						
					}
				}
				destDir.Delete(true);
			}
			return exceptions;
		}

		class FileCompare : IEqualityComparer<System.IO.FileInfo>
		{
			public FileCompare() { }

			public bool Equals(FileInfo f1, FileInfo f2)
			{
				return (f1.Name == f2.Name &&
						f1.Length == f2.Length);
			} 
			public int GetHashCode(FileInfo fi)
			{
				string s = $"{fi.Name}";
				return s.GetHashCode();
			}
		}
	}
}
