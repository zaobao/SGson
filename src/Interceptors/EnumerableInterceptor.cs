using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class EnumerableInterceptor : AInterceptor
	{
		private static readonly Type mGenericTypeDefinition = typeof(IEnumerable<>);
		private static readonly Type mListType = typeof(List<>);

		public override bool IsSerializable(object obj)
		{
			return (obj is IEnumerable) && !(obj is string);
		}

		public override bool IsDeserializable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == mGenericTypeDefinition;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			JsonArray ja = new JsonArray(GetJsonElementWithYield(o));
			return ja;
		}

		private IEnumerable<JsonElement> GetJsonElementWithYield(object o)
		{
			foreach (object item in (IEnumerable)o)
			{
				yield return Context.ToJsonTree(item);
			}
		}

		public override object InterceptWhenDeserialize(JsonElement je, Type type)
		{
			if (je == null || je is JsonNull)
			{
				return null;
			}
			if (!(je is JsonArray))
			{
				throw new Exception("Expect an array, but " + je);
			}
			JsonArray ja = (JsonArray)je;
			Type[] genericArguments = type.GetGenericArguments();
			Type tImpl = mListType.MakeGenericType(genericArguments);
			object o = PocsoUtils.GetInstance(tImpl);
			MethodInfo method = tImpl.GetMethod("Add");
			foreach (JsonElement element in ja)
			{
				method.Invoke(o, new Object[]
				{
					Context.FromJsonTree(element, genericArguments[0])
				});
			}
			return o;
		}

	}
}