using System;
using System.Collections.Generic;

namespace ProjectTo
{
	class ProjectionCache
	{
		public static ProjectionCache Current = new ProjectionCache();

		private readonly object lockObject = new object();
		private readonly Dictionary<UInt64, object> dict = new Dictionary<UInt64, object>();

		public object FindValue(string key)
		{
			lock (lockObject)
			{
				var hash = CalculateHash(key);
				return dict.ContainsKey(hash) ? dict[hash] : null;
			}
		}

		public void SetValue(string key, object value)
		{
			lock (lockObject)
			{
				var hash = CalculateHash(key);
				dict[hash] = value;
			}
		}

		static UInt64 CalculateHash(string read)
		{
			//Knuth hash
			UInt64 hashedValue = 3074457345618258791ul;
			for (int i = 0; i < read.Length; i++)
			{
				hashedValue += read[i];
				hashedValue *= 3074457345618258799ul;
			}
			return hashedValue;
		}
	}
}