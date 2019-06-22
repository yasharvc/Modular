using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace CoreCommons
{
	public static class ReflectionExtention
	{
		public static bool HasCustomAttribute(this MethodInfo method, Type type) => method.GetCustomAttribute(type) != null;

		public static bool HasGetAttribute(this MethodInfo method) => method.HasCustomAttribute(typeof(HttpGetAttribute));
		public static bool HasPostAttribute(this MethodInfo method) => method.HasCustomAttribute(typeof(HttpPostAttribute));
		public static bool HasPutAttribute(this MethodInfo method) => method.HasCustomAttribute(typeof(HttpPostAttribute));
		public static bool HasDeleteAttribute(this MethodInfo method) => method.HasCustomAttribute(typeof(HttpDeleteAttribute));
	}
}
