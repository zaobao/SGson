using System;
using System.Collections.Generic;

namespace SGson.Grammar
{
	public class WhiteCharMap
	{
		private static readonly WhiteCharType[] ascii_class = new WhiteCharType[]
		{
			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,

			WhiteCharType.Ignored,	WhiteCharType.White,
			WhiteCharType.White,	WhiteCharType.White,
			WhiteCharType.White,	WhiteCharType.White,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,

			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,

			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.Ignored,

			WhiteCharType.White
		};

		private static readonly HashSet<int> other_whitespaces = new HashSet<int>()
		{
			0x0085,
			0x00A0,
			0x1680,
			0x180E,
			0x2028,
			0x2029,
			0x202F,
			0x205F,
			0x2060,
			0x3000,
			0xFEFF
		};

		public WhiteCharType this[int index]
		{
			get
			{
				if (index >= 0 && index < 33)
				{
					return ascii_class[index];
				}
				if (index >= 128 && index < 160)
				{
					return WhiteCharType.Ignored;
				}
				if (index >= 0x2000 && index <= 0x200D)
				{
					return WhiteCharType.White;
				}
				if (other_whitespaces.Contains(index))
				{
					return WhiteCharType.White;
				}
				if (index <= 0xFFFF)
				{
					return WhiteCharType.Etc;
				}
				throw new Exception(index + "is not a char");
			}
		}

		private static WhiteCharMap instance = new WhiteCharMap();
		public static WhiteCharMap Instance { get { return instance; } }
	}
}