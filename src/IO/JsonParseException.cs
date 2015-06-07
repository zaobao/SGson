using System;

using SGson;

namespace SGson.IO
{
	public class JsonParseException : Exception
	{
		public JsonParseException(string message ,Exception inner, int? position, int? line, int? column, string nearby)
			: base(message, inner)
		{
			this.Data["position"] = position;
			this.Data["line"] = line;
			this.Data["column"] = column;
			this.Data["nearby"] = nearby;
		}

		public override string Message
		{
			get
			{
				return base.Message + String.Format(" Position: {0}, in line {1}, at column{2}. Near {3}.", this.Data["position"], this.Data["line"], this.Data["column"], new JsonString((string)this.Data["nearby"]));
			}
		}

	}
}