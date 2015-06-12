using SGson;
using System.Diagnostics;

namespace SGson.Example
{
	public class EMCAScriptNumber
	{
		public static void Main()
		{
			Gson gson = new Gson();

			// EMCAScript6 binary
			Debug.WriteLine(gson.FromJson<int>("0b1010"));

			// EMCAScript6 Octet
			Debug.WriteLine(gson.FromJson<int>("0O1010"));
		}
	}
}