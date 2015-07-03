# SGson 用户手册

Simple Gson for C#

## 概览

SGson是一个C#的JSON库，用于JSON格式的序列化和反序列化。

仿照Google Gson写一个C#下的JSON库，但API会依照C#的一些特性有所调整。

## 功能概述

* 常用类包括接口的默认序列化/反序列化功能
* 自定义序列化/反序列化格式
* 兼容非标准格式JSON输入，兼容EMCAScript6的数字格式
* 序列化输出标准格式JSON
* 语法错误定位

## 使用SGson

### 基础类型

SGson将C#中字符串和所有数字类型序列化为JSON的string和number，但不包括char类型，char类型留给用户可以自己决定将它序列化为字符串还是数字。

#### 数字类型扩展

除了JSON标准中的double类型以外，还支持JavaScript中16进制和8进制表示形式，并支持EMCAScript6中2进制和新的8进制表示方式。

#### 数字类型精度损失
无论是什么数字类型，SGson精度统一以IEEE754规定的双精度浮点数为准，这和EMCASript标准一致，也就是说不计符号位，有效二进制位数为53，以后会考虑提高long的精度至63位，ulong的精度至64位。

### 一般对象

SGson在序列化和反序列化对象时，只会使用public的属性，并且序列化时只会关注声明了get访问器的属性，反序列化是只会关注声明了set访问器的属性。这样，用户在声明类型时，就可以更细粒度地控制一个对象哪些属性需要序列化出来，哪些属性需要反序列化进去。

在构造对象时同一使用其无参构造器，必须确保该类型有一个public的无参构造器。

#### 对object的反序列化策略

如果对象的类型声明为object，将会使用以下策略反序列化：  
如果在JSON中是数字，会被反序列化为double类型；  
如果在JSON中是字符串，会被反序列化为string类型；  
如果在JSON中是数组，会被反序列化为List<object>类型，这里的泛型obhect类型也使用此规则反序列化；  
如果在JSON中是对象，被反序列化为Dictionary<string, object>类型，这里的泛型obhect类型也使用此规则反序列化。

### IDictionary<,>

SGson支持IDictionary<string,>和JSON对象之前的转换，注意Dictionary的key必须是string类型

### 数组

SGson默认支持的数组类型有，C#中的数组，继承了IEnumerable<T>接口、ICollection<T>接口的类型（比如List<T>，但继承自IDictionary<,>接口的类型除外），以及Stack<T>、Queue<T>类型

#### 一维数组

#### 多维数组

反序列化为多维数组的JSON必须保证单个维度中的所有长度一致。

#### List<T>

#### IEnumerable<T>

#### ICollection<T>

#### Stack<T>

Stack<T>依照先进后出的方式处理，所以对应到JSON中的数组顺序是反的

#### Queue<T>

### 日期

内建只支持DateTime类型，格式为"yyyy-MM-dd HH-mm-ss"。

如果我想修改内建的日期格式，比如XML标准的"yyyy-MM-ddTHH-mm-ss"，下面章节中有示例。

### 自定义序列化和反序列化方式

SGson自定义序列化和反序列化的方式有三种：注册委托、注册适配器、注册拦截器，SGson内建支持的C#格式也是通过这三种方式来定义的。其中适配器方式类似Google Gson的自定义方式。

三种方式优先级为，拦截器 > 适配器 > 委托，同时定义以拦截器优先，然后对于每种类型已适配器优先。

内建的数字类型、字符串和DateTime的解析都是通过注册委托的方式定义，所以无论使用哪种方式注册都会成功覆盖掉原有自带的序列化/反序列化方式。


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
下面是修改

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
192.168.0.1
"192.168.0.1"

#### 注册拦截器

如果想自定义对接口或者泛型类的解析方式，就会用到拦截器。拦截器的优先级最高，当你不能明确需要处理的对象是哪一种类型时（例如要一组类型或所有类型进行处理），推荐使用拦截器。

使用GsonBuilder.RegisterBreakInterceptor可以注册拦截器，拦截器定义的示例可以查看src/Interceptors中的源代码。

### Null值处理

JSON中的null会被反序列化为null，如果对应类型是不可位空的数字类型，遇到"null"，默认会反序列化为0。

序列化时为null的字段也会被序列化为null元素，而不是跳过忽略。

如果整个JSON字符串是空字符串——""，那么也会被反序列化会null，反之null不会被序列化为空字符串，而是null元素。


### Nullable类型

可空类型会被识别，并按照其泛型类型的方式处理，但如果值为空，会被当做Null处理。
例如，"null"默认反序列化为int类型为0，反序列化为int?类型为Null值

### 线程安全

一个SGson对象虽然可以安全地重复使用，但并不是线程安全的。如果考虑多线程，请使用ThreadLocal。
