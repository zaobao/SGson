using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using SGson;
using SGson.Interceptors;
using SGson.Reflection;

namespace SGson.Example
{
	public class InterceptorExample
	{
		public class KeyedCollectionInterceptor : AInterceptor
		{
			private static readonly Type mGenericType = typeof(KeyedCollection<,>);

			private static Type GetKeyedICollectionType(Type type)
			{
				while(type != null && 
					!(type.IsGenericType && type.GetGenericTypeDefinition() == mGenericType))
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
				return GetKeyedICollectionType(obj.GetType()) != null;
			}

			public override bool IsDeserializable(Type type)
			{
				return GetKeyedICollectionType(type) != null;
			}

			public override JsonElement InterceptWhenSerialize(object o)
			{
				JsonMap jm = new JsonMap();
				MethodInfo method = o.GetType().GetMethod("GetKeyForItem", BindingFlags.Instance | BindingFlags.NonPublic);
				foreach (object item in (IEnumerable)o)
				{
					object key = method.Invoke(o, new object[] {item});
					JsonString keyStr = Context.ToJsonTree(key) as JsonString;
					if (keyStr == null)
					{
						keyStr = key.ToString();
					}
					jm.Add(keyStr, Context.ToJsonTree(item));
				}
				return jm;
			}

			public override object InterceptWhenDeserialize(JsonElement je, Type type)
			{
				if (je == null || je is JsonNull)
				{
					return null;
				}
				if (!(je is JsonMap))
				{
					throw new Exception("Expect a map, but " + je);
				}
				JsonMap jm = (JsonMap)je;
				Type[] genericArguments = GetKeyedICollectionType(type).GetGenericArguments();
				object o = PocsoUtils.GetInstance(type);
				MethodInfo method = type.GetMethod("Add");
				foreach (KeyValuePair<string,JsonElement> kv in jm)
				{
					method.Invoke(o, new Object[]
					{
						Context.FromJsonTree(kv.Value, genericArguments[1])
					});
				}
				return o;
			}
		}

		public class SimpleOrder : KeyedCollection<int, OrderItem>
		{
			protected override int GetKeyForItem(OrderItem item)
			{
				return item.PartNumber;
			}
		}

		public class OrderItem
		{
			public int PartNumber { get; set; }
			public string Description { get; set; }
			public double UnitPrice { get; set; }

			private int _quantity = 0;

			public int Quantity	
			{
				get { return _quantity; }
				set
				{
					if (value<0)
						throw new ArgumentException("Quantity cannot be negative.");

					_quantity = value;
				}
			}
		}

		public static void Main()
		{
			Gson gson = new GsonBuilder()
				.RegisterInterceptor(new KeyedCollectionInterceptor())
				.Create();
			SimpleOrder so = gson.FromJson<SimpleOrder>(@"{
'110072674': {PartNumber: 110072674, Description: 'Widget', UnitPrice:400, Quantity:45.17},
'110072675': {PartNumber: 110072675, Description: 'Sprocket', UnitPrice:27, Quantity:5.3},
'101030411': {PartNumber: 101030411, Description: 'Motor', UnitPrice:10, Quantity:237.5},
'110072684': {PartNumber: 110072684, Description: 'Gear', UnitPrice:175, Quantity:5.17}
}");
			Console.WriteLine(gson.ToJson(so));
		}

	}
}