using System;
using System.Collections.Generic;

namespace SGson.Test.Entity
{
	public class NullableCase
	{
		public UInt64? NullableUInt64Undifined { get; set; }
		public UInt64? NullableUInt64Null { get; set; }
		public DateTime? NullableDateTime64MinValue { get; set; }
		public Int64? NullableInt64MaxValue { get; set; }
		public UInt64? NullableUInt64FromDecimal { get; set; }
	}
}