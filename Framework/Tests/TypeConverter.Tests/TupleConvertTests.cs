using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeConverter.Tests
{
	[TestClass]
	public class TupleConvertTests
	{
		[TestMethod]
		public void SimpleTuple()
		{
			Tuple<int> i = new Tuple<int>(10);

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(10, converter.Convert<Tuple<int>>(i).Item1);
		}

		[TestMethod]
		public void ClassTuple()
		{
			Tuple<SimpleClassSecondary> i = new Tuple<SimpleClassSecondary>(new SimpleClassSecondary { Age = 12, Name="ASDAS" });

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(i.Item1.Name, converter.Convert<Tuple<SimpleClass>>(i).Item1.Name);
		}

		[TestMethod]
		public void ListOfStringTuple()
		{
			Tuple<List<string>> i = new Tuple<List<string>>(new List<string> { "ASDAS","2342","$@#$", });

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(i.Item1.Count, converter.Convert<Tuple<List<string>>>(i).Item1.Count);
		}

		[TestMethod]
		public void ListOfObjectsTuple()
		{
			Tuple<List<SimpleClassSecondary>> i = new Tuple<List<SimpleClassSecondary>>(new List<SimpleClassSecondary> {
				new SimpleClassSecondary{ Age = 1 , Name = "ASDAS"},
				new SimpleClassSecondary{ Age = 2, Name = "dfgdf"}
			});

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(i.Item1.Count, converter.Convert<Tuple<List<SimpleClass>>>(i).Item1.Count);
			Assert.AreEqual(i.Item1[0].Name, converter.Convert<Tuple<List<SimpleClass>>>(i).Item1[0].Name);
		}

		[TestMethod]
		public void ListOfDynamicObjectsTuple()
		{
			Tuple<List<object>> i = new Tuple<List<object>>(new List<object> {
				new { add = true, Age = 1 , Name = "ASDAS"},
				new { add = true, Age = 2, Name = "dfgdf"}
			});

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(i.Item1.Count, converter.Convert<Tuple<List<SimpleClass>>>(i).Item1.Count);
			Assert.AreEqual("ASDAS", converter.Convert<Tuple<List<SimpleClass>>>(i).Item1[0].Name);
		}

		[TestMethod]
		public void ListOfDynamicObjectsTuple_2()
		{
			Tuple<List<dynamic>> i = new Tuple<List<dynamic>>(new List<dynamic> {
				new { add = true, Age = 1 , Name = "ASDAS"},
				new { add = true, Age = 2, Name = "dfgdf"}
			});

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(i.Item1.Count, converter.Convert<Tuple<List<SimpleClass>>>(i).Item1.Count);
			Assert.AreEqual("ASDAS", converter.Convert<Tuple<List<SimpleClass>>>(i).Item1[0].Name);
		}

		[TestMethod]
		public void ListOfDynamicObjectsAndIntegerTuple()
		{
			Tuple<List<dynamic>,int> i = new Tuple<List<dynamic>,int>(new List<dynamic> {
				new { add = true, Age = 1 , Name = "ASDAS"},
				new { add = true, Age = 2, Name = "dfgdf"}
			},250);

			TypeConverter converter = new TypeConverter();

			var res = converter.Convert<Tuple<List<SimpleClass>, int>>(i);

			Assert.AreEqual(i.Item1.Count, res.Item1.Count);
			Assert.AreEqual("ASDAS", res.Item1[0].Name);
			Assert.AreEqual(i.Item2, res.Item2);
		}
	}
}
