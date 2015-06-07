using System;
using System.Collections.Generic;
using System.Reflection;

using SGson.Reflection;

namespace SGson.TypeAdapters
{
	public class ObjectAdapter : ATypeAdapter
	{
		public override JsonElement Serialize(object o)
		{
			Dictionary<string, object> dic = PocsoUtils.O2d<object>(o);
			JsonMap jm = new JsonMap();
			foreach (KeyValuePair<string, object> keyValuePair in dic)
			{
				jm.Add(keyValuePair.Key, Context.ToJsonTree(keyValuePair.Value));
			}
			return jm;
		}

		public override object Deserialize(JsonElement je, Type originalType)
		{
			if (je == null || je.IsJsonNull)
			{
				return null;
			}
			if (originalType == typeof(Object))
			{
				if (je.IsJsonMap)
				{
					return Context.FromJsonTree(je, typeof(Dictionary<string,object>));
				}
				if (je.IsJsonArray)
				{
					return Context.FromJsonTree(je, typeof(List<object>));
				}
				if (je.IsJsonString)
				{
					return Context.FromJsonTree(je, typeof(string));
				}
				if (je.IsJsonNumber)
				{
					return Context.FromJsonTree(je, typeof(double));
				}
				if (je.IsJsonBoolean)
				{
					return Context.FromJsonTree(je, typeof(bool));
				}
			}
			if (je.IsJsonMap)
			{
				object o = PocsoUtils.GetInstance(originalType);
				Dictionary<string, PropertyInfo> map = PocsoUtils.GetGettablePropertyInfoMap(originalType);
				Dictionary<string,JsonElement> dic = (Dictionary<string,JsonElement>)(JsonMap)je;
				foreach (KeyValuePair<string, PropertyInfo> keyValuePair in map)
				{
					PropertyInfo pi = keyValuePair.Value;
					JsonElement element;
					if (dic.TryGetValue(keyValuePair.Key, out element))
					{
						pi.SetValue(o, Context.FromJsonTree(element, pi.PropertyType), null);
					}
				}
				return o;
			}
			throw new Exception(String.Format("No specific deserializer for type {0}.Type not match, can not convert {0} to object.", originalType,je.GetType()));
		}
	}
}