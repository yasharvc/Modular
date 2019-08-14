using Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Contracts.Authentication
{
	public abstract class ModuleAdministrationAuthentication : Authentication
	{
		public ModuleAdministrationAuthentication() => Token = "AUTH-9D45C06F-A0F3-48C0-BD0C-ED8DF023CEBE";
		public override string GetDescription() => "دسترسی به مدیریت ماژولها";
	}
}
