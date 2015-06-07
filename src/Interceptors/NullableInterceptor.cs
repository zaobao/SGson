using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class NullableInterceptor : ABreakInterceptor
	{
		private static readonly Type mStackType = typeof(Nullable<>);

		public override bool IsSerializable(object obj)
		{
			Type type = obj.GetType();
			return type.IsGenericType && type.GetGenericTypeDefinition() == mStackType;
		}

		public override bool IsDeserializable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == mStackType;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			Type type = o.GetType();
			PropertyInfo pi = type.GetProperty("GetV");
			return Context.ToJsonTree(pi.GetValue(o, null));
		}

		public override object InterceptWhenDeserialize(JsonElement je, Type type)
		{
			if (je == null || je is JsonNull)
			{
				return null;
			}
			Type[] genericArguments = type.GetGenericArguments();
			return Context.FromJsonTree(je, genericArguments[0]);
		}

	}
}