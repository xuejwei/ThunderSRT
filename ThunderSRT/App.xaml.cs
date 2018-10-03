using System;
using System.Windows;
using System.Windows.Threading;

namespace ThunderSRT
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			Application.Current.DispatcherUnhandledException += onError;

			var uri = new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative);
			App.Current.Resources.Source = uri;
			base.OnStartup(e);
		}

		private void onError(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;

			var errMsg = getErrorMsg(e.Exception);
			MessageBox.Show(errMsg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);

			string getErrorMsg(Exception err)
			{
				if (err is AggregateException && err.InnerException != null)
				{
					return getErrorMsg(err.InnerException);
				}

				return err.Message;
			}
		}
	}
}
