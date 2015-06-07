using System;
using System.Collections.Generic;

using SGson;

namespace SGson.Test.Entity
{
	public class AllInOneCase
	{
		public BooleanCase BooleanCase { get; set; }
		private StringCase mStringCase;
		public StringCase StringCase
		{
			get { return mStringCase; }
			set { mStringCase = value; }
		}
		public SbyteCase SbyteCase { get; set; }
		public Int64Case Int64Case { get; set; }
		public UInt64Case UInt64Case { get; set; }
		public DoubleCase DoubleCase { get; set; }
		public ArrayCase ArrayCase { get; set; }
		public DictionaryCase DictionaryCase { get; set; }
		public EnumerableCase EnumerableCase { get; set; }
		public CollectionCase CollectionCase { get; set; }
		public StackCase StackCase { get; set; }
		public QueueCase QueueCase { get; set; }
	}
}