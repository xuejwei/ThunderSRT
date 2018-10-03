using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ThunderSRT.Lib
{
	class TitleDownloader
	{
		static HttpClient _http;

		static TitleDownloader()
		{
			_http = new HttpClient(new HttpClientHandler()
			{
				UseProxy = false,
				Proxy = null,
			});
		}

		public FileInfo FileInfo { get;  }
		public TitleDownloader(string videoFile)
		{
			FileInfo = new FileInfo(videoFile);
		}


		public async Task<TitleData[]> GetTitles()
		{
			var cid = CidParser.GetCid(FileInfo.FullName);

			var url = $"http://sub.xmp.sandai.net:8000/subxl/{cid}.json";
			var content = await _http.GetStringAsync(url);

			return JsonConvert.DeserializeAnonymousType(content, new {sublist = new TitleData[0]})
			                  .sublist.OrderByDescending(i => i.Rate)
			                  .Where(i=>i.Url != null)
			                  .ToArray();
		}

		public async Task DownloadTitle(TitleData title)
		{
			var data = await _http.GetByteArrayAsync(title.Url);
			var path = Path.ChangeExtension(FileInfo.FullName, Path.GetExtension(title.Name));
			File.WriteAllBytes(path, data);
		}
	}
}
