using Contracts.Models;
using System;

namespace Contracts
{
	public class Delegates
	{
		public delegate string StringToString(string input);
		public delegate User UserInfoEventArg();
	}
}
