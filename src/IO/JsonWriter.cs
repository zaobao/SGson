using System;
using System.IO;
using System.Collections.Generic;

namespace SGson.IO
{
	public class JsonWriter
	{

		private readonly TextWriter writer;
		private readonly string[] controls = new string[32]
		{
			"\\u0000","\\u0001","\\u0002","\\u0003","\\u0004","\\u0005","\\u0006","\\u0007",
			"\\u0008","\\u0009","\\u000A","\\u000B","\\u000C","\\u000D","\\u000E","\\u000F",
			"\\u0010","\\u0011","\\u0012","\\u0013","\\u0014","\\u0015","\\u0016","\\u0017",
			"\\u0018","\\u0019","\\u001A","\\u001B","\\u001C","\\u001D","\\u001E","\\u001F"
		};

		public JsonWriter() : this(new StringWriter()) {}

		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new Exception("writer == null");
			}
			this.writer = writer;
		}

		public void Flush()
		{
			writer.Flush();
		}

		public void Close()
		{
			writer.Close();
		}

		public void Write(JsonElement jsonElement)
		{
			if (jsonElement == null)
			{
				writer.Write("null");
			}
			else if (jsonElement.IsJsonString)
			{
				this.Write((JsonString)jsonElement);
			}
			else if (jsonElement.IsJsonNumber)
			{
				this.Write((JsonNumber)jsonElement);
			}
			else if (jsonElement.IsJsonNull)
			{
				this.Write((JsonNull)jsonElement);
			}
			else if (jsonElement.IsJsonBoolean)
			{
				this.Write((JsonBoolean)jsonElement);
			}
			else if (jsonElement.IsJsonMap)
			{
				this.Write((JsonMap)jsonElement);
			}
			else if (jsonElement.IsJsonArray)
			{
				this.Write((JsonArray)jsonElement);
			}
			else
			{
				throw new Exception("Unexpected type of JsonElement.");
			}
		}

		public void Write(JsonNull jsonNull)
		{
			writer.Write("null");
		}

		public void Write(JsonBoolean JsonBoolean)
		{
			if (JsonBoolean == null)
			{
				writer.Write("null");
			}
			else if (JsonBoolean)
			{
				writer.Write("true");
			}
			else
			{
				writer.Write("false");
			}
		}

		public void Write(JsonNumber jsonNumber)
		{
			if (jsonNumber == null)
			{
				writer.Write("null");
			}
			else
			{
				writer.Write(jsonNumber.ToString());
			}
		}

		public void Write(JsonString jsonString)
		{
			writer.Write('"');
			for (int i = 0, length = jsonString.Length; i < length; i++)
			{
				char c = jsonString[i];
				switch (c)
				{
					case '"':
					case '\\':
						writer.Write('\\');
						writer.Write(c);
						break;
					case '\t':
						writer.Write("\\t");
						break;
					case '\b':
						writer.Write("\\b");
						break;
					case '\n':
						writer.Write("\\n");
						break;
					case '\r':
						writer.Write("\\r");
						break;
					case '\f':
						writer.Write("\\f");
						break;
					case '\u2028':
						writer.Write("\\u2028");
						break;
					case '\u2029':
						writer.Write("\\u2029");
						break;
					default:
						if (c <= 0x1F)
						{
							writer.Write(controls[c]);
						}
						else
						{
							writer.Write(c);
						}
						break;
				}
			}
			writer.Write('"');
		}

		public void Write(JsonArray jsonArray)
		{
			writer.Write('[');
			int length = jsonArray.Length;
			if (length > 0)
			{
				this.Write(jsonArray[0]);
				for (int i = 1; i < length; i++)
				{
					writer.Write(',');
					this.Write(jsonArray[i]);
				}
			}
			writer.Write(']');
		}

		public void Write(JsonMap jsonMap)
		{
			writer.Write('{');
			int count = 0;
			foreach (KeyValuePair<string,JsonElement> kv in jsonMap)
			{
				if (count != 0)
				{
					writer.Write(",");
				}
				this.Write((JsonString)kv.Key);
				writer.Write(":");
				this.Write(kv.Value);
				count++;
			}
			writer.Write('}');
		}

	}
}