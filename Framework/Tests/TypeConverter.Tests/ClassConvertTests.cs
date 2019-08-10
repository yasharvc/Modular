using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeConverter.Tests
{
	[TestClass]
	public class ClassConvertTests
	{
		[TestMethod]
		public void SimpleClassToSimpleClass()
		{
			var x = new SimpleClass { Age = 12, Name = "ASDAS" };

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(x.Name, converter.Convert<SimpleClass>(x).Name);
		}

		[TestMethod]
		public void DynamicObjectToSimpleClass()
		{
			var x = new { Age = 12, Name = "ASDAS", Family="ASDAS" };

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(x.Name, converter.Convert<SimpleClass>(x).Name);
		}

		[TestMethod]
		public void ListOfSimpleClass()
		{
			var x = new List<SimpleClass>{
				new SimpleClass { Age = 12, Name = "ASDAS" },
				new SimpleClass { Age = 402, Name = "sdfsd" }
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<List<SimpleClass>>(x).Count);
		}


		[TestMethod]
		public void ListOfDynamicClass()
		{
			var x = new List<object>{
				new { Age = 12, Name = "ASDAS",Add=true },
				new { Age = 402, Name = "sdfsd",Add=true }
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<List<SimpleClass>>(x).Count);
		}


		[TestMethod]
		public void ListOfClassWithListInside()
		{
			var x = new List<TestClassSecondary>{
				new TestClassSecondary{ Age = 12, FirstName="Yashar", Courses=new List<string>{ "C","C++" } },
				new TestClassSecondary{ Age = 32, FirstName="SAS", Courses=new List<string>{ "Python","R" } },
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<List<TestClass>>(x).Count);
		}
	}
}
