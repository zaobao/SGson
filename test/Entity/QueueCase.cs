using System;
using System.Collections.Generic;

namespace SGson.Test.Entity
{
	public class QueueCase
	{
		public Queue<string> QueueUndifined { get; set; }
		public Queue<object> QueueNull { get; set; }
		public Queue<sbyte> QueueEmpty { get; set; }
		public Queue<object> QueueCommon { get; set; }
		public Queue<Queue<double>> QueueInQueue { get; set; }
	}
}