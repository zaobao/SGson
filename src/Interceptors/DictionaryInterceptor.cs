using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class DictionaryInterceptor : AInterceptor
	{
		private static readonly Type mGenericTypeDefinition = typeof(IDictionary<,>);
		private static readonly Type mDicDefinition = typeof(Dictionary<,>);

		public override bool IsSerializable(object obj)
		{
			return (obj is IDictionary);
		}

		public override bool IsDeserializable(Type type)
		{
			return type.GetInterface("System.Collections.Generic.IDictionary`2") != null ||
				type.IsGenericType && type.GetGenericTypeDefinition() == mGenericTypeDefinition;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			JsonMap jm = new JsonMap();
			IDictionary dic = (IDictionary)o;
			foreach (object key in dic.Keys)
			{
				JsonString keyStr = null;
				keyStr = Context.ToJsonTree(key) as JsonString;
				if (keyStr == null)
				{
					keyStr = key.ToString();
				}
				jm.Add(keyStr, Context.ToJsonTree(dic[key]));
			}
			return jm;
		}

		public override object InterceptWhenDeserialize(JsonElement je, Type type)
		{
			if (je == null || je is JsonNull)
			{
				return null;
			}
			if (!(je is JsonMap))
			{
				throw new Exception("Expect a map, but " + je);
			}
			JsonMap jm = (JsonMap)je;
			Type interfaceType = type.GetInterface("System.Collections.Generic.IDictionary`2");
			Type implType = null;
			Type[] genericArguments = null;
			if (interfaceType == null)
			{
				genericArguments = type.GetGenericArguments();
				implType = mDicDefinition.MakeGenericType(genericArguments);
			}
			else
			{
				genericArguments = interfaceType.GetGenericArguments();
				implType = type;
			}
			object o = PocsoUtils.GetInstance(implType);
			MethodInfo method = implType.GetMethod("Add");
			foreach (KeyValuePair<string,JsonElement> kv in jm)
			{
				object key = null;
				try
				{
					key = Context.FromJsonTree(kv.Key, genericArguments[0]);
				}
				catch (Exception)
				{
					key = Convert.ChangeType(kv.Key, genericArguments[0]);
				}
				method.Invoke(o, new Object[]
				{
					key,
					Context.FromJsonTree(kv.Value, genericArguments[1])
				});
			}
			return o;
		}

	}
}