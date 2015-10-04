using System;
using System.Collections.Generic;

namespace SGson.Test.Entity
{
	public class DictionaryCase
	{
		public Dictionary<string, int> DictionaryUndifined { get; set; }
		public Dictionary<string, double> DictionaryNull { get; set; }
		public Dictionary<string, sbyte> DictionaryEmpty { get; set; }
		public Dictionary<string, string> DictionaryCommon { get; set; }
		public Dictionary<string, Dictionary<string, byte>> DictionaryInDictionary { get; set; }
		public Dictionary<uint, string> DictionaryUintKey { get; set; }
		public Dictionary<DateTime?, string> DictionaryNullableDateTimeKey { get; set; }
	}
}