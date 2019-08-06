using System;
using System.Collections.Generic;

namespace Utility
{
	public class JsonHandler
	{
		public string GetElement(string json, string elementName)
		{
			var element = json.Substring(json.IndexOf(":", json.IndexOf($"\"{elementName}\"") + 1) + 1);
			string res = "";
			int depth = 0;
			foreach (var ch in element)
			{
				if (ch == '[' || ch == '{')
					depth++;
				else if (ch == ']' || ch == '}')
				{
					depth--;
					if (depth < 0)
						break;
				}
				else if (ch == ',' && depth == 0)
					break;
				res += ch;
			}
			res = res.Trim();
			if (res.StartsWith('"'))
				res = res.Substring(1, res.Length - 2);
			return res;
		}

		public string GetElementFromArray(string json, int index)
		{
			if (!json.StartsWith('['))
				return "";
			json = json.Substring(1, json.Length - 2);
			string res = "";
			int depth = 0;
			foreach (var ch in json)
			{
				if (ch == '[' || ch == '{')
					depth++;
				else if (ch == ']' || ch == '}')
				{
					depth--;
					if (depth < 0)
						throw new System.Exception();
				}
				else if (ch == ',' && depth == 0)
				{
					index--;
					if (index == -1)
						return res;
					res = "";
					continue;
				}
				res += ch;
			}
			res = res.Trim();
			if (res.StartsWith('"'))
				res = res.Substring(1, res.Length - 2);
			return index > 0 ? "" : res;
		}

		public List<string> GetElementsInsideArray(string json)
		{
			List<string> res = new List<string>();
			var curr = "";
			while ((curr = GetElementFromArray(json, res.Count)) != "")
				res.Add(curr.Contains(":") ? curr : curr.Replace("\"", ""));
			return res;
		}

		public object JsonToObject(string json, Type type)
		{
			var res = Activator.CreateInstance(type);
			var props = ((System.Reflection.TypeInfo)type).DeclaredProperties;
			foreach (var prop in props)
			{
				prop.SetValue(res, GetElement(json, prop.Name).ConvertToType(prop.PropertyType));
			}
			return res;
		}
	}
}
