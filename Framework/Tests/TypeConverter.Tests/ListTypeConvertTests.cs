using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeConverter.Tests
{
	[TestClass]
	public class ListTypeConvertTests
	{
		[TestMethod]
		public void PrimitiveTypeStringList()
		{
			var x = new List<string>
			{
				"Math","C++" 
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<List<string>>(x).Count);
		}
		[TestMethod]
		public void PrimitiveTypeIntegerList()
		{
			var x = new List<int>
			{
				200,500,600
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(3, converter.Convert<List<int>>(x).Count);
		}
		[TestMethod]
		public void ListOfPrimitiveTypeStringList()
		{
			var x = new List<List<string>>
			{
				new List<string>{
				"a","b","c"
				},
				new List<string>{
				"1","2","3"
				}
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<List<List<string>>>(x).Count);
		}
		//[TestMethod]
		//public void ClassTypeList()
		//{
		//	var x = new List<TestClassSecondary>
		//	{
		//		new TestClassSecondary
		//		{
		//			Age=12,
		//			Courses=new List<string>{ "Math","C++" },
		//			FirstName = "Yashar"
		//		}
		//	};

		//	TypeConverter converter = new TypeConverter();

		//	Assert.AreEqual(1, converter.Convert<List<TestClass>>(x).Count);
		//}
	}
}
