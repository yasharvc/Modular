using Contracts.Models;
using System.Collections.Generic;

namespace Contracts
{
	public delegate string StringToString(string input);
	public delegate User UserInfoEventArg();
	public delegate Dictionary<string, string> StringToStringDictionarryArgs();
}