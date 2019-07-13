using System.IO;
using System.Linq;

namespace ModulesFileUploader
{
	public class FolderedFileUploader : FileUploader
	{
		public string Folder { get; set; }

		public FolderedFileUploader(string folder) => Folder = folder;

		public override bool Move(string sourcePath) => new FileHelper().MoveAll(sourcePath, Path.Combine(BasePath,Folder)).Count() == 0;

	}
}