namespace SGson
{
	public class JsonNull : JsonElement
	{
		public static readonly JsonNull Instance = new JsonNull();

		public override string ToString()
		{
			return "null";
		}
	}
}