using System;
using System.Collections.Generic;

using SGson.Interceptors;
using SGson.TypeAdapters;

namespace SGson
{
	public class GsonBuilder
	{

		private Dictionary<Type, Func<object, JsonElement>> serializerDictionary = new Dictionary<Type, Func<object, JsonElement>>();
		private Dictionary<Type, Func<JsonElement, object>> deserializerDictionary = new Dictionary<Type, Func<JsonElement, object>>();
		private Dictionary<Type, ATypeAdapter> typeAdapterDictionary = new Dictionary<Type, ATypeAdapter>();
		private List<ABreakInterceptor> breakInterceptorList = new List<ABreakInterceptor>();
		private int visitedObjectStackLength = 8;
		private int visitedObjectCountLimit = 100000;

		public GsonBuilder()
		{
			RegisterSerializer<bool>(delegate(object x)
			{
				return (JsonElement)(bool)x;
			});
			RegisterDeserializer<bool>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return false;
				}
				if (x.IsJsonBoolean)
				{
					return (bool)(JsonBoolean)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to bool, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<sbyte>(delegate(object x)
			{
				return (JsonElement)(sbyte)x;
			});
			RegisterDeserializer<sbyte>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (sbyte)0;
				}
				if (x is JsonNumber)
				{
					return (sbyte)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to sbyte, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<byte>(delegate(object x)
			{
				return (JsonElement)(byte)x;
			});
			RegisterDeserializer<byte>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (byte)0;
				}
				if (x is JsonNumber)
				{
					return (byte)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to byte, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<int>(delegate(object x)
			{
				return (JsonElement)(int)x;
			});
			RegisterDeserializer<int>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (int)0;
				}
				if (x is JsonNumber)
				{
					return (int)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to int, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<uint>(delegate(object x)
			{
				return (JsonElement)(uint)x;
			});
			RegisterDeserializer<uint>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (uint)0;
				}
				if (x is JsonNumber)
				{
					return (uint)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to uint, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<long>(delegate(object x)
			{
				return (JsonElement)(long)x;
			});
			RegisterDeserializer<long>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (long)0;
				}
				if (x is JsonNumber)
				{
					return (long)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to long, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<ulong>(delegate(object x)
			{
				return (JsonElement)(ulong)x;
			});
			RegisterDeserializer<ulong>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (ulong)0;
				}
				if (x is JsonNumber)
				{
					return (ulong)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to ulong, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<float>(delegate(object x)
			{
				return (JsonElement)(float)x;
			});
			RegisterDeserializer<float>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (float)0;
				}
				if (x is JsonNumber)
				{
					return (float)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to float, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<double>(delegate(object x)
			{
				return (JsonElement)(double)x;
			});
			RegisterDeserializer<double>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (double)0;
				}
				if (x is JsonNumber)
				{
					return (double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to double, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<decimal>(delegate(object x)
			{
				return (JsonElement)(decimal)x;
			});
			RegisterDeserializer<decimal>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return (decimal)0;
				}
				if (x is JsonNumber)
				{
					return (decimal)(double)(JsonNumber)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to decimal, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterSerializer<string>(delegate(object x)
			{
				return (JsonElement)(string)x;
			});
			RegisterDeserializer<string>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return null;
				}
				if (x.IsJsonString)
				{
					return (string)(JsonString)x;
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to string, JsonElement: {1}.", x.GetType(), x.ToString()));

			});
			RegisterSerializer<DateTime>(delegate(object x)
			{
				return (JsonElement)(((DateTime)x).ToString("yyyy-MM-dd HH:mm:ss"));
			});
			RegisterDeserializer<DateTime>(delegate(JsonElement x)
			{
				if (x == null || x is JsonNull)
				{
					return DateTime.MinValue;
				}
				if (x.IsJsonString)
				{
					return DateTime.Parse((string)(JsonString)x);
				}
				throw new Exception(String.Format("Type not match, can not convert {0} to DateTime, JsonElement: {1}.", x.GetType(), x.ToString()));
			});
			RegisterAdapter(new ObjectAdapter(), typeof(object));
			RegisterBreakInterceptor(new EnumerableInterceptor());
			RegisterBreakInterceptor(new CollectionInterceptor());
			RegisterBreakInterceptor(new DictionaryInterceptor());
			RegisterBreakInterceptor(new StackInterceptor());
			RegisterBreakInterceptor(new QueueInterceptor());
			RegisterBreakInterceptor(new ArrayInterceptor());
			RegisterBreakInterceptor(new NullableInterceptor());
		}

		public GsonBuilder RegisterSerializer<T>(Func<object, JsonElement> serializer)
		{
			Type t = typeof(T);
			if (serializerDictionary.ContainsKey(t))
			{
				serializerDictionary[t] = serializer;
			}
			else
			{
				serializerDictionary.Add(t, serializer);
			}
			return this;
		}

		public GsonBuilder RegisterDeserializer<T>(Func<JsonElement, object> deserializer)
		{
			Type t = typeof(T);
			if (serializerDictionary.ContainsKey(t))
			{
				deserializerDictionary[t] = deserializer;
			}
			else
			{
				deserializerDictionary.Add(t, deserializer);
			}
			return this;
		}

		public GsonBuilder RegisterAdapter(ATypeAdapter adapter, Type t)
		{
			if (typeAdapterDictionary.ContainsKey(t))
			{
				typeAdapterDictionary[t] = adapter;
			}
			else
			{
				typeAdapterDictionary.Add(t, adapter);
			}
			return this;
		}

		public GsonBuilder RegisterBreakInterceptor(ABreakInterceptor interceptor)
		{
			if (interceptor != null)
			{
				breakInterceptorList.Add(interceptor);
			}
			return this;
		}

		public GsonBuilder SetVisitedObjectStackLength(int length)
		{
			visitedObjectStackLength = length;
			return this;
		}

		public GsonBuilder SetVisitedObjectCountLimit(int countLimit)
		{
			visitedObjectCountLimit = countLimit;
			return this;
		}

		public Gson Create()
		{
			Gson gson = new Gson();
			gson.VisitedObjectStackLength = visitedObjectStackLength;
			gson.VisitedObjectCountLimit = visitedObjectCountLimit;
			foreach (KeyValuePair<Type, Func<object, JsonElement>> keyValuePair in serializerDictionary)
			{
				gson.SerializerDictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<Type, Func<JsonElement, object>> keyValuePair in deserializerDictionary)
			{
				gson.DeserializerDictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<Type, ATypeAdapter> keyValuePair in typeAdapterDictionary)
			{
				ATypeAdapter adapter = keyValuePair.Value.Clone();
				adapter.Context = gson;
				gson.TypeAdapterDictionary.Add(keyValuePair.Key, adapter);
			}
			for (int i =  0; i < breakInterceptorList.Count; i++)
			{
				ABreakInterceptor interceptor = breakInterceptorList[i].Clone();
				interceptor.Context = gson;
				gson.BreakInterceptorList.Add(interceptor);
			}
			return gson;
		}
	}
}