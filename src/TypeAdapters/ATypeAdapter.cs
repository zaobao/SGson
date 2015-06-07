using System;

namespace SGson.TypeAdapters
{
	public abstract class ATypeAdapter
	{
		protected internal Gson Context;

		public abstract JsonElement Serialize(object o);
		public abstract object Deserialize(JsonElement je, Type originalType);

		public ATypeAdapter Clone()
		{
			return (ATypeAdapter)this.MemberwiseClone();
		}
	}
}