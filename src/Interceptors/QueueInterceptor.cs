using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class QueueInterceptor : ABreakInterceptor
	{
		private static readonly Type mQueueType = typeof(Queue<>);

		// Not use to serialize Queue<T>
		public override bool IsSerializable(object obj)
		{
			return false;
		}

		public override bool IsDeserializable(Type type)
		{
			return type.GetInterface("System.Collections.Generic.Queue`1") != null ||
				type.IsGenericType && type.GetGenericTypeDefinition() == mQueueType;
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
			MethodInfo method = type.GetMethod("Enqueue");
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