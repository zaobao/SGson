using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class CollectionInterceptor : ABreakInterceptor
	{
		private static readonly Type mGenericTypeDefinition = typeof(ICollection<>);
		private static readonly Type mListType = typeof(List<>);

		// Not use to serialize ICollection<T>
		public override bool IsSerializable(object obj)
		{
			return false;
		}

		public override bool IsDeserializable(Type type)
		{
			return type.GetInterface("System.Collections.Generic.ICollection`1") != null ||
				type.IsGenericType && type.GetGenericTypeDefinition() == mGenericTypeDefinition;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			return JsonNull.Instance;
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
			Type interfaceType = type.GetInterface("System.Collections.Generic.ICollection`1");
			Type implType = null;
			Type[] genericArguments = null;
			if (interfaceType == null)
			{
				genericArguments = type.GetGenericArguments();
				implType = mListType.MakeGenericType(genericArguments);
			}
			else
			{
				genericArguments = interfaceType.GetGenericArguments();
				implType = type;
			}
			object o = PocsoUtils.GetInstance(implType);
			MethodInfo method = implType.GetMethod("Add");
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