# SGson 用户手册

Simple Gson for C#

## 概览

SGson是一个C#的JSON库，用于JSON格式的序列化和反序列化。

仿照Google Gson写一个C#下的JSON库，但API会依照C#的一些特性有所调整。

SGson类的对象中的方法不是线程安全的，如果在多线程环境下编程，请看文档末尾。

## 功能概述

* 常用类包括接口的默认序列化/反序列化功能
* 自定义序列化/反序列化格式
* 兼容非标准格式JSON输入，兼容ECMAScript6的数字格式
* 序列化输出标准格式JSON
* 语法错误定位

## 使用SGson

### 基础类型

SGson将C#中字符串和所有数字类型序列化为JSON的string和number，但不包括char类型，char类型留给用户可以自己决定将它序列化为字符串还是数字。

#### 字符串
##### 反序列化
字符串反序列化支持Json官方定义的格式
```
string
    ""
    " chars "
chars
    char
    char chars
char
    any-Unicode-character-
        except-"-or-\-or-
        control-character
    \"
    \\
    \/
    \b
    \f
    \n
    \r
    \t
    \u four-hex-digits 
```
##### 字符串序列化格式
字符串序列化按照上面官方规则序列化，规则应用优先级自上而下递减。即正斜杠被序列化为"/"而不是"\/"；能使用"\\\*"表示的控制字符不会被序列化为"\u****"，比如换行符被序列化为"\n"，而不是"\u000A"。

#### 数字类型扩展

除了JSON标准中的double类型以外，还支持JavaScript中16进制和8进制表示形式，并支持ECMAScript6中2进制和新的8进制表示方式。

#### 数字类型无损转换

在ECMASript标准中，只有一种数字类型，以IEEE754规定的双精度浮点数为准，也就是说不计符号位，有效二进制位数为53，但SGson并不以double作为通用的数字类型：long的精度保持在63位；ulong的精度保持在64位；并且decimal类型，也不会被强转成double而导致精度损失。

使用SGson，只要序列化和反序列化两边数字类型一致，就可以无损拷贝。
但两边类型不一致时就会令人疑惑，例如从float到double的转换，(double)4.6f和double.Parse("4.6"))并不相等，SGson会取后者。
所谓无损是指在序列化和反序列化两边数字类型不一致时，其JSON表示方式是一样的。使用SGson序列化和反序列化时，所有的数字类型都以其字符串表现形式为准。
例如在一次数据传输过程中，序列化一方的实体类型的一个字段是float，其值是x，反序列化的实体类型的对应字段是double。
那么反序列化方收到的double数值是double.Parse(x.ToString())，而不是(double)x。反序列化方收到的double数值再次序列化时将会和x.ToString()一样。

### 一般对象

SGson在序列化和反序列化对象时，只会使用public的属性，并且序列化时只会关注声明了get访问器的属性，反序列化是只会关注声明了set访问器的属性。这样，用户在声明类型时，就可以更细粒度地控制一个对象哪些属性需要序列化出来，哪些属性需要反序列化进去。

在构造对象时同一使用其无参构造器，必须确保该类型有一个public的无参构造器。

#### 对object的反序列化策略

如果对象的类型声明为object，将会使用以下策略反序列化：  
如果在JSON中是数字，会被反序列化为double类型；  
如果在JSON中是字符串，会被反序列化为string类型；  
如果在JSON中是数组，会被反序列化为List<object>类型，这里的泛型obhect类型也使用此规则反序列化；  
如果在JSON中是对象，被反序列化为Dictionary<string, object>类型，这里的泛型obhect类型也使用此规则反序列化。

### IDictionary<TKey,>

SGson默认支持IDictionary<TKey,>类型的反序列化，Dictionary的key必须是string类型或者能用System.Convert装换为string类型（所以不包括int?之类的类型），或者使用当前Gson对象可以将该C#key的对象从key对应的JsonString反序列化出来（此优先级最高），也就是说，只要自定义了一种类型的反序列化方式为从json中的字符串反序列化出来，那么这种类型的key也就可以被反序列化出来了。因为DateTime默认序列化为json的字符串值，所以DateTime?类型的key是被支持的。

