using System;
using System.Collections;
using System.Collections.Generic;

namespace SGson
{
	public class JsonMap : JsonElement, IEnumerable<KeyValuePair<string,JsonElement>>
	{
		private Dictionary<string,JsonElement> map;

		public JsonElement this[string key]
		{
			get
			{
				if (map[key] == null)
				{
					return JsonNull.Instance;
				}
				else
				{
					return map[key];
				}
			}
			set { map[key] = value; }
		}

		public JsonMap()
		{
			map = new Dictionary<string,JsonElement>();
		}

		public JsonMap(Dictionary<string,JsonElement> dic)
		{
			if (dic == null)
			{
				throw new Exception("dic == null");
			}
			map = dic;
		}

		public IEnumerator GetEnumerator() {
			return map.GetEnumerator();
		}

		IEnumerator<KeyValuePair<string,JsonElement>> IEnumerable<KeyValuePair<string,JsonElement>>.GetEnumerator()
		{
			return map.GetEnumerator();
		}

		public void Add(JsonString str, JsonElement element)
		{
			if (element == JsonNull.Instance)
			{
				element = null;
			}
			map.Add((string)str, element);
		}

		public static implicit operator JsonMap(Dictionary<string,JsonElement> map)
		{
			if (map == null)
			{
				throw new Exception("Can't convert null to JsonMap");
			}
			return new JsonMap(map);
		}

		public static implicit operator Dictionary<string,JsonElement>(JsonMap jsonMap)
		{
			return jsonMap.map;
		}
	}
}