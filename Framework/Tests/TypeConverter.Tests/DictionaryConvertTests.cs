using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeConverter.Tests
{
	[TestClass]
	public class DictionaryConvertTests
	{
		[TestMethod]
		public void SimpleDictionary()
		{
			var dict = new Dictionary<int, string>
			{
				{12,"Yashar" },
				{40,"Samad" }
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<Dictionary<int, string>>(dict).Count);
		}

		[TestMethod]
		public void ClassKeyStringValue()
		{
			var dict = new Dictionary<SimpleClass, string>
			{
				{new SimpleClass{ Age =12, Name="A" },"Yashar" },
				{new SimpleClass{ Age =111, Name="B" },"Samad" }
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<Dictionary<SimpleClass, string>>(dict).Count);
		}

		[TestMethod]
		public void SecondaryClassKeyStringValue()
		{
			var dict = new Dictionary<SimpleClassSecondary, string>
			{
				{new SimpleClassSecondary{ Age =12, Name="A" },"Yashar" },
				{new SimpleClassSecondary{ Age =111, Name="B" },"Samad" }
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<Dictionary<SimpleClass, string>>(dict).Count);
		}

		[TestMethod]
		public void DynamicClassKeyStringValue()
		{
			var dict = new Dictionary<object, string>
			{
				{new { Age =12, Name="A" ,add = true },"Yashar" },
				{new { Age =111, Name="B",add = true },"Samad" }
			};

			TypeConverter converter = new TypeConverter();

			Assert.AreEqual(2, converter.Convert<Dictionary<SimpleClass, string>>(dict).Count);
		}
	}
}
