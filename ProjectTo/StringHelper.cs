using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTo
{
	internal class StringHelper
	{
		/// <summary>
		/// Splits string to all posible cases of CamelCase
		/// E.g.: DicSomeName=> [DicSome, Name], [Dic, SomeName]
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static IEnumerable<string[]> SplitToWordGroups(string s)
		{
			if (!s.Contains('_') && (s.ToUpperInvariant() == s || s.ToLowerInvariant() == s))
			{
				//nothing
			}
			else
			{
				var splitIndexes = GetSplitIndexes(s);
				var bitVectors = GetAllBitVectors(splitIndexes.Length);
				foreach (var v in bitVectors)
				{
					yield return SplitToParts(s, ReduceArray(splitIndexes, v));
				}
			}
		}

		static string[] SplitToParts(string s, int[] splitIndexes)
		{
			var result = new List<string>();
			int startIndex = 0;
			foreach (var i in splitIndexes)
			{
				result.Add(s.Substring(startIndex, i - startIndex).Trim('_'));
				startIndex = i;
			}
			if (startIndex < s.Length)
				result.Add(s.Substring(startIndex, s.Length - startIndex).Trim('_'));
			return result.ToArray();
		}

		static int[] ReduceArray(int[] array, short[] bitVector)
		{
			var result = new int[bitVector.Length];
			for (var i = 0; i < bitVector.Length; i++)
			{
				result[i] = array[bitVector[i]];
			}
			return result;
		}
		/// <summary>
		/// Get all bit vectors of length n.
		/// </summary>
		static short[][] GetAllBitVectors(int n)
		{
			//Общее число вариантов
			int totalNumber = 1 << n;
			var vectors = new List<Tuple>();
			for (var i = 1; i < totalNumber; i++)
			{
				int bitsCount = 0;
				var bitsPositions = new Stack<short>();
				for (var j = 0; j < n; j++)
				{
					if ((i & 1 << j) != 0)
					{
						bitsCount++;
						bitsPositions.Push((short)(n - 1 - j));
					}
				}
				var bitsArray = bitsPositions.ToArray();
				vectors.Add(new Tuple(i, bitsCount, bitsArray));
			}
			return vectors.OrderBy(v => v.Item2).ThenBy(v => v.Item1).Select(v => v.Item3).ToArray();
		}

		/// <summary>
		/// Splits the string into all possible cases of CamelCase.
		/// E.g.: DicSomeName=> [DicSome, Name], [Dic, SomeName]
		/// </summary>
		static int[] GetSplitIndexes(string s)
		{
			if (s.Contains('_'))
			{
				return AllIndexesOf(s, "_").ToArray();
			}
			var camelIndexes = s.Select((c, i) => new { c, i })
				.Where(c => c.i > 0)
				.Where(c => c.c >= 'A' && c.c <= 'Z')
				.Select(c => c.i).ToArray();
			return camelIndexes;
		}

		static List<int> AllIndexesOf(string str, string value)
		{
			List<int> indexes = new List<int>();
			for (int index = 0; ; index += value.Length)
			{
				index = str.IndexOf(value, index);
				if (index == -1)
					return indexes;
				indexes.Add(index);
			}
		}

		class Tuple
		{
			public Tuple(int item1, int item2, short[] item3)
			{
				this.Item1 = item1;
				this.Item2 = item2;
				this.Item3 = item3;
			}

			public int Item1 { get; set; }
			public int Item2 { get; set; }
			public short[] Item3 { get; set; }
		}
	}
}
