using System;
using System.Collections.Generic;

namespace SGson.Test.Entity
{
	public class StackCase
	{
		public class MyStack<T> : Stack<T>
		{

		}

		public class MyStackShort : Stack<short?>
		{

		}
		public Stack<string> StackUndifined { get; set; }
		public Stack<uint> StackNull { get; set; }
		public Stack<ulong> StackEmpty { get; set; }
		public Stack<object> StackCommon { get; set; }
		public Stack<Stack<byte>> StackInStack { get; set; }
		public MyStack<int> MyStack { get; set; }
		public MyStackShort ClosedStack { get; set; }
	}
}