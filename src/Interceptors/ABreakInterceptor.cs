using System;

namespace SGson.Interceptors
{
	public abstract class ABreakInterceptor
	{
		protected internal Gson Context;

		public abstract bool IsSerializable(object obj);
		public abstract bool IsDeserializable(Type type);
		public abstract JsonElement InterceptWhenSerialize(object o);
		public abstract object InterceptWhenDeserialize(JsonElement je, Type type);

		public ABreakInterceptor Clone()
		{
			return (ABreakInterceptor)this.MemberwiseClone();
		}
	}
}