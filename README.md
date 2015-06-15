# SGson 用户手册
Simple Gson for C#

## 概览

SGson是一个C#的JSON库，用于JSON格式的序列化和反序列化。

作者认为Google Gson的使用体验比较好，将仿照它的功能写一个C#下的JSON库，提供一些基础的好用的功能，但API会依照C#的一些特性有所调整。


## SGson目标

* 提供System.Collections.Generic下所以类包括接口的默认序列化/反序列化功能
* 在JSON标准格式下，允许自定义序列化/反序列化格式
* 反序列化时，兼容非标准格式JSON输入，尽可能兼容EMCAScript语法
* 序列化输出标准格式JSON
* 提供友好的语法错误定位功能

## 使用SGson

### 基础类型

SGson将C#中字符串和所有数字类型序列化为JSON的string和number，但不包括char类型，留给用户可以自己决定将它序列化为字符串还是数字。

#### 数字类型扩展

除了JSON标准中的double类型以外，还支持JavaScript中16进制和8进制表示形式，并支持EMCAScript6中2进制和新的8进制表示方式。

#### 数字类型精度损失
无论是什么数字类型，SGson精度统一以IEEE754规定的双精度浮点数为准，这和EMCASript标准一致，也就是说不计符号位，有效二进制位数为53，以后会考虑提高long的精度至63位，ulong的精度至64位。

### 一般对象

SGson在序列化和反序列化对象时，只会使用public的属性，并且序列化时只会关注声明了get访问器的属性，反序列化是只会关注声明了set访问器的属性。这样，用户在声明类型时，就可以更细粒度地控制一个对象哪些属性需要序列化出来，哪些属性需要反序列化进去。

在构造对象时同一使用其无参构造器，必须确保该类型有一个public的无参构造器。

#### 对object的反序列化策略

如果对象的类型声明为object，将会使用以下策略反序列化。

* 如果在JSON中是数字，会被反序列化为double类型
* 如果在JSON中是字符串，会被反序列化为string类型
* 如果在JSON中是数组，会被反序列化为List<object>类型，这里的泛型obhect类型也使用此规则反序列化
* 如果在JSON中是对象，被反序列化为Dictionary<string, object>类型，这里的泛型obhect类型也使用此规则反序列化

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

### 自定义序列化和反序列化方式
SGson自定义序列化和反序列化的方式有三种：注册委托、注册适配器、注册拦截器，SGson内建支持的C#格式也是通过这三种方式来定义的。其中适配器方式类似Google Gson的自定义方式。

#### 注册委托

#### 注册适配器

#### 注册拦截器

### Null值处理

JSON中的null会被反序列化为null；序列化时为null的字段也会被序列化为null元素，而不是跳过忽略。

如果整个JSON字符串是空字符串——""，那么也会被反序列化会null，反之null不会被序列化为空字符串，而是null元素.
