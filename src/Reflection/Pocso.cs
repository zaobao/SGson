using System;
using System.Collections.Generic;

namespace SGson.Reflection {

	public abstract class Pocso
	{

		public Gson Gson;

		public override string ToString()
		{
			return Gson.ToJson(this);
		}

		public Dictionary<string, object> ToDictionary()
		{
			return PocsoUtils.O2d(this);
		}

		public T To<T>()
			where T : class, new()
		{
			return PocsoUtils.O2o<T,Pocso>(this);
		}

		public static implicit operator Dictionary<string, object>(Pocso obj)
		{
			return PocsoUtils.O2d(obj);
		}

		public static implicit operator string(Pocso obj)
		{
			return obj.ToString();
		}

	}

}