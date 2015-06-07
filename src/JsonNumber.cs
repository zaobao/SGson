using System;

namespace SGson
{
	public class JsonNumber : JsonElement
	{
		protected double value;

		public JsonNumber(double value)
		{
			this.value = value;
		}

		public static implicit operator JsonNumber(double value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(sbyte value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(byte value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(int value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(uint value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(long value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(ulong value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(char value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(decimal value)
		{
			return new JsonNumber((double)value);
		}

		public static implicit operator JsonNumber(string value)
		{
			if (value == null)
			{
				throw new Exception("Can't convert null to JsonNumber");
			}
			try
			{
				return new JsonNumber(Double.Parse(value));
			}
			catch (Exception e)
			{
				throw new Exception(String.Format("Can't convert string \"{}\" to JsonNumber", value), e);
			}
		}

		public static implicit operator double(JsonNumber element)
		{
			return element.value;
		}

		public static explicit operator string(JsonNumber element)
		{
			return element.value.ToString();
		}

		public override string ToString()
		{
			long l;
			if (value >= -9007199254740991 && value <= 9007199254740991 && (l = (long)value) == value)
			{
				return l.ToString();
			}
			return value.ToString();
		}
	}
}