using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TypeConverter.Tests
{
	[TestClass]
	public class TypeConverterTests
	{
		[TestMethod]
		public void IntTypeConverter()
		{
			int i = 100;

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(i, converter.Convert(i, typeof(int)));
		}

		[TestMethod]
		public void DoubleTypeConverter()
		{
			var i = "100.5";

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(100.5, converter.Convert(i, typeof(double)));
		}

		[TestMethod]
		public void StringToGuidTypeConverter()
		{
			var guid = Guid.NewGuid().ToString();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(new Guid(guid), converter.Convert(guid, typeof(Guid)));
		}

		[TestMethod]
		public void GuidToGuidTypeConverter()
		{
			var guid = Guid.NewGuid();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(guid, converter.Convert(guid, typeof(Guid)));
		}

		[TestMethod]
		public void GenericTypeDetector_List()
		{
			var list = new List<string>();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsGeneric(list.GetType()));
		}

		[TestMethod]
		public void GenericTypeDetector_Dictionary()
		{
			var dict = new Dictionary<string,int>();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsGeneric(dict.GetType()));
		}

		[TestMethod]
		public void GenericTypeDetector_Tuple()
		{
			var tuple = new Tuple<string, int, Char>("", 10, 'c');

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsGeneric(tuple.GetType()));
		}

		[TestMethod]
		public void GenericTypeDetector_Class()
		{
			var cls = new TestClass();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsGeneric(cls.GetType()));
		}

		[TestMethod]
		public void GenericTypeDetector_GenericClass()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsGeneric(typeof(TestGenericClass<>)));
		}

		[TestMethod]
		public void IsList_List()
		{
			var list = new List<string>();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsList(list.GetType()));
		}

		[TestMethod]
		public void IsList_IList()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsList(typeof(IList<>)));
		}

		[TestMethod]
		public void IsList_Tuple_2_Item()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsList(typeof(Tuple<int,string>)));
		}

		[TestMethod]
		public void IsList_Tuple_1_Item()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsList(typeof(Tuple<int>)));
		}

		[TestMethod]
		public void IsList_Dictionary()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsList(typeof(Dictionary<,>)));
		}

		[TestMethod]
		public void IsList_Class()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsList(typeof(TestClass)));
		}

		[TestMethod]
		public void IsList_GenericClass()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsList(typeof(TestGenericClass<>)));
		}

		/////////////////////////////////////////////
		[TestMethod]
		public void IsDictionary_List()
		{
			var list = new List<string>();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsDictionary(list.GetType()));
		}

		[TestMethod]
		public void IsDictionary_IList()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsDictionary(typeof(IList<>)));
		}

		[TestMethod]
		public void IsDictionary_Tuple_2_Item()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsDictionary(typeof(Tuple<int, string>)));
		}

		[TestMethod]
		public void IsDictionary_Tuple_1_Item()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsDictionary(typeof(Tuple<int>)));
		}

		[TestMethod]
		public void IsDictionary_Dictionary()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsDictionary(typeof(Dictionary<,>)));
		}

		[TestMethod]
		public void IsDictionary_Class()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsDictionary(typeof(TestClass)));
		}

		[TestMethod]
		public void IsDictionary_GenericClass()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsDictionary(typeof(TestGenericClass<>)));
		}
		/////////////////////////////////////////////

		/////////////////////////////////////////////
		[TestMethod]
		public void IsTuple_List()
		{
			var list = new List<string>();

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsTuple(list.GetType()));
		}

		[TestMethod]
		public void IsTuple_IList()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsTuple(typeof(IList<>)));
		}

		[TestMethod]
		public void IsTuple_Tuple_2_Item()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsTuple(typeof(Tuple<int, string>)));
		}

		[TestMethod]
		public void IsTuple_Tuple_1_Item()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(true, converter.IsTuple(typeof(Tuple<int>)));
		}

		[TestMethod]
		public void IsTuple_Dictionary()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsTuple(typeof(Dictionary<,>)));
		}

		[TestMethod]
		public void IsTuple_Class()
		{
			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(false, converter.IsTuple(typeof(TestClass)));
		}

		[TestMethod]
		public void IsTuple_GenericClass()
		{
			TypeConverter converter = new TypeConverter();
			Assert.AreEqual(false, converter.IsTuple(typeof(TestGenericClass<>)));
		}
		/////////////////////////////////////////////

	}
}
