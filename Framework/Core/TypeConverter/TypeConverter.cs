using CoreCommons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TypeConverter
{
	public class TypeConverter
	{
		Assembly Assembly { get; set; }

		public TypeConverter(Assembly assembly)
		{
			Assembly = assembly;
		}
		public TypeConverter() : this(null) { }

		public object Convert(object input,Type destType)
		{
			if (input == null)
				return null;
			var inputType = input.GetType();
			if (destType.IsPrimitiveType())
			{
				if (destType == typeof(string))
					return input;
				else
					return System.Convert.ChangeType(input, destType);
			}
			else if (destType == typeof(Guid))
				return new Guid(input.ToString());
			else if (IsGeneric(destType))
				return ConvertGenericType(input, destType);
			else //This is class
				return ConvertClass(input, destType);
		}

		private object ConvertClass(object input, Type destType)
		{
			object res = CreateObjectByType(destType);
			var toProps = destType.GetProperties();
			var fromType = input.GetType();
			foreach (var prop in toProps)
			{
				var inputPropertyValue = fromType.GetProperty(prop.Name).GetValue(input);
				prop.SetValue(res, Convert(inputPropertyValue, prop.PropertyType));
			}
			return res;
		}

		public T Convert<T>(object input) => (T)Convert(input, typeof(T));

		private object ConvertGenericType(object input, Type destType)
		{
			if (IsList(destType))
				return ConvertListType(input, destType);
			else if (IsDictionary(destType))
				return ConvertDictionaryType(input, destType);
			else if (IsTuple(destType))
				return ConvertTupleType(input, destType);
			return null;
		}

		private object ConvertTupleType(object input, Type destType)
		{
			var tempTuple = input as ITuple;
			List<object> pars = new List<object>();
			for (int i = 0; i < tempTuple.Length; i++)
			{
				pars.Add(Convert(tempTuple[i], destType.GetGenericArguments()[i]));
			}
			object res = Activator.CreateInstance(destType, pars.ToArray());
			return res;
		}

		private IEnumerable<object> EnumerateValueTuple(object valueTuple)
		{
			var tuples = new Queue<object>();
			tuples.Enqueue(valueTuple);

			while (tuples.Count > 0 && tuples.Dequeue() is object tuple)
			{
				foreach (var field in tuple.GetType().GetFields())
				{
					if (field.Name == "Rest")
						tuples.Enqueue(field.GetValue(tuple));
					else
						yield return field.GetValue(tuple);
				}
			}
		}

		private object ConvertDictionaryType(object input, Type destType)
		{
			var keyType = destType.GetGenericArguments()[0];
			var valueType = destType.GetGenericArguments()[1];

			var d1 = typeof(Dictionary<,>);
			Type[] typeArgs = { keyType, valueType };
			var makeme = d1.MakeGenericType(typeArgs);
			object res = CreateObjectByType(makeme);
			var keys = input.GetType().GetProperty("Keys").GetValue(input) as IEnumerable;
			var method = input.GetType().GetMethod("get_Item");
			foreach (var key in keys)
			{
				var value = method.Invoke(input, new object[] { key });
				var setter = res.GetType().GetMethod("set_Item");
				setter.Invoke(res, new object[] { Convert(key, keyType), Convert(value, valueType) });
			}
			return res;
		}

		private object ConvertListType(object input, Type destType)
		{
			var genericType = destType.GetGenericArguments()[0];
			var IListRef = typeof(IList<>);
			Type[] IListParam = { genericType };
			object res = CreateObjectByType(destType);
			try
			{
				res.GetType().GetMethod("Add").Invoke(res, new object[] { input });
			}
			catch
			{
				var lst = input as IEnumerable;
				foreach (var item in lst)
				{
					res.GetType().GetMethod("Add").Invoke(res, new object[] { Convert(item, genericType) });
				}
			}
			return res;
		}

		public bool IsGeneric(Type type) => type.IsGenericType;

		public bool IsList(Type type) => IsGeneric(type) && (
			(type.GetGenericTypeDefinition() == typeof(List<>))
			|| (type.GetGenericTypeDefinition() == typeof(IList<>))
			);

		public bool IsDictionary(Type type) => IsGeneric(type) && (
			(type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
			|| (type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
			);

		public bool IsTuple(Type type) => IsGeneric(type) && (
			(type.GetGenericTypeDefinition() == typeof(Tuple<>))
			|| (type.GetGenericTypeDefinition() == typeof(Tuple<,>))
			|| (type.GetGenericTypeDefinition() == typeof(Tuple<,,>))
			|| (type.GetGenericTypeDefinition() == typeof(Tuple<,,,>))
			|| (type.GetGenericTypeDefinition() == typeof(Tuple<,,,,>))
			|| (type.GetGenericTypeDefinition() == typeof(Tuple<,,,,,>))
			);
		public object CreateObjectByType(Type type)
		{
			if (type.IsPrimitiveType())
			{
				if (type == typeof(string))
					return "";
			}
			return Activator.CreateInstance(type);
		}
	}
}