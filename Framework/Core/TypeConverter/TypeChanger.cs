using CoreCommons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace TypeConverter
{
	public class TypeChanger
	{
		Assembly assembly { get; set; }
		public TypeChanger(Assembly assembly)
		{
			this.assembly = assembly;
		}
		public object ChangeType(object input, Type destType)
		{
			if (destType == typeof(void))
				return null;
			object res = CreateObjectByType(destType);
			if (res.GetType() == null)
				return null;
			var resType = res.GetType();
			if (resType.IsGenericType)
			{
				GetClassListValue(input, res, resType);
			}
			else
			{
				var props = resType.GetProperties();

				foreach (var prop in props)
					PropertyResolver(input, res, prop);
			}
			return res;
		}

		public void GetClassListValue(object input, object Result, Type propertyType)
		{
			var typeOfInput = input.GetType();
			var IListRef = typeof(List<>);
			var typeOfT = propertyType.GetGenericArguments()[0];
			Type[] IListParam = { typeOfT };
			//object Result = CreateObjectByType(IListRef.MakeGenericType(IListParam));
			try
			{
				Result.GetType().GetMethod("AddRange").Invoke(Result, new object[] { input });
			}
			catch
			{
				var lst = input as IEnumerable;
				foreach (var item in lst)
				{
					Result.GetType().GetMethod("Add").Invoke(Result, new object[] { ChangeType(item, typeOfT) });
				}
			}
		}

		public object ChangeType<FROM>(FROM input, Type destType)
		{
			object res = CreateObjectByType(destType);
			var toProps = res.GetType().GetProperties();
			var fromType = typeof(FROM);
			foreach (var prop in toProps)
			{
				PropertyResolver(input, res, prop);
			}
			return res;
		}

		public TO ChangeType<TO>(object input) where TO : class, new()
		{
			Type toType = typeof(TO);
			return ChangeType(input, toType) as TO;
		}

		private void GetClassDictionaryValue(object input, object res, Type type)
		{
			Type keyType = type.GetGenericArguments()[0];
			Type valueType = type.GetGenericArguments()[1];
			Type d1 = typeof(Dictionary<,>);
			Type[] typeArgs = { keyType, valueType };
			Type makeme = d1.MakeGenericType(typeArgs);
			var property = input.GetType().GetMethod("get_Item").Invoke(input, new object[] { });
			var keys = property.GetType().GetProperty("Keys").GetValue(property) as IEnumerable;
			foreach (var key in keys)
			{
				var method = property.GetType().GetMethod("get_Item");
				var value = method.Invoke(property, new object[] { key });
				var setter = res.GetType().GetMethod("set_Item");
				setter.Invoke(res, new object[] { key, ChangeType(value, valueType) });
			}
		}
		private void PropertyResolverByType(object input, object res, PropertyInfo prop)
		{
			try
			{
				bool isGeneric = prop.PropertyType.IsGenericType;
				bool isDictionary = prop.PropertyType.FullName.StartsWith("System.Collections.Generic.Dictionary");
				if (isGeneric)
				{
					if (!isDictionary)
						prop.SetValue(res, GetListPropertyValue(input, prop));
					else
						prop.SetValue(res, GetDictionaryPropertyValue(input, prop));
				}
				else if (prop.PropertyType.IsPrimitiveType())
					prop.SetValue(res, GetPrimitivePropertyValue(input, prop));
				else if (prop.PropertyType is IConvertible)
					prop.SetValue(res, Convert.ChangeType(input, prop.PropertyType));
				else
					prop.SetValue(res, GetClassPropertyValue(input, prop));
			}
			catch
			{
				prop.SetValue(res, null);
			}
		}

		private object GetDictionaryPropertyValue(object input, PropertyInfo prop)
		{
			Type keyType = prop.PropertyType.GetGenericArguments()[0];
			Type valueType = prop.PropertyType.GetGenericArguments()[1];
			Type d1 = typeof(Dictionary<,>);
			Type[] typeArgs = { keyType, valueType };
			Type makeme = d1.MakeGenericType(typeArgs);
			object res = CreateObjectByType(makeme);
			var property = input.GetType().GetProperty(prop.Name).GetValue(input);
			var keys = property.GetType().GetProperty("Keys").GetValue(property) as IEnumerable;
			foreach (var key in keys)
			{
				var method = property.GetType().GetMethod("get_Item");
				var value = method.Invoke(property, new object[] { key });
				var setter = res.GetType().GetMethod("set_Item");
				setter.Invoke(res, new object[] { key, ChangeType(value, valueType) });
			}
			return res;
		}

		private void PropertyResolver<FROM, TO>(FROM input, TO res, PropertyInfo prop) where TO : class, new()
		{
			PropertyResolverByType(input, res, prop);
		}

		private object GetClassPropertyValue(object input, PropertyInfo propInfo)
		{
			var props = propInfo.PropertyType.GetProperties();
			object res = CreateObjectByType(propInfo.PropertyType);
			var typeOfInput = input.GetType();
			var x = typeOfInput.GetProperty(propInfo.Name).GetValue(input);
			typeOfInput = x.GetType();
			foreach (var prop in props)
				PropertyResolverByType(x, res, prop);
			return res;
		}

		private object GetPrimitivePropertyValue<FROM>(FROM input, PropertyInfo propInfo)
		{
			var inpType = input.GetType();
			return inpType.GetProperty(propInfo.Name).GetValue(input);
		}

		private object CreateObjectByType(Type type)
		{
			try
			{
				return Activator.CreateInstance(type);
			}
			catch
			{
				return assembly.CreateInstance(type.FullName);
			}
		}

		private object GetListPropertyValue<FROM>(FROM input, PropertyInfo propInfo)
		{
			Type propertyType = propInfo.PropertyType;
			var typeOfInput = input.GetType();
			var IListRef = typeof(List<>);
			var typeOfT = propertyType.GenericTypeArguments[0];
			Type[] IListParam = { typeOfT };
			object Result = CreateObjectByType(IListRef.MakeGenericType(IListParam));
			try
			{
				Result.GetType().GetMethod("AddRange").Invoke(Result, new object[] { typeOfInput.GetProperty(propInfo.Name).GetValue(input) });
			}
			catch
			{
				var lst = typeOfInput.GetProperty(propInfo.Name).GetValue(input) as IEnumerable;
				foreach (var item in lst)
				{
					Result.GetType().GetMethod("Add").Invoke(Result, new object[] { ChangeType(item, typeOfT) });
				}
			}
			return Result;
		}
	}
}
