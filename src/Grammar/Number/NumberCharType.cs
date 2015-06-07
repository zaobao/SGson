namespace SGson.Grammar.Number
{
	public class NumberCharType
	{
		public static bool IsDigit(int c)
		{
			return c >= 0x30 && c <= 0x39;
		}

		public static bool IsBinaryDigit(int c)
		{
			return c == '0' || c =='1';
		}

		public static bool IsOctDigit(int c)
		{
			return c >= 0x30 && c <= 0x37;
		}

		public static bool IsHexDigit(int c)
		{
			return c >= 0x30 && c <= 0x39 ||
				c >= 0x41 && c <= 0x46 ||
				c >= 0x61 && c <= 0x66;
		}
	}
}