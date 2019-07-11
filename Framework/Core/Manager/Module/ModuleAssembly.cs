using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Manager.Module
{
	public class ModuleAssembly
	{
		private Assembly assembly = null;
		private byte[] _bytes;
		private string _physicalPath;

		public ModuleManifest Manifest { get; } = new ModuleManifest();
		public byte[] Bytes { get => _bytes; set { _bytes = value; assembly = Assembly.Load(value); } }
		public string PhysicalPath { get => _physicalPath; set { _physicalPath = value; Bytes = File.ReadAllBytes(value); } }
		

		public ModuleAssembly(string name, string token, string physicalPath)
		{
			PhysicalPath = physicalPath;
			Manifest.Name = name;
			Manifest.Token = token;
			Manifest.Version = assembly.GetName().Version;
		}

		public void SetDescription(string desc) => Manifest.Description = desc;

		public static implicit operator Assembly(ModuleAssembly module) => module.assembly;
	}
}
