using System.Collections.Generic;

namespace TypeConverter.Tests
{
	internal class TestClass
	{
		public int Age { get; set; }
		public List<string> Courses { get; set; }
		public string FirstName { get; set; }
	}

	internal class TestClassSecondary
	{
		public int Age { get; set; }
		public List<string> Courses { get; set; }
		public string FirstName { get; set; }
	}

	internal class TestGenericClass<T>
	{
		public T Data { get; set; }
	}

	internal class SimpleClass
	{
		public int Age { get; set; }
		public string Name { get; set; }
	}

	internal class SimpleClassSecondary
	{
		public int Age { get; set; }
		public string Name { get; set; }
	}
}
