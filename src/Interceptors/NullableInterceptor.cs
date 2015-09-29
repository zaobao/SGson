using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class NullableInterceptor : AInterceptor
	{
		private static readonly Type mNullableType = typeof(Nullable<>);

		public override bool IsSerializable(object obj)
		{
			if (obj == null)
			{
				return true;
			}
			Type type = obj.GetType();
			return type.IsGenericType && type.GetGenericTypeDefinition() == mNullableType;
		}

		public override bool IsDeserializable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == mNullableType;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			if (o == null)
			{
				return JsonNull.Instance;
			}
			Type type = o.GetType();
			PropertyInfo pi = type.GetProperty("Value");
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