using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SGson.Deserializers;
using SGson.IO;
using SGson.Interceptors;
using SGson.Serializers;
using SGson.TypeAdapters;

namespace SGson
{
	public class Gson
	{
		internal static Dictionary<Type, Func<object, JsonElement>> DefaultSerializerDictionary = new Dictionary<Type, Func<object, JsonElement>>()
		{
			{typeof(bool), DefaultSerializer.SerializeBoolean},
			{typeof(sbyte), DefaultSerializer.SerializeSbyte},
			{typeof(byte), DefaultSerializer.SerializeByte},
			{typeof(int), DefaultSerializer.SerializeInt32},
			{typeof(uint), DefaultSerializer.SerializeUInt32},
			{typeof(long), DefaultSerializer.SerializeInt64},
			{typeof(ulong), DefaultSerializer.SerializeUInt64},
			{typeof(float), DefaultSerializer.SerializeFloat},
			{typeof(double), DefaultSerializer.SerializeDouble},
			{typeof(decimal), DefaultSerializer.SerializeDecimal},
			{typeof(string), DefaultSerializer.SerializeString},
			{typeof(DateTime), DefaultSerializer.SerializeDateTime}
		};
		internal static Dictionary<Type, Func<JsonElement, object>> DefaultDeserializerDictionary = new Dictionary<Type, Func<JsonElement, object>>()
		{
			{typeof(bool), DefaultDeserializer.DeserializeBoolean},
			{typeof(sbyte), DefaultDeserializer.DeserializeSbyte},
			{typeof(byte), DefaultDeserializer.DeserializeByte},
			{typeof(int), DefaultDeserializer.DeserializeInt32},
			{typeof(uint), DefaultDeserializer.DeserializeUInt32},
			{typeof(long), DefaultDeserializer.DeserializeInt64},
			{typeof(ulong), DefaultDeserializer.DeserializeUInt64},
			{typeof(float), DefaultDeserializer.DeserializeFloat},
			{typeof(double), DefaultDeserializer.DeserializeDouble},
			{typeof(decimal), DefaultDeserializer.DeserializeDecimal},
			{typeof(string), DefaultDeserializer.DeserializeString},
			{typeof(DateTime), DefaultDeserializer.DeserializeDateTime}
		};
		internal static Dictionary<Type, ATypeAdapter> DefaultTypeAdapterDictionary = new Dictionary<Type, ATypeAdapter>()
		{
			{typeof(Enum), new EnumAdapter()},
			{typeof(object), new ObjectAdapter()}
		};
		internal static List<AInterceptor> DefaultInterceptorList = new List<AInterceptor>()
		{
			new EnumerableInterceptor(),
			new CollectionInterceptor(),
			new DictionaryInterceptor(),
			new StackInterceptor(),
			new QueueInterceptor(),
			new ArrayInterceptor(),
			new NullableInterceptor()
		};
		internal static int DefaultVisitedObjectStackLength = 8;
		internal static int DefaultVisitedObjectCountLimit = 100000;

		internal Dictionary<Type, Func<object, JsonElement>> SerializerDictionary = new Dictionary<Type, Func<object, JsonElement>>();
		internal Dictionary<Type, Func<JsonElement, object>> DeserializerDictionary = new Dictionary<Type, Func<JsonElement, object>>();
		internal Dictionary<Type, ATypeAdapter> TypeAdapterDictionary = new Dictionary<Type, ATypeAdapter>();
		internal List<AInterceptor> InterceptorList = new List<AInterceptor>();
		internal int VisitedObjectStackLength;
		internal long VisitedObjectCountLimit;

		private Stack<object> visitedObjectStack = new Stack<object>();
		private int allVisitedOjectCount = 0;

		public Gson()
			: this(DefaultSerializerDictionary,
				DefaultDeserializerDictionary,
				DefaultTypeAdapterDictionary,
				DefaultInterceptorList,
				DefaultVisitedObjectStackLength,
				DefaultVisitedObjectCountLimit) {}

		public Gson(Dictionary<Type, Func<object, JsonElement>> serializerDictionary,
			Dictionary<Type, Func<JsonElement, object>> deserializerDictionary,
			Dictionary<Type, ATypeAdapter> typeAdapterDictionary,
			List<AInterceptor> InterceptorList,
			int visitedObjectStackLength,
			long visitedObjectCountLimit)
		{
			this.VisitedObjectStackLength = visitedObjectStackLength;
			this.VisitedObjectCountLimit = visitedObjectCountLimit;
			foreach (KeyValuePair<Type, Func<object, JsonElement>> keyValuePair in serializerDictionary)
			{
				this.SerializerDictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<Type, Func<JsonElement, object>> keyValuePair in deserializerDictionary)
			{
				this.DeserializerDictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<Type, ATypeAdapter> keyValuePair in typeAdapterDictionary)
			{
				ATypeAdapter adapter = keyValuePair.Value.Clone();
				adapter.Context = this;
				this.TypeAdapterDictionary.Add(keyValuePair.Key, adapter);
			}
			for (int i =  0; i < InterceptorList.Count; i++)
			{
				AInterceptor interceptor = InterceptorList[i].Clone();
				interceptor.Context = this;
				this.InterceptorList.Add(interceptor);
			}
		}

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
					for (int i = InterceptorList.Count - 1; i >= 0; i--)
					{
						if (InterceptorList[i].IsSerializable(obj))
						{
							return InterceptorList[i].InterceptWhenSerialize(obj);
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
			for (int i = InterceptorList.Count - 1; i >= 0; i--)
			{
				if (InterceptorList[i].IsDeserializable(type))
				{
					return InterceptorList[i].InterceptWhenDeserialize(je, type);
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