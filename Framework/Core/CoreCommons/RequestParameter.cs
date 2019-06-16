using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CoreCommons
{
	public class RequestParameter
	{
		public string Name { get; set; } = "";
		public string Index { get; set; } = "";
		public string PropertyName { get; set; } = "";
		public string Value { get; set; } = "";
		public IFormFile File { get; set; }

		public bool Equals(RequestParameter parameter)
		{
			return parameter.Index == Index &&
				parameter.Name == Name &&
				parameter.PropertyName == PropertyName &&
				parameter.Value == Value;
		}
		public bool IsList() => Index.Length > 0;
		public bool IsFile() => File != null;
		public byte[] GetBytes()
		{
			if (IsFile())
			{
				using (var ms = new MemoryStream())
				{
					File.CopyTo(ms);
					var fileBytes = ms.ToArray();
					return fileBytes;
				}
			}
			return new byte[0];
		}
	}
}
