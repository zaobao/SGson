using System;
using System.Text.RegularExpressions;

namespace SGson
{
	public class JsonBoolean : JsonElement
	{
		protected bool value;
		protected static readonly Regex boolReg = new Regex("^(?:(true|True|TRUE)|(false|False|FALSE))$", RegexOptions.Compiled);

		public JsonBoolean(bool value)
		{
			this.value = value;
		}

		public static implicit operator JsonBoolean(bool value)
		{
			return new JsonBoolean(value);
		}

		public static implicit operator JsonBoolean(string value)
		{
			if (value == null)
			{
				throw new Exception("Can't convert null to JsonBoolean");
			}
			GroupCollection groups = JsonBoolean.boolReg.Match(value).Groups;
			if (groups[1].Success)
			{
				return true;
			}
			if (groups[2].Success)
			{
				return false;
			}
			throw new Exception(String.Format("Can't convert string \"{0}\" to JsonBoolean", value));
		}

		public static implicit operator bool(JsonBoolean element)
		{
			return element.value;
		}

		public static explicit operator string(JsonBoolean element)
		{
			return element.value.ToString();
		}

		public override string ToString()
		{
			return value ? "true" : "false";
		}
	}
}