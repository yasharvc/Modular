using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contracts.Meta
{
	public class FunctionMeta : BaseMeta
	{
		public IEnumerable<FieldMeta> Parameters { get; set; }
		public FieldMeta ReturnType { get; set; }
		public bool IsObsolete { get; set; } = false;
		public string Namespace { get; set; }
		public string ClassName { get; set; }
		public string Description { get; private set; }
		private bool IsVoid { get; set; } = false;
		public FunctionMeta()
		{

		}
		public FunctionMeta(MethodInfo method, ServiceFunction serviceFunction, IService service)
		{
			Namespace = method.DeclaringType.Namespace;
			ClassName = method.DeclaringType.Name;
			Name = method.Name;
			Description = !string.IsNullOrEmpty(serviceFunction.Description) ? @"/// " + serviceFunction.Description.Replace("\n", "\n" + "///") : "";
			List<FieldMeta> parameters = new List<FieldMeta>();
			IsVoid = method.ReturnType.FullName.Equals("System.void", StringComparison.OrdinalIgnoreCase);
			ReturnType = new FieldMeta
			{
				Name = "",
				IsNullable = Nullable.GetUnderlyingType(method.ReturnType) != null
			};
			if (method.ReturnType.IsGenericType)
			{
				ReturnType.FullTypeName = method.ReturnType.UnderlyingSystemType.ToString().Replace("[", "<").Replace("]", ">").Replace("`1", "").Replace("`2", "").Replace("`3", "");
			}
			else
			{
				if (IsVoid)
					ReturnType.FullTypeName = "void";
				else
					ReturnType.FullTypeName = method.ReturnType.FullName;
			}
			var types = method.GetParameters();
			foreach (var type in types)
			{
				var parameter = new FieldMeta
				{
					Name = $"{type.Name}",
					FullTypeName = type.ParameterType.FullName
				};
				parameters.Add(parameter);
			}
			Parameters = parameters;
		}
		public override string ToString()
		{
			var paramList = "";
			var paramsJson = new List<string>();
			foreach (var parameter in Parameters)
			{
				paramList += $"{(paramList.Length > 0 ? ", " : "")}{parameter.FullTypeName.GetSimpleClassName()} {parameter.Name}";
				try
				{
					//if (Type.GetType(parameter.FullTypeName).IsPrimitiveType())
					paramsJson.Add(parameter.Name);
					//else
					//	paramsJson.Add($"Newtonsoft.Json.JsonConvert.SerializeObject({parameter.Name})");
				}
				catch
				{
					paramsJson.Add($"Newtonsoft.Json.JsonConvert.SerializeObject({parameter.Name})");
				}
			}
			var body = $"\t\tvar returnType = typeof({ReturnType.FullTypeName.GetSimpleClassName()});";
			body += $"\r\n\t\t{(IsVoid ? "" : "var result = ")}{ModuleClassName}.Hub.InvocationHub.ServiceInvoke(this.ModuleName,\"{Namespace}.{ClassName}\",\"{Name}\",typeof({ReturnType.FullTypeName.GetSimpleClassName()}){(paramsJson.Count > 0 ? "," : "")}{string.Join(",", paramsJson)});\r\n";
			if (!IsVoid)
				body += $"\t\t{GetReturnPart()}";
			string descriptionXML = "";
			if (!string.IsNullOrEmpty(Description))
			{
				descriptionXML = "\t/// <summary>\r\n" + Description.Replace("///", "\t///") + "\r\n\t/// </summary>";
			}
			string template = $"{descriptionXML}\r\n\tpublic {ReturnType.FullTypeName.GetSimpleClassName()} {Name}({paramList})" + "\r\n\t{\r\n" + body + "\r\n\t}";
			return template;
		}

		private string GetReturnPart()
		{
			if (ReturnType.IsNullable)
				return $"return result as {ReturnType.FullTypeName.GetSimpleClassName()};";
			else if (ReturnType.FullTypeName.ToLower() == "System.Boolean".ToLower())
			{
				return $"return System.Convert.ToBoolean(result);";
			}
			else
				return $"return ({ReturnType.FullTypeName.GetSimpleClassName()})result;";
		}
	}
}