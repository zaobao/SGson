using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class StackInterceptor : ABreakInterceptor
	{
		private static readonly Type mStackType = typeof(Stack<>);

		// Not use to serialize Stack<T>
		public override bool IsSerializable(object obj)
		{
			return false;
		}

		public override bool IsDeserializable(Type type)
		{
			return type.GetInterface("System.Collections.Generic.Stack`1") != null ||
				type.IsGenericType && type.GetGenericTypeDefinition() == mStackType;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			return null;
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
			object o = PocsoUtils.GetInstance(type);
			MethodInfo method = type.GetMethod("Push");
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