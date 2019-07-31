﻿using Contracts;
using Contracts.Module;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Manager.Module
{
	public class ModuleAssembly
	{
		private Assembly assembly = null;
		private byte[] _bytes;
		private string _physicalPath;

		public ModuleManifest Manifest { get; }
		public byte[] Bytes { get => _bytes; set { _bytes = value; assembly = Assembly.Load(value); } }
		public string PhysicalPath { get => _physicalPath; set { _physicalPath = value; Bytes = File.ReadAllBytes(value); } }


		//public ModuleAssembly(string name, string token, string physicalPath) : this(Assembly.Load(File.ReadAllBytes(physicalPath))) => PhysicalPath = physicalPath;

		public ModuleAssembly(byte[] bytes)
		{
			Bytes = bytes;
			ResolveNameAndToken();
		}
		public ModuleAssembly(Assembly assembly)
		{
			this.assembly = assembly;
			ResolveNameAndToken();
		}

		private void ResolveNameAndToken()
		{
			var manifest = GetManifest();
			Manifest.Name = manifest.Name;
			Manifest.Token = manifest.Token;
		}

		private ModuleManifest GetManifest() => Activator.CreateInstance(assembly.GetTypes().Single(m => m.IsSubclassOf(typeof(ModuleManifest)))) as ModuleManifest;

		public IThemeProvider GetThemeProvider() => Activator.CreateInstance(assembly.GetTypes().Single(m => m.GetInterface(nameof(IThemeProvider)) != null)) as IThemeProvider;

		public void SetDescription(string desc) => Manifest.Description = desc;

		public static implicit operator Assembly(ModuleAssembly module) => module.assembly;
	}
}
