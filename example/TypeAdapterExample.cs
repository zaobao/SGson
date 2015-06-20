using System;
using System.Net;

using SGson;
using SGson.TypeAdapters;

namespace SGson.Example
{
	public class TypeAdapterExample
	{
		class IPAddressAdaper : ATypeAdapter
		{
			public override JsonElement Serialize(object o)
			{
				if (o == null)
				{
					return JsonNull.Instance;
				}
				return new JsonString(((IPAddress)o).ToString());
			}

			public override object Deserialize(JsonElement je, Type originalType)
			{
				if (je == JsonNull.Instance)
				{
					return null;
				}
				if (je.IsJsonString)
				{
					try
					{
						return IPAddress.Parse((string)(JsonString)je);
					}
					catch (Exception)
					{
						;
					}
				}
				throw new Exception(String.Format("Can not parse {0} to an IPAdress.", je.ToString()));
			}
		}

		public static void Main()
		{
			Gson gson = new GsonBuilder()
				.RegisterAdapter(new IPAddressAdaper(), typeof(IPAddress))
				.Create();

			IPAddress ipa = gson.FromJson<IPAddress>("\"192.168.0.1\"");
			Console.WriteLine(ipa);
			Console.WriteLine(gson.ToJson(ipa));
		}
	}
}