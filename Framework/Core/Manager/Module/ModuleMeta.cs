using Contracts.Meta;
using Contracts.Module;
using CoreCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeConverter;

namespace Manager.Module
{
	public class ModuleMeta
	{
		protected TypeConverter.TypeConverter TypeChanger { get; set; }
		protected Assembly Assembly { get; set; }
		public ServiceMeta ServiceMeta { get; protected set; }
		public ModuleMeta(Assembly asm, ModuleManifest manifest)
		{
			Assembly = asm;
			ServiceMeta = new ServiceMeta
			{
				Name = Assembly.GetName().Name,
				ModuleName = manifest.Name,
				VersionMajor = manifest.Version.Major
			};
			TypeChanger = new TypeConverter.TypeConverter(Assembly);
			ResolveServiceFunctions();
			ResolveServiceModels();
		}

		private void ResolveServiceModels()
		{
			var types = GetModels();
			foreach (var type in types)
			{
				if (type.IsPublic)
				{
					ModelMeta model = new ModelMeta(type);
					ServiceMeta.Models.Add(model);
				}
			}
		}

		private void ResolveServiceFunctions()
		{
			var types = GetServies();
			foreach (var type in types)
			{
				if (type.IsPublic)
				{
					foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
						if (method.GetCustomAttribute(typeof(ServiceFunction)) is ServiceFunction functionAttr)
							ServiceMeta.Functions.Add(new FunctionMeta(method, functionAttr, Activator.CreateInstance(type) as IService));
				}
			}
		}

		private IEnumerable<Type> GetServies() => Assembly.GetTypes().Where(m => m.GetInterface(nameof(IService)) != null);

		private IEnumerable<Type> GetModels() => Assembly.GetTypes().Where(m => m.GetCustomAttribute(typeof(ServiceModel)) != null);

		public void Reload(byte[] AssemblyBytes) => Assembly = Assembly.Load(AssemblyBytes);
		public object CreateObject(string FullTypeName)
		{
			object obj = Assembly.CreateInstance(FullTypeName);
			return obj;
		}
		public object CreateObject(string Namespace, string ClassName) => CreateObject($"{Namespace}.{ClassName}");
		public object InvokeMethod(object obj, string MethodName, Type returnType, params object[] Parameters)
		{
			object res = InvokeMethod(obj, MethodName, Parameters);
			if (returnType != typeof(void))
				return TypeChanger.Convert(res, returnType);
			return null;
		}
		public object InvokeMethod(object obj, string MethodName, params object[] Parameters)
		{
			var methodInfo = obj.GetType().GetMethod(MethodName);
			var parameters = methodInfo.GetParameters();
			var convertedParams = new object[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				convertedParams[i] = TypeChanger.Convert(Parameters[i], parameters[i].ParameterType);
			}
			try
			{
				return obj.GetType().InvokeMember(MethodName, BindingFlags.DeclaredOnly |
													   BindingFlags.Public | BindingFlags.NonPublic |
													   BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, convertedParams);
			}
			catch
			{
				return methodInfo.Invoke(obj, convertedParams);
			}
		}
		public object InvokeMethodWithJson(object obj, string serviceName, string[] Parameters)
		{
			var methodInfo = obj.GetType().GetMethod(serviceName);
			var methodParameters = methodInfo.GetParameters();
			var convertedParams = new object[methodParameters.Length];
			for (int i = 0; i < methodParameters.Length; i++)
			{
				try
				{
					convertedParams[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(Parameters[i], methodParameters[i].ParameterType);
				}
				catch
				{
					if (methodParameters[i].ParameterType.IsPrimitiveType())
					{
						convertedParams[i] = Convert.ChangeType(Parameters[i], methodParameters[i].ParameterType);
					}
					else
					{
						convertedParams[i] = TypeChanger.Convert(Parameters[i], methodParameters[i].ParameterType);
					}
				}
			}
			try
			{
				return obj.GetType().InvokeMember(serviceName, BindingFlags.DeclaredOnly |
													   BindingFlags.Public | BindingFlags.NonPublic |
													   BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, convertedParams);
			}
			catch
			{
				return methodInfo.Invoke(obj, convertedParams);
			}
		}
		public IEnumerable<ParameterInfo> GetMethodParameters(object obj, string MethodName) => obj.GetType().GetMethod(MethodName).GetParameters();
	}
}