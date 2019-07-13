using System.Linq;

namespace ModulesFileUploader
{
	public abstract class FileUploader
	{
		protected string BasePath { get; set; } = "wwwroot";

		public virtual bool Move(string sourcePath) => new FileHelper().MoveAll(sourcePath, BasePath).Count() == 0;
	}
}
