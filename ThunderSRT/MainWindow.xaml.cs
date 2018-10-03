using System.IO;
using System.Windows;

namespace ThunderSRT
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			this.AllowDrop = true;
			this.DragOver += (s, e) =>
			{
				var isValid = tryGetDropFile(e, out var file);
				e.Effects = isValid ? DragDropEffects.Link : DragDropEffects.None;
			};

			this.Drop += (s, e) =>
			{
				tryGetDropFile(e, out var file);
				this.DataContext = new TitleContext(file);
			};

			bool tryGetDropFile(DragEventArgs e, out string file)
			{
				var files = e.Data.GetData(DataFormats.FileDrop) as string[];
				if (files?.Length >= 1 && File.Exists(files[0]))
				{
					file = files[0];
					return true;
				}
				else
				{
					file = null;
					return false;
				}
			}
		}

		private void onDownload(object sender, RoutedEventArgs e)
		{
			(this.DataContext as TitleContext)?.Download();
		}
	}
}