using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SGson
{
	public class JsonArray : JsonElement, IEnumerable<JsonElement>
	{
		private List<JsonElement> elements;

		public int Length
		{
			get { return elements.Count; }
		}

		public JsonElement this[int index]
		{
			get
			{
				if (elements[index] == null)
				{
					return JsonNull.Instance;
				}
				else
				{
					return elements[index];
				}
			}
			set { elements[index] = value; }
		}

		public JsonArray()
		{
			elements = new List<JsonElement>();
		}

		public JsonArray(IEnumerable<JsonElement> jsonElements)
		{
			elements = jsonElements.ToList();
		}

		public IEnumerator GetEnumerator() {
			return elements.GetEnumerator();
		}

		IEnumerator<JsonElement> IEnumerable<JsonElement>.GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		public void Add(JsonElement element)
		{
			if (element == JsonNull.Instance)
			{
				element = null;
			}
			elements.Add(element);
		}

		public void AddRange(JsonArray array)
		{
			elements.AddRange(array.elements);
		}

		public static implicit operator JsonArray(List<JsonElement> jsonElements)
		{
			return new JsonArray(jsonElements);
		}

		public static implicit operator List<JsonElement>(JsonArray jsonArray)
		{
			return jsonArray.elements.ToList();
		}

	}
}