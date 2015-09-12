using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

using SGson;
using SGson.Test.Entity;

namespace SGson.Test
{
	public class GsonTest
	{
		private static void TestNullJson()
		{
			Gson gson = new Gson();
			string str = null;
			ConsoleColor dfc = ConsoleColor.Black;
			Console.ForegroundColor = dfc;
			Console.BackgroundColor = ConsoleColor.White;
			Console.Write("Check Null ");
			for (int i = 0; i < Math.Max(Console.WindowWidth - 20 - 1, 4); i++)
			{
				Console.Write("-");
			}
			if (gson.FromJson<AllInOneCase>(str) == null)
			{
				Console.Write("---- [");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("OK");
				Console.ForegroundColor = dfc;
				Console.WriteLine("]");
			}
			else
			{
				Console.Write(" [");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Failed");
				Console.ForegroundColor = dfc;
				Console.WriteLine("]");
			}
		}

		private static void TestEmptyJson()
		{
			Gson gson = new Gson();
			string str = "";
			ConsoleColor dfc = Console.ForegroundColor;
			Console.Write("Check EmptyJson ");
			for (int i = 0; i < Math.Max(Console.WindowWidth - 25 - 1, 4); i++)
			{
				Console.Write("-");
			}
			if (gson.FromJson<AllInOneCase>(str) == null)
			{
				Console.Write("---- [");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("OK");
				Console.ForegroundColor = dfc;
				Console.WriteLine("]");
			}
			else
			{
				Console.Write(" [");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Failed");
				Console.ForegroundColor = dfc;
				Console.WriteLine("]");
			}
		}

		private static void TestAllInOne()
		{
			GsonBuilder jb = new GsonBuilder();
			Gson gson = jb.SetVisitedObjectCountLimit(10000).SetVisitedObjectStackLength(5).Create();
			StreamReader sr = new StreamReader("JsonText/AllInOneJsonInput.txt", Encoding.UTF8);
			AllInOneCase obj = gson.FromJson<AllInOneCase>(sr);
			sr.Close();
			StreamReader sr1 = new StreamReader("JsonText/AllInOneJsonOutput.txt", Encoding.UTF8);
			string str = sr1.ReadToEnd();
			ConsoleColor dfc = Console.ForegroundColor;
			Console.Write("Check AllInOneCase ");
			for (int i = 0; i < Math.Max(Console.WindowWidth - 28 - 1, 4); i++)
			{
				Console.Write("-");
			}
			if (gson.ToJson(obj) == str)
			{
				Console.Write("---- [");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("OK");
				Console.ForegroundColor = dfc;
				Console.WriteLine("]");
			}
			else
			{
				Console.Write(" [");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Failed");
				Console.ForegroundColor = dfc;
				Console.WriteLine("]");
				Console.WriteLine(gson.ToJson(obj));
			}
		}

		public static void Main()
		{
			TestNullJson();
			TestEmptyJson();
			TestAllInOne();
		}
	}
}