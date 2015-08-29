using System;
using System.Collections.Generic;

namespace SGson.Test.Entity
{
	public class CollectionCase
	{
		public ICollection<int> CollectionUndifined { get; set; }
		public ICollection<double> CollectionNull { get; set; }
		public ICollection<sbyte> CollectionEmpty { get; set; }
		public ICollection<string> CollectionCommon { get; set; }
		public ICollection<ICollection<object>> CollectionInCollection { get; set; }
	}
}