SGson默认支持IDictionary类型的序列化，key取值为Dictionary的key.ToString()，或者使用当前Gson对象可以将该C#key的对象序列化为JsonString对象（此优先级最高），也就是说，只要自定义了一种类型的序列化方式为序列化为json中的字符串，那么这种类型的key也就可以被序列化了。因为DateTime默认从json的字符串值中反序列化出来，所以DateTime?类型的key是被支持的。

注意：SGson反序列化解析json时，认为key的正确形式是字母以及_、$开头的变量名以及任意用单/双引号括起的字符串，SGson在序列化时，无论C#类型是什么，key统一序列化为双引号括起的字符串。JS中null可以作为key，但其实和字符串"null"作为key等同，根据上述SGson中的规则null会被当做"null"来处理，这样既不违背ECMAScript标准，也和C#的Dictionary中不能有为null的key的规则相符。

KeyedCollection<,>默认当做数组处理，而不是dictionary。如要将其当做dictionary处理，见“注册拦截器”一节的示例。

### 数组

SGson默认支持的数组类型有，C#中的数组，继承了IEnumerable<T>接口、ICollection<T>接口的类型（比如List<T>，但继承自IDictionary<,>接口的类型除外），以及Stack<T>、Queue<T>类型

#### 一维数组

#### 多维数组

反序列化为多维数组的JSON必须保证每个维度上的所有向量长度一致。

#### List<T>

反序列化是，如果JS数组对应的变量的类型是object，会默认赋予该object类型变量一个List<T>的对象。

#### IEnumerable<T>

#### ICollection<T>

#### Stack<T>

Stack<T>依照先进后出的方式处理，所以对应到JSON中的数组顺序是反的。序列化和反序列化的顺序都和一般IEnumerable类型相反，所以序列化以后再反序列化，顺序不会改变，保证数据传递过程中不会改变。

SGson默认支持继承Stack<T>的封闭类型的类，如：
```csharp
public class MyIntStack : Stack<int>
```
以及开放类型的类，如
```csharp
public class MyStack<T> : Stack<T>
```

#### Queue<T>

SGson默认支持继承Queue<T>的封闭类型的类，如：
```csharp
public class MyIntQueue : Queue<int>
```
以及开放类型的类，如
```csharp
public class MyQueue<T> : Queue<T>
```


### 日期

内建只支持DateTime类型，格式为"yyyy-MM-dd HH-mm-ss"。

如果我想修改内建的日期格式，比如XML标准的"yyyy-MM-ddTHH-mm-ss"，下面章节中有示例。

### 自定义序列化和反序列化方式

SGson自定义序列化和反序列化的方式有三种：注册委托、注册适配器、注册拦截器，SGson内建支持的C#格式也是通过这三种方式来定义的。其中适配器方式类似Google Gson的自定义方式。

三种方式优先级为，拦截器 > 适配器 > 委托，拦截器优先级是最高的，对于每种类型以适配器优先。拦截器后注册的优先级更高，也就是后注册的拦截器会先拦截到当前需要序列化/反序列化的对象。经过拦截器过滤以后，会寻找适合该类的的适配器和委托，如未找到，则继续递归地对其父类进行适配器和委托进行匹配。注意：注册开放类型的适配器和委托没有任何意义，程序不支持匹配开放类型。如果要自定义开放类型的序列化/反序列化方式，请使用拦截器。

内建的数字类型、字符串和DateTime的序列化/反序列化方式都是通过注册委托的方式定义，所以无论使用哪种方式注册都会成功覆盖掉原有自带的序列化/反序列化方式。

内建的接口、数组、开放类型的序列化/反序列化方式都是使用拦截器方式定义的，所以请使用注册的拦截器来自定义接口和泛型开放类型的解析方式。

内建的枚举的序列化/反序列化方式都是使用适配器定义的，所以若想重新定义它们的列化/反序列化方式，请时候适配器方式。

#### 注册委托

通过GsonBuider的RegisterSerializer<T>和RegisterDeserializer<T>可以轻松定义类型T的序列化/反序列化方式

如果我想修改内建的日期格式为"yyyy-MM-ddTHH-mm-ss"，因为反序列化使用DateTime.Parse方法，能够兼容不用修改，只需修改DateTime序列化方式：

