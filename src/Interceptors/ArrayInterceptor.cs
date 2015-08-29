using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SGson.Reflection;

namespace SGson.Interceptors
{
	public class ArrayInterceptor : AInterceptor
	{

		// Not for one dimensional array
		public override bool IsSerializable(object obj)
		{
			return obj is Array && ((Array)obj).Rank > 1;
		}

		public override bool IsDeserializable(Type type)
		{
			return type.IsArray;
		}

		public override JsonElement InterceptWhenSerialize(object o)
		{
			return getJsonElementRecurse((Array)o, 0, new LinkedList<long>());
		}

		private JsonElement getJsonElementRecurse(Array array, int dimension, LinkedList<long> indices)
		{
			if (dimension < array.Rank)
			{
				JsonArray ja = new JsonArray();
				for (long i = array.GetLowerBound(dimension); i <= array.GetUpperBound(dimension); i++)
				{
					indices.AddLast(i);
					ja.Add(getJsonElementRecurse(array, dimension + 1, indices));
					indices.RemoveLast();
				}
				return ja;
			}
			return Context.ToJsonTree(array.GetValue(indices.ToArray()));
		}

		public override object InterceptWhenDeserialize(JsonElement je, Type type)
		{
			if (je == null || je is JsonNull)
			{
				return null;
			}
			int rank = type.GetArrayRank();
			long?[] lengthBuffer = new long?[rank];
			CheckIndexLimitRecurse(je, 0, rank, lengthBuffer);
			long[] lengths = lengthBuffer.Select(x => x.GetValueOrDefault()).ToArray();
			Type elementType = type.GetElementType();
			Array array = Array.CreateInstance(elementType, lengths);
			SetArrayRecurse(je, array, 0, new LinkedList<long>(), elementType);
			return array;
		}

		private void SetArrayRecurse(JsonElement je, Array array, int dimension,  LinkedList<long> indices, Type elementType)
		{
			if (dimension < array.Rank)
			{
				JsonArray ja = (JsonArray)je;
				for (int i = 0; i < ja.Length; i++)
				{
					indices.AddLast(i);
					SetArrayRecurse(ja[i], array, dimension + 1, indices, elementType);
					indices.RemoveLast();
				}
			}
			else
			{
				array.SetValue(Context.FromJsonTree(je, elementType), indices.ToArray());
			}
		}

		private void CheckIndexLimitRecurse(JsonElement je, int dimension, int rank, long?[] lengths)
		{
			if (dimension < rank)
			{
				if (je == null || je is JsonNull)
				{
					throw new Exception("Expect an array, but " + je.ToString());
				}
				if (!(je is JsonArray))
				{
					throw new Exception("Expect an array, but " + je.ToString());
				}
				JsonArray ja = (JsonArray)je;
				if (lengths[dimension] == null)
				{
					lengths[dimension] = ja.Length;
				}
				else if (lengths[dimension] != ja.Length)
				{
					throw new Exception("Expect an array of length " + lengths[dimension] + ", but " + je.ToString());
				}
				foreach(JsonElement element in ja)
				{
					CheckIndexLimitRecurse(element, dimension + 1, rank, lengths);
				}
			}
		}

	}
}