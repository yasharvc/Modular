using System;
using System.Collections.Generic;
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
	}
}