```csharp
Gson gson = new GsonBuilder().RegisterSerializer<DateTime>(delegate(object x)
{
	return (JsonElement)(((DateTime)x).ToString("yyyy-MM-ddTHH:mm:ss"));
}).Create();

Console.WriteLine(gson.ToJson(DateTime.Now));
```

#### 注册适配器
和Google Gson相似的功能。在适配器中定义好序列化和反序列化的方法，注册到GsonBuilder中。注意考虑Null值的情况。

下面是通过自定义适配器，使SGson兼容IP地址类型的示例

定义适配器：
```csharp
class IPAddressAdaper : ATypeAdapter
{
	public override JsonElement Serialize(object o)
	{
		if (o == null)
		{
			return JsonNull.Instance;
		}
		return new JsonString(((IPAddress)o).ToString());
	}

	public override object Deserialize(JsonElement je, Type originalType)
	{
		if (je == JsonNull.Instance)
		{
			return null;
		}
		if (je.IsJsonString)
		{
			try
			{
				return IPAddress.Parse((string)(JsonString)je);
			}
			catch (Exception)
			{
				;
			}
		}
		throw new Exception(String.Format("Can not parse {0} to an IPAdress.", je.ToString()));
	}
}
```
使用适配器
```csharp
Gson gson = new GsonBuilder()
	.RegisterAdapter(new IPAddressAdaper(), typeof(IPAddress))
	.Create();

IPAddress ipa = gson.FromJson<IPAddress>("\"192.168.0.1\"");
Console.WriteLine(ipa);
Console.WriteLine(gson.ToJson(ipa));
```
输出结果为：
```
192.168.0.1
"192.168.0.1"
```

#### 注册拦截器

如果想自定义对接口或者泛型类的解析方式，就会用到拦截器。拦截器的优先级最高，当你不能明确需要处理的对象是哪一种类型时（例如要一组类型或所有类型进行处理或者是接口），推荐使用拦截器。

拦截器后注册的优先级更高，也就是后注册的拦截器会先拦截到当前需要序列化/反序列化的对象。

示例：
```csharp
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
```
输出结果为：
```
{"110072674":{"PartNumber":110072674,"Description":"Widget","UnitPrice":400,"Quantity":45},"110072675":{"PartNumber":110072675,"Description":"Sprocket","UnitPrice":27,"Quantity":5},"101030411":{"PartNumber":101030411,"Description":"Motor","UnitPrice":10,"Quantity":237},"110072684":{"PartNumber":110072684,"Description":"Gear","UnitPrice":175,"Quantity":5}}
```

### Null值处理

JSON中的null会被反序列化为null，如果对应类型是不可位空的数字类型，遇到"null"，默认会反序列化为0。

序列化时为null的字段也会被序列化为null元素，而不是跳过忽略。

JSON中没有出现的字段会被忽略，反序列化完后值就是default(x)。

如果整个JSON字符串是空字符串——""，那么也会被反序列化会null，反之null不会被序列化为空字符串，而是null元素。


### Nullable类型

可空类型会被识别，并按照其泛型类型的方式处理，但如果值为空，会被当做Null处理。
例如，"null"默认反序列化为int类型为0，反序列化为int?类型为Null值

### 线程安全

一个SGson对象虽然可以安全地重复使用，但并不是线程安全的。如果考虑用在多线程程序中，请使用ThreadLocal，示例如下
```csharp
public class JsonUtils
{
	private static readonly GsonBuilder builder = new GsonBuilder()
		.SetVisitedObjectCountLimit(int.MaxValue)
		.SetVisitedObjectStackLength(8);
	private static ThreadLocal<Gson> gsons = new ThreadLocal<Gson>();

	private static Gson GsonInstance
	{
		get
		{
			if (gsons.IsValueCreated)
			{
				return gsons.Value;
			}
			else
			{
				gsons.Value = builder.Create();
				return gsons.Value;
			}
		}
	}

	public static string ToJson(object obj)
	{
		return GsonInstance.ToJson(obj);
	}

	public static T FromJson<T>(string json)
	{
		return GsonInstance.FromJson<T>(json);
	}
}
```
