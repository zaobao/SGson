using System;
using System.Collections.Generic;

using SGson;

namespace SGson.Test.Entity
{
	public class DictionaryCase
	{
		public Dictionary<string, int> DictionaryUndifined { get; set; }
		public Dictionary<string, double> DictionaryNull { get; set; }
		public Dictionary<string, sbyte> DictionaryEmpty { get; set; }
		public Dictionary<string, string> DictionaryCommon { get; set; }
		public Dictionary<string, Dictionary<string, byte>> DictionaryInDictionary { get; set; }
	}
}