using System.IO;
using System.Linq;

namespace ModulesFileUploader
{
	public class FolderedFileUploader : FileUploader
	{
		public string Folder { get; set; }

		public FolderedFileUploader(string folder) => Folder = folder;

		protected string SourcePathFolder { get; set; } = "";

		public override bool Move(string sourcePath) => new FileHelper().MoveAll(Path.Combine(sourcePath, SourcePathFolder), Path.Combine(BasePath, Folder, SourcePathFolder), FileIgnorance, Convertors.ToArray()).Count() == 0;

	}
}