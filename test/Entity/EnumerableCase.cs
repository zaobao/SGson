using System;
using System.Collections.Generic;

namespace SGson.Test.Entity
{
	public class EnumerableCase
	{
		public IEnumerable<int> EnumerableUndifined { get; set; }
		public IEnumerable<double> EnumerableNull { get; set; }
		public IEnumerable<sbyte> EnumerableEmpty { get; set; }
		public IEnumerable<string> EnumerableCommon { get; set; }
		public IEnumerable<IEnumerable<float>> EnumerableInEnumerable { get; set; }
	}
}