using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SGson.IO;

namespace SGson
{
	public abstract class JsonElement
	{
		public bool IsJsonMap
		{
			get { return this is JsonMap; }
		}
		public bool IsJsonArray
		{
			get { return this is JsonArray; }
		}
		public bool IsJsonNull
		{
			get { return this is JsonNull; }
		}
		public bool IsJsonBoolean
		{
			get { return this is JsonBoolean; }
		}
		public bool IsJsonNumber
		{
			get { return this is JsonNumber; }
		}
		public bool IsJsonString
		{
			get { return this is JsonString; }
		}

		public static implicit operator JsonElement(bool value)
		{
			return new JsonBoolean(value);
		}

		public static implicit operator JsonElement(double value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonElement(sbyte value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonElement(byte value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonElement(int value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonElement(uint value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonElement(long value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonElement(ulong value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonElement(decimal value)
		{
			return new JsonNumber((double)value);
		}

		public static implicit operator JsonElement(string value)
		{
			if (value == null)
			{
				return JsonNull.Instance;
			}
			return new JsonString(value);
		}

		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter();
			JsonWriter jsonWriter = new JsonWriter(stringWriter);
			jsonWriter.Write(this);
			return stringWriter.ToString();
		}

	}
}