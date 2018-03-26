using System;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using SGson;

[assembly: AssemblyTitle ("SGson.dll")]
[assembly: AssemblyDescription ("A C# library to convert JSON to C# objects and vice-versa")]
[assembly: AssemblyDefaultAlias ("SGson.dll")]

[assembly: AssemblyCompany ("")]
[assembly: AssemblyProduct ("SGson")]
[assembly: AssemblyCopyright ("Copyright © Zaobao 2018")]
[assembly: AssemblyVersion (Gson.Version)]
[assembly: SatelliteContractVersion (Gson.Version)]
[assembly: AssemblyInformationalVersion (Gson.Version)]
[assembly: AssemblyFileVersion (Gson.Version)]

[assembly: ComVisible (false)]
[assembly: AllowPartiallyTrustedCallers]