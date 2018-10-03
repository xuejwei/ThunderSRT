using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Polly;
using PropertyChanged;
using ThunderSRT.Lib;

namespace ThunderSRT
{
	[AddINotifyPropertyChangedInterface]
	class TitleContext
	{
		static Policy _policy;

		static TitleContext()
		{
			_policy = Policy.Handle<Exception>().WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(1));
		}

		TitleDownloader _downloader;
		
		public TitleContext(string file)
		{
			File        = new FileInfo(file);
			_downloader = new TitleDownloader(file);
			Task.Run(loadTitleData);
		}


		async Task loadTitleData()
		{
			try
			{
				IsBusy = true;
				var titles = await _policy.ExecuteAsync(_downloader.GetTitles);
				ItemsSource  = titles;
				SelectedItem = titles?.FirstOrDefault();
			}
			finally
			{
				IsBusy = false;
			}
			
		}

		public bool IsBusy { get; private set; }


		public async void Download()
		{
			
			if (SelectedItem == null)
			{
				throw new InvalidOperationException("没有选择字幕文件");
			}

			try
			{
				IsBusy = true;
				await _policy.ExecuteAsync(() => _downloader.DownloadTitle(SelectedItem));
				MessageBox.Show("下载完成", "", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public FileInfo File { get; }

		public TitleData[] ItemsSource { get; private set; }

		public TitleData SelectedItem { get; set; }
	}
}