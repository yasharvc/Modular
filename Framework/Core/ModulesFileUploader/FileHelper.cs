using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Contracts.Delegates;

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

		public IEnumerable<string> MoveAll(string srcPath,string destPath,Func<FileInfo,bool> fileIgnore,params FileConverter.FileConverter[] fileConvertors)
		{
			var exceptions = new List<string>();
			var moved = new List<string>();
			DirectoryInfo srcDir = new DirectoryInfo(srcPath);
			if (fileConvertors.Length == 0 && fileIgnore == null)
			{
				try
				{
					srcDir.MoveTo(destPath);
				}
				catch
				{
					foreach (var file in srcDir.GetFiles("*.*", SearchOption.AllDirectories))
						MoveFile(srcPath, destPath, file, moved: moved, exceptions: exceptions);
				}
			}
			else
			{
				if (fileIgnore == null)
				{
					foreach (var file in srcDir.GetFiles("*.*", SearchOption.AllDirectories))
						MoveFile(srcPath, destPath, file, moved: moved, exceptions: exceptions, fileConvertors: fileConvertors);
				}
				else
				{
					foreach (var file in srcDir.GetFiles("*.*", SearchOption.AllDirectories))
					{
						if(fileIgnore(file) == false)
							MoveFile(srcPath, destPath, file, moved: moved, exceptions: exceptions, fileConvertors: fileConvertors);
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
					MoveFile(destPath, srcPath, file, false);
				}
				destDir.Delete(true);
			}
			return exceptions;
		}

		private void MoveFile(string srcPath, string destPath, FileInfo file,bool useCopyDeleteAlgorithm = true, List<string> moved = null, List<string> exceptions = null, params FileConverter.FileConverter[] fileConvertors)
		{
			var relativePath = file.DirectoryName.Replace(srcPath, "");
			if (relativePath.StartsWith("\\"))
				relativePath = relativePath.Substring(1);
			var filePathInDest = Path.Combine(destPath, relativePath);
			Directory.CreateDirectory(filePathInDest);
			try
			{
				if (fileConvertors.Length == 0)
				{
					if (useCopyDeleteAlgorithm)
					{
						file.CopyTo(Path.Combine(filePathInDest, file.Name), true);
						file.Delete();
					}
					else
					{
						file.MoveTo(Path.Combine(filePathInDest, file.Name));
					}
				}
				else
				{
					ConvertFileAndSave(file, fileConvertors, filePathInDest);
				}
				if (moved != null)
					moved.Add(Path.Combine(filePathInDest, file.Name));
			}
			catch
			{
				if (exceptions != null)
					exceptions.Add(file.FullName);
			}
		}

		private void ConvertFileAndSave(FileInfo file, FileConverter.FileConverter[] fileConvertors, string filePathInDest)
		{
			var res = File.ReadAllText(file.FullName);
			foreach (var converter in fileConvertors)
			{
				res = converter.Convert(file.Name, res);
			}
			File.WriteAllText(Path.Combine(filePathInDest, file.Name), res);
			file.Delete();
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
