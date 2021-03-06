using System;

namespace SGson.Interceptors
{
	public abstract class AInterceptor
	{
		protected internal Gson Context;

		public abstract bool IsSerializable(object obj);
		public abstract bool IsDeserializable(Type type);
		public abstract JsonElement InterceptWhenSerialize(object o);
		public abstract object InterceptWhenDeserialize(JsonElement je, Type type);

		public AInterceptor Clone()
		{
			return (AInterceptor)this.MemberwiseClone();
		}
	}
}