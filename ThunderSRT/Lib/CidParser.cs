using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.Cryptography;

namespace ThunderSRT.Lib
{
	static class CidParser
	{
		static Dictionary<string,string> _cache = new Dictionary<string, string>();

		public static string GetCid(string path)
		{
			//避免重试的时候重复计算cid
			if (_cache.TryGetValue(path, out var cid))
			{
				return cid;
			}

			var data = loadData(path).SelectMany(i=>i).ToArray();
			cid = getSha(data);
			_cache[path] = cid;
			return cid;
		}

		static IEnumerable<byte[]> loadData(string file)                      
		{
			var size = new FileInfo(file).Length;
			using (var mem = MemoryMappedFile.CreateFromFile(file))
			{
				if (size < 0xf000)
				{
					yield return readBytes(0, (int)size);
				}
				else
				{
					yield return readBytes(0, 0x5000);
					yield return readBytes(size / 3, 0x5000);
					yield return readBytes(size - 0x5000, 0x5000);
				}

				byte[] readBytes(long offset, int len)
				{
					using (var view = mem.CreateViewAccessor(offset, len))
					{
						var buffer = new byte[len];
						view.ReadArray(0, buffer, 0, len);
						return buffer;
					}
				}
			}
		}


		static string getSha(byte[] buffer)
		{
			using (var sha1 = SHA1.Create())
			{
				var data = sha1.ComputeHash(buffer);
				return BitConverter.ToString(data).Replace("-", "");
			}
		}
	}
}