using System;
using Newtonsoft.Json;

namespace ThunderSRT.Lib
{
	class TitleData
	{
		[JsonProperty("scid")]
		public string Scid { get; set; }

		[JsonProperty("sname")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public string Language { get; set; }

		[JsonProperty("rate")]
		public long Rate { get; set; }

		[JsonProperty("surl")]
		public Uri Url { get; set; }

		[JsonProperty("svote")]
		public long Vote { get; set; }

		[JsonProperty("roffset")]
		public long Offset { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}