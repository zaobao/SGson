using System;

namespace SGson.Test.Entity
{
	public class EnumCase
	{
		public Days EnumUndifined { get; set; }
		public Days EnumNull { get; set; }
		public Days Monday { get; set; }
		public Days Day6 { get; set; }
		public Days DayString4 { get; set; }
		public Days Day7 { get; set; }
	}

	public enum Days : sbyte { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday };
}