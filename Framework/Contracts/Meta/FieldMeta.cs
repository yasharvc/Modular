namespace Contracts.Meta
{
	public class FieldMeta : BaseMeta
	{
		public string FullTypeName { get; set; }
		public bool IsNullable { get; set; }
	}
}