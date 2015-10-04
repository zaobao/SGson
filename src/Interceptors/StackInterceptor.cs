using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class StackInterceptor : AInterceptor
	{
		private static readonly Type mStackType = typeof(Stack<>);

		private static Type GetStackType(Type type)
		{
			while(type != null && 
				!(type.IsGenericType && type.GetGenericTypeDefinition() == mStackType))
			{
				type = type.BaseType;
			}
			return type;
		}

		public override bool IsSerializable(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			return GetStackType(obj.GetType()) != null;
		}

		public override bool IsDeserializable(Type type)
		{
			return GetStackType(type) != null;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			JsonArray ja = new JsonArray();
			foreach (object item in (IEnumerable)o)
			{
				ja.Add(Context.ToJsonTree(item));
			}
			ja.Reverse();
			return ja;
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
			Type[] genericArguments = GetStackType(type).GetGenericArguments();
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