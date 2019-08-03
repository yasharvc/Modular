using System.Collections.Generic;
using System.Linq;

namespace Contracts.Meta
{
	public class ServiceMeta : BaseMeta
	{
		protected List<string> Imports => new List<string> {
			"using System;",
			"using System.Collections.Generic;"
		};
		public List<ModelMeta> Models { get; set; } = new List<ModelMeta>();
		public List<FunctionMeta> Functions { get; set; } = new List<FunctionMeta>();
		public string ModuleName { get; set; }
		public int VersionMajor { get; set; }
		public override string ToString()
		{
			var models = $"{string.Join("\r\n", Models.Select(m => m.ToString()))}";
			models += models.Length > 0 ? "\r\n\r\n" : "";
			var functions = string.Join("\r\n", Functions.Select(m => m.ToString().Replace("\t", "\t\t")));
			var ctor = $"\r\n\t\tpublic {Name.Replace(".","_")}Services()" + " { }";
			functions = "public class " + Name.Replace(".","_") + "Services\r\n\t{\r\n\t\tprotected string ModuleName=\"" + ModuleName + "\";\r\n\t\tprotected int VersionMajor=" + VersionMajor + ";" + ctor + "\r\n" + functions
				+ $"\r\n\t\tpublic static implicit operator {ModuleClassName}.Module.Dependency(" + Name.Replace(".","_") + "Services s) { return new " + ModuleClassName + ".Module.Dependency { ModuleName = s.ModuleName, AcceptableMajor = s.VersionMajor }; }"
				+ "\r\n\t}";
			string imports = $"{string.Join("\r\n", Imports)}\r\n";
			string template = imports + "namespace " + Name.Replace(".","_") + "\r\n{\r\n\t" + models.Replace("\r\n", "\r\n\t") + functions.Replace("\r\n", "\r\n") + "\r\n}";
			return template;
		}
	}
}