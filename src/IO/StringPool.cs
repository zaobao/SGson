using System;

namespace SGson.IO
{
	class StringPool
	{
		private const int mask = 511;

		private string[] pool = new string[512];

		private readonly uint seed = (uint)new Random().Next(0, ushort.MaxValue);

		public string Intern(char[] array, int length)
		{
			uint hashCode = seed;
			for (int i = 0; i < length; i++)
			{
				hashCode = (hashCode * 31) + array[i];
			}

			hashCode ^= (hashCode >> 20) ^ (hashCode >> 12);
			hashCode ^= (hashCode >> 7) ^ (hashCode >> 4);
			uint index = hashCode & mask;

			String pooled = pool[index];
			if (pooled == null || pooled.Length != length)
			{
				string result = new String(array, 0, length);
				pool[index] = result;
				return result;
			}

			for (int i = 0; i < length; i++)
			{
				if (pooled[i] != array[i])
				{
					string result = new String(array, 0, length);
					pool[index] = result;
					return result;
				}
			}
			return pooled;
		}
	}
}