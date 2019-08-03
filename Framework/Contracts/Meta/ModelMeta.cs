using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contracts.Meta
{
	public class ModelMeta : BaseMeta
	{
		public String Namespace { get; set; }
		public IEnumerable<FieldMeta> Properties { get; set; }
		public ModelMeta()
		{

		}
		public ModelMeta(Type type)
		{
			Namespace = type.Namespace;
			var props = GetModelProperties(type);
			Name = type.Name;
			List<FieldMeta> properties = new List<FieldMeta>();
			foreach (var prop in props)
				properties.Add(toFieldInformation(prop));
			Properties = properties;
		}

		private FieldMeta toFieldInformation(PropertyInfo prop)
		{
			FieldMeta res = new FieldMeta
			{
				FullTypeName = GetFullName(prop),
				Name = prop.Name
			};
			return res;
		}

		private static string GetFullName(PropertyInfo prop)
		{
			if (prop.PropertyType.IsNullable())
			{
				return $"{prop.PropertyType.GenericTypeArguments[0].FullName}?";
			}
			else
			{
				return prop.PropertyType.FullName;
			}
		}

		public override string ToString()
		{

			var propsStr = "";
			foreach (var prop in Properties)
				propsStr += $"\tpublic {prop.FullTypeName.GetSimpleClassName()} {prop.Name}" + "{ get; set; }\r\n";

			string template =
				$"public class {Name}\r\n{{\r\n{propsStr}\r\n}}";
			return template;
		}

		private PropertyInfo[] GetModelProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}
	}
}