using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SGson.Reflection {

	public class PocsoUtils
	{
		protected static readonly Object[] EmptyObjects = new Object[0];
		protected static readonly ParameterModifier[] EmptyParameterModifiers = new ParameterModifier[0];

		protected static Dictionary<Type, Dictionary<string, PropertyInfo>> publicGettableFieldsMap = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
		protected static Dictionary<Type, Dictionary<string, PropertyInfo>> publicSettableFieldsMap = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

		public static object GetInstance(Type t)
		{
			ConstructorInfo constructor = t.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, EmptyParameterModifiers);

			if (constructor == null)
			{
				throw new ArgumentException("Invalid type " + t.FullName + ". The type must have a public constructor that takes no parameters.", "t");
			}

			return constructor.Invoke(EmptyObjects);
		}

		public static Dictionary<string, PropertyInfo> GetGettablePropertyInfoMap(Type t)
		{
			Dictionary<string, PropertyInfo> map;
			if (!publicGettableFieldsMap.TryGetValue(t, out map))
			{
				map = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					// Must have a pulic get method, and the indexer will be ignored.
					.Where(x => x.GetGetMethod(false) != null &&　x.GetIndexParameters().Length == 0)
					.ToDictionary(k => k.Name, v => v);
				lock(publicGettableFieldsMap)
				{
					if (!publicGettableFieldsMap.ContainsKey(t))
					{
						publicGettableFieldsMap.Add(t, map);
					}
				}
			}
			return map;
		}

		public static Dictionary<string, PropertyInfo> GetSettablePropertyInfoMap(Type t)
		{
			Dictionary<string, PropertyInfo> map;
			if (!publicSettableFieldsMap.TryGetValue(t, out map))
			{
				map = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					// Must have a pulic set method, and the indexer will be ignored.
					.Where(x => x.GetSetMethod(false) != null &&　x.GetIndexParameters().Length == 0)
					.ToDictionary(k => k.Name, v => v);
				lock(publicSettableFieldsMap)
				{
					if (!publicSettableFieldsMap.ContainsKey(t))
					{
						publicSettableFieldsMap.Add(t, map);
					}
				}
			}
			return map;
		}

		public static Dictionary<string, object> O2d<T>(T o) where T : class
		{
			if (o == null)
			{
				return null;
			}

			Type t = o.GetType();
			Dictionary<string, PropertyInfo> map = GetGettablePropertyInfoMap(t);
			Dictionary<string, object> dic = new Dictionary<string, object>();
			foreach (KeyValuePair<string, PropertyInfo> kv in map)
			{
				try
				{
					dic.Add(kv.Key, kv.Value.GetValue(o, null));
				}
				catch (Exception)
				{
					;
				}

			}

			return dic;
		}

		public static T D2o<T>(Dictionary<string, object> dic) where T : class, new()
		{
			if (dic == null)
			{
				return null;
			}

			T o = new T();
			Type t = o.GetType();

			D2o(dic, t, o);

			return o;
		}

		public static object D2o(Dictionary<string, object> dic, Type t)
		{
			if (dic == null)
			{
				return null;
			}

			object o = GetInstance(t);

			D2o(dic, t, o);

			return o;
		}

		public static void D2o(Dictionary<string, object> dic, Type t, object o)
		{
			Dictionary<string, PropertyInfo> map = GetSettablePropertyInfoMap(t);
			foreach (KeyValuePair<string, PropertyInfo> kv in map)
			{
				object value;
				if (dic.TryGetValue(kv.Key, out value))
				{
					kv.Value.SetValue(o, value, null);
				}
			}
		}

		public static TResult O2o<TResult,TSource>(TSource src)
			where TResult : class, new()
			where TSource : class
		{
			if (src == null)
			{
				return default (TResult);
			}

			TResult result = new TResult();
			O2o<TSource>(src, typeof(TResult), result);

			return result;
		}

		public static object O2o<TSource>(TSource src, Type targetType)
			where TSource : class
		{
			if (src == null)
			{
				return null;
			}

			object dst = GetInstance(targetType);

			O2o<TSource>(src, targetType, dst);

			return dst;
		}

		public static void O2o<TSource>(TSource src, Type targetType, object dst)
			where TSource : class
		{
			Type tSrc = src.GetType();
			Dictionary<string, PropertyInfo> mapSrc = GetGettablePropertyInfoMap(tSrc);

			Dictionary<string, PropertyInfo> mapDst = GetSettablePropertyInfoMap(targetType);

			foreach (KeyValuePair<string, PropertyInfo> kvSrc in mapSrc)
			{
				PropertyInfo piDst;
				if (mapDst.TryGetValue(kvSrc.Key, out piDst))
				{
					try
					{
						piDst.SetValue(dst, kvSrc.Value.GetValue(src, null), null);
					}
					catch (Exception)
					{
						;
					}
				}
			}
		}

	}

}