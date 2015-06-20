using System;

using SGson;

namespace SGson.Example
{
	public class EMCAScript6Number
	{
		public static void Main()
		{
			Gson gson = new Gson();

			// EMCAScript6 binary
			Console.WriteLine(gson.FromJson<int>("0b1010"));

			// EMCAScript6 Octet
			Console.WriteLine(gson.FromJson<int>("0O1010"));
		}
	}
}