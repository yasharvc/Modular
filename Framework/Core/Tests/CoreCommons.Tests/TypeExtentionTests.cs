using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace CoreCommons.Tests
{
	[TestClass]
	public class TypeExtentionTests
	{
		public class TestClass {
			public string Name { get; set; }
			public int Age { get; set; }

			public override bool Equals(object obj)
			{
				var x = obj as TestClass;
				return x.Age == Age && Name == x.Name;
			}
		}

		public void TestFunc(TestClass t,int i)
		{

		}

		[TestMethod]
		public void Class_And_PrimitiveType()
		{
			var requests = new List<RequestParameter>
			{
				new RequestParameter
				{
					Name = "name",
					Value = "Yashar"
				},
				new RequestParameter
				{
					Name = "Age",
					Value = "32"
				},

				new RequestParameter
				{
					Name = "i",
					Value = "89"
				}
			};

			ParameterInfo firstParameterInfo = GetType().GetMethod(nameof(TestFunc)).GetParameters()[0];
			ParameterInfo secondParameterInfo = GetType().GetMethod(nameof(TestFunc)).GetParameters()[1];

			var obj = firstParameterInfo.CastToType(requests);
			var i = secondParameterInfo.CastToType(requests);

			Assert.IsTrue(obj.Equals(new TestClass { Age = 32, Name = "Yashar" }));
			Assert.AreEqual(i, 89);
		}
	}
}
