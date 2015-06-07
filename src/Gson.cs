using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SGson.IO;
using SGson.Interceptors;
using SGson.TypeAdapters;

namespace SGson
{
	public class Gson
	{
		internal Dictionary<Type, Func<object, JsonElement>> SerializerDictionary = new Dictionary<Type, Func<object, JsonElement>>();
		internal Dictionary<Type, Func<JsonElement, object>> DeserializerDictionary = new Dictionary<Type, Func<JsonElement, object>>();
		internal Dictionary<Type, ATypeAdapter> TypeAdapterDictionary = new Dictionary<Type, ATypeAdapter>();
		internal List<ABreakInterceptor> BreakInterceptorList = new List<ABreakInterceptor>();
		internal int VisitedObjectStackLength = 8;
		internal int VisitedObjectCountLimit = 100000;

		private Stack<object> visitedObjectStack = new Stack<object>();
		private int allVisitedOjectCount = 0;

		public string ToJson(object obj)
		{
			allVisitedOjectCount = 0;
			JsonElement je = ToJsonTree(obj);
			StringWriter stringWriter = new StringWriter();
			JsonWriter jsonWriter = new JsonWriter(stringWriter);
			jsonWriter.Write(je);
			return stringWriter.ToString();
		}

		public T FromJson<T>(TextReader reader)
		{
			JsonReader jsonReader = new JsonReader(reader);
			JsonElement je = jsonReader.ToJsonElement();
			return (T)FromJsonTree(je, typeof(T));
		}

		public T FromJson<T>(string json)
		{
			if (json == null)
			{
				json = "";
			}
			StringReader stringReader = new StringReader(json);
			return FromJson<T>(stringReader);
		}

		protected internal JsonElement ToJsonTree(object obj)
		{
			allVisitedOjectCount++;
			if (allVisitedOjectCount > VisitedObjectCountLimit)
			{
				throw new Exception(String.Format("Visited too much objects. AllVisitedOjectCount limit({0}) exceeded.", VisitedObjectCountLimit));
			}
			if (obj != null && visitedObjectStack.Count <= VisitedObjectStackLength && !visitedObjectStack.Any(x => Object.ReferenceEquals(obj, x)))
			{
				visitedObjectStack.Push(obj);
				try
				{
					if (obj is JsonElement)
					{
						return (JsonElement)obj;
					}
					for (int i = BreakInterceptorList.Count - 1; i >= 0; i--)
					{
						if (BreakInterceptorList[i].IsSerializable(obj))
						{
							return BreakInterceptorList[i].InterceptWhenSerialize(obj);
						}
					}
					Type type = obj.GetType();
					Func<object, JsonElement> func;
					ATypeAdapter adapter;
					while (type != null)
					{
						if (TypeAdapterDictionary.TryGetValue(type, out adapter))
						{
							return adapter.Serialize(obj);
						}
						if (SerializerDictionary.TryGetValue(type, out func))
						{
							return func(obj);
						}
						type = type.BaseType;
					}
				}
				finally
				{
					visitedObjectStack.Pop();
				}
			}
			return JsonNull.Instance;
		}

		protected internal object FromJsonTree(JsonElement je, Type type)
		{
			for (int i = BreakInterceptorList.Count - 1; i >= 0; i--)
			{
				if (BreakInterceptorList[i].IsDeserializable(type))
				{
					return BreakInterceptorList[i].InterceptWhenDeserialize(je, type);
				}
			}
			Func<JsonElement, object> func;
			ATypeAdapter adapter;
			Type OriginalType = type;
			while (type != null)
			{
				if (TypeAdapterDictionary.TryGetValue(type, out adapter))
				{
					return adapter.Deserialize(je, OriginalType);
				}
				if (DeserializerDictionary.TryGetValue(type, out func))
				{
					return func(je);
				}
				type = type.BaseType;
			}
			return null;
		}
	}
}