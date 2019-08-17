using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCommons
{
	public static class TypeExtentions
	{
		public static bool IsPrimitiveType(this object o)
		{
			Type returnType = o is Type ? (Type)o : o.GetType();
			return returnType.IsPrimitive || returnType == typeof(Decimal) || returnType == typeof(string) || returnType == typeof(byte[]);
		}

		public static object CastToType(this ParameterInfo parameterInfo,List<RequestParameter> parameters,Assembly assembly = null)
		{
			var destType = parameterInfo.ParameterType;
			var Name = parameterInfo.Name;
			if (destType == typeof(void))
				return null;
			object res = CreateObjectByType(destType, assembly);
			if (res.GetType() == null)
				return null;
			var resType = res.GetType();
			res = ConvertProperties(resType, parameters, Name, assembly);
			return res;
		}

		private static object ConvertProperties(Type resType, List<RequestParameter> parameters, string parameterName, Assembly assembly = null)
		{
			return ConvertProperty(resType, parameters, parameterName, assembly);
		}

		private static object ConvertProperty(Type resType, List<RequestParameter> parameters,string propertyName, Assembly assembly = null,string name = "")
		{
			if (resType.IsPrimitiveType() || resType == typeof(DateTime))
			{
				object obj = null;
				try
				{
					obj= parameters.Single(m => m.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).Value;
				}
				catch
				{
					obj = parameters.Single(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && m.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).Value;
				}
				return Convert.ChangeType(obj, resType);
			}
			else if (resType == typeof(Guid))
			{
				try
				{
					return new Guid(parameters.Single(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Value);
				}
				catch
				{
					return new Guid(parameters.Single(m => m.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).Value);
				}
			}
			else if (resType.IsGenericType)
			{
				return GetListParameter(resType, propertyName, parameters, assembly);
			}
			else if (resType.IsGenericType == false)//Class
			{
				return GetClassParameter(resType, parameters, assembly, propertyName);
			}
			return null;
		}

		private static object GetListParameter(Type resType, string parameterName, List<RequestParameter> parameters, Assembly assembly)
		{
			var Result = CreateObjectByType(resType);
			var IListRef = typeof(List<>);
			var typeOfT = resType.GetGenericArguments()[0];
			Type[] IListParam = { typeOfT };
			if (typeOfT.IsPrimitiveType() == false)
			{
				var filteredData = parameters.Where(m => m.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase)).OrderBy(m => int.Parse(m.Index));
				var count = filteredData.Select(m => m.Index).Distinct();
				var props = typeOfT.GetDeclaredProperties();
				foreach (var counter in count)
				{
					var currentList = filteredData.Where(m => m.Index == counter);
					var item = CreateObjectByType(typeOfT);
					foreach (var prop in props)
					{
						var value = currentList.Single(m => m.PropertyName.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)).Value;
						prop.SetValue(item, value);
					}
					Result.GetType().GetMethod("Add").Invoke(Result, new object[] { item });
				}
			}
			else
			{
				var filteredData = parameters.Where(m => m.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase));
				foreach (var item in filteredData)
				{
					Result.GetType().GetMethod("Add").Invoke(Result, new object[] { item.Value });
				}
			}
			return Result;

			//try
			//{
			//	Result.GetType().GetMethod("AddRange").Invoke(Result, new object[] { input });
			//}
			//catch
			//{
			//	var lst = input as IEnumerable;
			//	foreach (var item in lst)
			//	{
			//		Result.GetType().GetMethod("Add").Invoke(Result, new object[] { ChangeType(item, typeOfT) });
			//	}
			//}
		}

		public static PropertyInfo[] GetDeclaredProperties(this Type type) => type.GetProperties();

		private static object GetClassParameter(Type resType, List<RequestParameter> parameters, Assembly assembly, string name = "")
		{
			var res = CreateObjectByType(resType, assembly);
			var props = resType.GetDeclaredProperties();
			foreach (var prop in props)
			{
				try
				{
					var value = ConvertProperty(prop.PropertyType, parameters, prop.Name, assembly, name);
					prop.SetValue(res, value);
				}
				catch
				{
					prop.SetValue(res, null);
				}
			}
			return res;
		}

		//private static void PropertyResolver(List<RequestParameter> parameters, object res, PropertyInfo prop)
		//{
		//	if (prop.PropertyType.IsPrimitiveType())
		//		prop.SetValue(res, GetPrimitivePropertyValue(parameters.Single(m => m.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)).Value, prop));
		//	else if (prop.PropertyType is IConvertible)
		//		prop.SetValue(res, Convert.ChangeType(parameters.Single(m => m.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)).Value, prop.PropertyType));
		//	else
		//		prop.SetValue(res, GetClassPropertyValue(res, prop));
		//}

		private static object CreateObjectByType(Type type, Assembly assembly = null)
		{
			try
			{
				return Activator.CreateInstance(type);
			}
			catch
			{
				try
				{
					return assembly.CreateInstance(type.FullName);
				}
				catch
				{
					return null;
				}
			}
		}

		public static bool IsNullable(this Type t)
		{
			try
			{
				return !t.IsPrimitiveType() && t.IsGenericType && Nullable.GetUnderlyingType(t) != null;
			}
			catch
			{
				return false;
			}
		}

		//private static object GetPrimitivePropertyValue<FROM>(FROM input, PropertyInfo propInfo)
		//{
		//	var inpType = input.GetType();
		//	return inpType.GetProperty(propInfo.Name).GetValue(input);
		//}
	}
}
