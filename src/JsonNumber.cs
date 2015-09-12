using System;

namespace SGson
{
	public class JsonNumber : JsonElement
	{
		private string stringValue;
		private sbyte? sbyteValue;
		private byte? byteValue;
		private char? charValue;
		private int? intValue;
		private uint? uintValue;
		private long? longValue;
		private ulong? ulongValue;
		private float? floatValue;
		private double? doubleValue;
		private decimal? decimalValue;

		public sbyte SbyteValue
		{
			get
			{
				if (sbyteValue.HasValue)
				{
					return sbyteValue.Value;
				}
				else
				{
					try
					{
						sbyteValue = sbyte.Parse(stringValue);
					}
					catch (FormatException)
					{
						sbyteValue = (sbyte)double.Parse(stringValue);
					}
					return sbyteValue.Value;
				}
			}
		}

		public byte ByteValue
		{
			get
			{
				if (byteValue.HasValue)
				{
					return byteValue.Value;
				}
				else
				{
					try
					{
						byteValue = byte.Parse(stringValue);
					}
					catch (FormatException)
					{
						byteValue = (byte)double.Parse(stringValue);
					}
					return byteValue.Value;
				}
			}
		}

		public char CharValue
		{
			get
			{
				if (charValue.HasValue)
				{
					return charValue.Value;
				}
				else
				{
					try
					{
						charValue = char.Parse(stringValue);
					}
					catch (FormatException)
					{
						charValue = (char)double.Parse(stringValue);
					}
					return charValue.Value;
				}
			}
		}

		public int IntValue
		{
			get
			{
				if (intValue.HasValue)
				{
					return intValue.Value;
				}
				else
				{
					try
					{
						intValue = int.Parse(stringValue);
					}
					catch (FormatException)
					{
						intValue = (int)double.Parse(stringValue);
					}
					return intValue.Value;
				}
			}
		}

		public uint UIntValue
		{
			get
			{
				if (uintValue.HasValue)
				{
					return uintValue.Value;
				}
				else
				{
					try
					{
						uintValue = uint.Parse(stringValue);
					}
					catch (FormatException)
					{
						uintValue = (uint)double.Parse(stringValue);
					}
					return uintValue.Value;
				}
			}
		}

		public long LongValue
		{
			get
			{
				if (longValue.HasValue)
				{
					return longValue.Value;
				}
				else
				{
					try
					{
						longValue = long.Parse(stringValue);
					}
					catch (FormatException)
					{
						longValue = (long)double.Parse(stringValue);
					}
					return longValue.Value;
				}
			}
		}

		public ulong ULongValue
		{
			get
			{
				if (ulongValue.HasValue)
				{
					return ulongValue.Value;
				}
				else
				{
					try
					{
						ulongValue = ulong.Parse(stringValue);
					}
					catch (FormatException)
					{
						ulongValue = (ulong)double.Parse(stringValue);
					}
					return ulongValue.Value;
				}
			}
		}

		public float FloatValue
		{
			get
			{
				if (floatValue.HasValue)
				{
					return floatValue.Value;
				}
				else
				{
					floatValue = float.Parse(stringValue);
					return floatValue.Value;
				}
			}
		}

		public double DoubleValue
		{
			get
			{
				if (doubleValue.HasValue)
				{
					return doubleValue.Value;
				}
				else
				{
					doubleValue = double.Parse(stringValue);
					return doubleValue.Value;
				}
			}
		}

		public decimal DecimalValue
		{
			get
			{
				if (decimalValue.HasValue)
				{
					return decimalValue.Value;
				}
				else
				{
					decimalValue = decimal.Parse(stringValue);
					return decimalValue.Value;
				}
			}
		}

		public JsonNumber(sbyte value)
		{
			this.sbyteValue = value;
			initStringValue(value);
		}

		public JsonNumber(byte value)
		{
			this.byteValue = value;
			initStringValue(value);
		}

		public JsonNumber(char value)
		{
			this.charValue = value;
			initStringValue(value);
		}

		public JsonNumber(int value)
		{
			this.intValue = value;
			initStringValue(value);
		}

		public JsonNumber(uint value)
		{
			this.uintValue = value;
			initStringValue(value);
		}

		public JsonNumber(long value)
		{
			this.longValue = value;
			initStringValue(value);
		}

		public JsonNumber(ulong value)
		{
			this.ulongValue = value;
			initStringValue(value);
		}

		public JsonNumber(float value)
		{
			this.floatValue = value;
			initStringValue(value);
		}

		public JsonNumber(double value)
		{
			this.doubleValue = value;
			initStringValue(value);
		}

		public JsonNumber(decimal value)
		{
			this.decimalValue = value;
			initStringValue(value);
		}

		public JsonNumber(string value)
		{
			initStringValue(value);
		}

		private void initStringValue(object o)
		{
			if (o == null)
			{
				this.stringValue = "0";
			}
			else
			{
				this.stringValue = o.ToString();
			}
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

		public static implicit operator JsonNumber(float value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(double value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(decimal value)
		{
			return new JsonNumber(value);
		}

		public static implicit operator JsonNumber(string value)
		{
			if (value == null)
			{
				throw new Exception("Can't convert null to JsonNumber");
			}
			try
			{
				Double.Parse(value);
				return new JsonNumber(value);
			}
			catch (Exception e)
			{
				throw new Exception(String.Format("Can't convert string \"{}\" to JsonNumber", value), e);
			}
		}

		public static explicit operator sbyte(JsonNumber element)
		{
			return element.SbyteValue;
		}

		public static explicit operator byte(JsonNumber element)
		{
			return element.ByteValue;
		}

		public static explicit operator char(JsonNumber element)
		{
			return element.CharValue;
		}

		public static explicit operator int(JsonNumber element)
		{
			return element.IntValue;
		}

		public static explicit operator uint(JsonNumber element)
		{
			return element.UIntValue;
		}

		public static explicit operator long(JsonNumber element)
		{
			return element.LongValue;
		}

		public static explicit operator ulong(JsonNumber element)
		{
			return element.ULongValue;
		}

		public static explicit operator float(JsonNumber element)
		{
			return element.FloatValue;
		}

		public static explicit operator double(JsonNumber element)
		{
			return element.DoubleValue;
		}

		public static explicit operator decimal(JsonNumber element)
		{
			return element.DecimalValue;
		}

		public static implicit operator string(JsonNumber element)
		{
			return element.stringValue;
		}

		public override string ToString()
		{
			return this.stringValue;
		}
	}
}