using System;
using System.Collections.Generic;

using SGson;

namespace SGson.Test.Entity
{
	public class DoubleCase
	{
		public double DoubleUndifined { get; set; }
		public double DoubleNull { get; set; }
		public double DoubleMinValue { get; set; }
		public Double DoubleMaxValue { get; set; }
		public Double DoubleFromBinary { get; set; }
		public Double DoubleFromOct1 { get; set; }
		public Double DoubleFromOct2 { get; set; }
		public Double DoubleFromHex { get; set; }
		public Double DoubleFromInt { get; set; }
		public Double DoublePoint { get; set; }

		public double DoubleEpsilon { get; set; }
		public double DoubleUnderflow  { get; set; }
		public double DoubleNotUnderflow  { get; set; }
	}
}