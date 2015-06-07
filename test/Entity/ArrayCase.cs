using System;
using System.Collections.Generic;

using SGson;

namespace SGson.Test.Entity
{
	public class ArrayCase
	{
		public int[] ArrayUndifined { get; set; }
		public ulong[] ArrayNull { get; set; }
		public sbyte[] ArrayEmpty { get; set; }
		public double[,,] MultidimensionalArray { get; set; }
		public double[,,] MultidimensionalArrayEmpty0 { get; set; }
		public double[,,] MultidimensionalArrayEmpty1 { get; set; }
		public double[,,] MultidimensionalArrayEmpty2 { get; set; }
		public string[][] JaggedArray { get; set; }
		public string[][] JaggedArrayEmpty0 { get; set; }
		public string[][] JaggedArrayEmpty1 { get; set; }
	}
}