using System;
using System.Collections.Generic;

using SGson;

namespace SGson.Test.Entity
{
	public class StackCase
	{
		public Stack<string> StackUndifined { get; set; }
		public Stack<uint> StackNull { get; set; }
		public Stack<ulong> StackEmpty { get; set; }
		public Stack<object> StackCommon { get; set; }
		public Stack<Stack<byte>> StackInStack { get; set; }
	}
}