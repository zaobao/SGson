using System;

namespace SGson.Grammar
{
	public class IgnoredCharMap
	{
		private static readonly IgnoredCharType[] ascii_class = new IgnoredCharType[]
		{
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,

			IgnoredCharType.Ignored,	IgnoredCharType.Etc,
			IgnoredCharType.Etc,		IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Etc,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,

			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,

			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
			IgnoredCharType.Ignored,	IgnoredCharType.Ignored,
		};

		public IgnoredCharType this[int index]
		{
			get
			{
				if (index >= 0 && index < 32)
				{
					return ascii_class[index];
				}
				if (index >= 128 && index < 160)
				{
					return IgnoredCharType.Ignored;
				}
				if (index <= 0xFFFF)
				{
					return IgnoredCharType.Etc;
				}
				throw new Exception(index + "is not a char");
			}
		}

		private static IgnoredCharMap instance = new IgnoredCharMap();
		public static IgnoredCharMap Instance { get { return instance; } }
	}
}