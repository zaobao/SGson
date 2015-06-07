using System;

namespace SGson.Grammar.VarName
{
	public class VarNameCharMap
	{
		private static readonly VarNameCharType[] ascii_class = new VarNameCharType[]
		{
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,

			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,

			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,

			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,

			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,

			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,

			VarNameCharType.Digit,		VarNameCharType.Digit,
			VarNameCharType.Digit,		VarNameCharType.Digit,
			VarNameCharType.Digit,		VarNameCharType.Digit,
			VarNameCharType.Digit,		VarNameCharType.Digit,

			VarNameCharType.Digit,		VarNameCharType.Digit,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,

			VarNameCharType.Invalid,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,

			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,

			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,

			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.Invalid,
			VarNameCharType.Invalid,	VarNameCharType.AsFirst,

			VarNameCharType.Invalid,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,

			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,

			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,

			VarNameCharType.AsFirst,	VarNameCharType.AsFirst,
			VarNameCharType.AsFirst
		};

		public VarNameCharType this[int index]
		{
			get
			{
				if (index < 123 && index > -1)
				{
					return ascii_class[index];
				}
				if (index <= 0xFFFF)
				{
					return VarNameCharType.Invalid;
				}
				throw new Exception(index + "is not a char");
			}
		}

		private static VarNameCharMap instance = new VarNameCharMap();
		public static VarNameCharMap Instance { get { return instance; } }
	}
}