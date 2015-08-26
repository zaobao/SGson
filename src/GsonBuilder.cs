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
		private int visitedObjectStackLength;
		private int visitedObjectCountLimit;

		public GsonBuilder()
			: this(Gson.DefaultSerializerDictionary,
				Gson.DefaultDeserializerDictionary,
				Gson.DefaultTypeAdapterDictionary,
				Gson.DefaultBreakInterceptorList,
				Gson.DefaultVisitedObjectStackLength,
				Gson.DefaultVisitedObjectCountLimit) {}

		public GsonBuilder(Dictionary<Type, Func<object, JsonElement>> serializerDictionary,
			Dictionary<Type, Func<JsonElement, object>> deserializerDictionary,
			Dictionary<Type, ATypeAdapter> typeAdapterDictionary,
			List<ABreakInterceptor> breakInterceptorList,
			int visitedObjectStackLength,
			int visitedObjectCountLimit)
		{
			this.visitedObjectStackLength = visitedObjectStackLength;
			this.visitedObjectCountLimit = visitedObjectCountLimit;
			foreach (KeyValuePair<Type, Func<object, JsonElement>> keyValuePair in serializerDictionary)
			{
				this.serializerDictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<Type, Func<JsonElement, object>> keyValuePair in deserializerDictionary)
			{
				this.deserializerDictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<Type, ATypeAdapter> keyValuePair in typeAdapterDictionary)
			{
				ATypeAdapter adapter = keyValuePair.Value.Clone();
				this.typeAdapterDictionary.Add(keyValuePair.Key, adapter);
			}
			for (int i =  0; i < breakInterceptorList.Count; i++)
			{
				ABreakInterceptor interceptor = breakInterceptorList[i].Clone();
				this.breakInterceptorList.Add(interceptor);
			}
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
			return new Gson(serializerDictionary,
				deserializerDictionary,
				typeAdapterDictionary,
				breakInterceptorList,
				visitedObjectStackLength,
				visitedObjectCountLimit);
		}
	}
}