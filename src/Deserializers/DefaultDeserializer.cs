using System;

namespace SGson.Deserializers
{
	public class DefaultDeserializer
	{
		public static object DeserializeBoolean(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return false;
			}
			if (x.IsJsonBoolean)
			{
				return (bool)(JsonBoolean)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to bool, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeSbyte(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (sbyte)0;
			}
			if (x is JsonNumber)
			{
				return (sbyte)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to sbyte, JsonElement: {1}.", x.GetType(), x.ToString()));

		}
		public static object DeserializeByte(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (byte)0;
			}
			if (x is JsonNumber)
			{
				return (byte)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to byte, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeInt32(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (int)0;
			}
			if (x is JsonNumber)
			{
				return (int)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to int, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeUInt32(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (uint)0;
			}
			if (x is JsonNumber)
			{
				return (uint)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to uint, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeInt64(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (long)0;
			}
			if (x is JsonNumber)
			{
				return (long)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to long, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeUInt64(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (ulong)0;
			}
			if (x is JsonNumber)
			{
				return (ulong)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to ulong, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeFloat(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (float)0;
			}
			if (x is JsonNumber)
			{
				return (float)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to float, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeDouble(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (double)0;
			}
			if (x is JsonNumber)
			{
				return (double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to double, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeDecimal(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return (decimal)0;
			}
			if (x is JsonNumber)
			{
				return (decimal)(double)(JsonNumber)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to decimal, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeString(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return null;
			}
			if (x.IsJsonString)
			{
				return (string)(JsonString)x;
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to string, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
		public static object DeserializeDateTime(JsonElement x)
		{
			if (x == null || x is JsonNull)
			{
				return DateTime.MinValue;
			}
			if (x.IsJsonString)
			{
				return DateTime.Parse((string)(JsonString)x);
			}
			throw new Exception(String.Format("Type not match, can not convert {0} to DateTime, JsonElement: {1}.", x.GetType(), x.ToString()));
		}
	}
}