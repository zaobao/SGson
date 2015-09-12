using System;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.TypeAdapters
{
	public class EnumAdapter : ATypeAdapter
	{
		public override JsonElement Serialize(object o)
		{
			return o.ToString();
		}

		public override object Deserialize(JsonElement je, Type originalType)
		{
			if (je == null || je.IsJsonNull)
			{
				return Enum.Parse(originalType, "0");
			}
			if (je.IsJsonString)
			{
				return Enum.Parse(originalType, (string)(JsonString)je);
			}
			if (je.IsJsonNumber)
			{
				return Enum.Parse(originalType, je.ToString());
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to {1}, JsonElement: {2}.", je.GetType(), originalType, je.ToString()));
		}
	}
}