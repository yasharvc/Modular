﻿using Contracts.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Authentication
{
	public class AuthenticationManager
	{
		protected Dictionary<string, IAuthenticationType> Authentications { get; } = new Dictionary<string, IAuthenticationType>();

		public void AddAuthentication(IAuthenticationType authenticationType) => Authentications[authenticationType.Token] = authenticationType;

		public string GenerateNewToken()
		{
			var generator = new AuthenticationGUIDMaker();
			var token = generator.GetNew();
			while (IsAuthenticationExists(token)) token = generator.GetNew();
			return token;
		}

		public bool IsAuthenticationExists(string token) => Authentications.ContainsKey(token);

		public IEnumerable<IAuthenticationType> GetInstalledAuthentications() => Authentications.Values;

		public void Upload(byte[] content,string fileName,string authName)
		{

		}
	}
}