using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Contracts.Delegates;

namespace ModulesFileUploader
{
	public abstract class FileUploader
	{
		protected List<FileConverter.FileConverter> Convertors { get; } = new List<FileConverter.FileConverter>();

		protected Func<FileInfo, bool> FileIgnorance { get; set; } = f => false;

		protected string BasePath { get; set; } = "wwwroot";

		protected string SubPath { get; set; } = "";

		public virtual bool Move(string sourcePath) => new FileHelper().MoveAll(sourcePath, Path.Combine(BasePath, SubPath), FileIgnorance, Convertors.ToArray()).Count() == 0;
	}
}