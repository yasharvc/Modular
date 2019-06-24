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

		private static object ConvertProperty(Type resType, List<RequestParameter> parameters,string propertyName, Assembly assembly = null)
		{
			if (resType.IsPrimitiveType())
			{
				return Convert.ChangeType(parameters.Single(m => m.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).Value, resType);
			}
			else if (resType == typeof(Guid))
			{
				return new Guid(parameters.Single(m => m.Name.Equals(resType.Name, StringComparison.OrdinalIgnoreCase)).Value);
			}
			else if (resType.IsGenericType == false)//Class
			{
				var res = CreateObjectByType(resType, assembly);
				var props = resType.GetProperties();
				foreach (var prop in props)
				{
					var value = ConvertProperty(prop.PropertyType, parameters, prop.Name, assembly);
					prop.SetValue(res, value);
				}
				return res;
			}
			return null;
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

		//private static object GetPrimitivePropertyValue<FROM>(FROM input, PropertyInfo propInfo)
		//{
		//	var inpType = input.GetType();
		//	return inpType.GetProperty(propInfo.Name).GetValue(input);
		//}
	}
}
