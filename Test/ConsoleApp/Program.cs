using System;
using System.IO;
using System.Reflection;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var pathToDLL = @"G:\Modular\Test\TestWebApplication\bin\Debug\netcoreapp2.1\TestWebApplication.dll";
			Assembly assembly = Assembly.Load(File.ReadAllBytes(pathToDLL));
			var x = new ControllerExecuter.ControllerExecuter(assembly);
			x.ResolveRoutes();
			Console.WriteLine(x.GetRouteFor("/Default/Zest/asd/asa").Path);
			Console.ReadKey();
		}
	}
}
