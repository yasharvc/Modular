using System;

namespace WebUtility
{
	internal class ContentIndexProcessor
	{
		public string ToJson()
		{
			return $"\"{(PropertyName.Length > 0 ? PropertyName : VariableName)}\":\"{Value}\"";
		}
		public string VariableName { get; set; } = "";
		public int Index { get; set; } = -1;
		public string PropertyName { get; set; } = "";
		public string Value { get; set; } = "";
		public ContentIndexProcessor()
		{
		}

		public ContentIndexProcessor(string content)
		{
			ExtractData(content);
		}

		public void ExtractData(string content)
		{
			if (content.Contains('['))
			{
				VariableName = content.Substring(0, content.IndexOf('['));
				content = content.Substring(content.IndexOf('[') + 1);
				if (content.Contains("]["))
				{
					Index = Convert.ToInt32(content.Substring(0, content.IndexOf(']')).Replace("[", ""));
					content = content.Substring(content.IndexOf("][") + 2);
					PropertyName = content.Substring(0, content.LastIndexOf(']'));
				}
				else
				{
					var propOrIndex = content.Substring(0, content.IndexOf(']')).Replace("[", "");
					if (char.IsDigit(propOrIndex[0]))
						Index = Convert.ToInt32(propOrIndex);
					else
						PropertyName = propOrIndex;
				}
			}
			else
			{
				VariableName = content.Trim();
			}
		}
	}
}