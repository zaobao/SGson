using System;

namespace SGson.Serializers
{
	public class DefaultSerializer
	{
		public static JsonElement SerializeBoolean(object x)
		{
			return (JsonElement)(bool)x;
		}
		public static JsonElement SerializeSbyte(object x)
		{
			return (JsonElement)(sbyte)x;
		}
		public static JsonElement SerializeByte(object x)
		{
			return (JsonElement)(byte)x;
		}
		public static JsonElement SerializeInt16(object x)
		{
			return (JsonElement)(short)x;
		}
		public static JsonElement SerializeUInt16(object x)
		{
			return (JsonElement)(ushort)x;
		}
		public static JsonElement SerializeInt32(object x)
		{
			return (JsonElement)(int)x;
		}
		public static JsonElement SerializeUInt32(object x)
		{
			return (JsonElement)(uint)x;
		}
		public static JsonElement SerializeInt64(object x)
		{
			return (JsonElement)(long)x;
		}
		public static JsonElement SerializeUInt64(object x)
		{
			return (JsonElement)(ulong)x;
		}
		public static JsonElement SerializeFloat(object x)
		{
			return (JsonElement)(float)x;
		}
		public static JsonElement SerializeDouble(object x)
		{
			return (JsonElement)(double)x;
		}
		public static JsonElement SerializeDecimal(object x)
		{
			return (JsonElement)(decimal)x;
		}
		public static JsonElement SerializeString(object x)
		{
			return (JsonElement)(string)x;
		}
		public static JsonElement SerializeDateTime(object x)
		{
			return (JsonElement)(((DateTime)x).ToString("yyyy-MM-dd HH:mm:ss"));
		}
	}
}