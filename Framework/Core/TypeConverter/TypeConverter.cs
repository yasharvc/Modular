using System;
using System.Collections.Generic;

namespace TypeConverter
{
	public class TypeConverter
	{
		public object Convert(object input,Type destType)
		{
			var inputType = input.GetType();
			if (destType.IsPrimitive)
				return System.Convert.ChangeType(input, destType);
			else if (destType == typeof(Guid))
				return new Guid(input.ToString());
			else if (destType.IsGenericType)
				return true;
			return null;
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
	}
}