using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SGson
{
	public class JsonString : JsonElement
	{
		protected string value;

		public int Length
		{
			get { return value.Length; }
		}

		public JsonString(string value)
		{
			if (value == null)
			{
				throw new Exception("value == null");
			}
			this.value = value;
		}

		public char this[int index]
		{
			get { return this.value[index]; }
		}

		public static implicit operator JsonString(string value)
		{
			if (value == null)
			{
				throw new Exception("Can't convert null to JsonString");
			}
			return new JsonString(value);
		}

		public static explicit operator string(JsonString element)
		{
			return element.value;
		}
	}
}