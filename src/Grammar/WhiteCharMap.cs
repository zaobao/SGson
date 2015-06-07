using System;

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
			WhiteCharType.White,	WhiteCharType.Ignored,
			WhiteCharType.Ignored,	WhiteCharType.White,
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

		public WhiteCharType this[int index]
		{
			get
			{
				if (index < 33 && index > -1)
				{
					return ascii_class[index];
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