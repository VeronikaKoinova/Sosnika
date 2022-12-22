using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFApplicationOptika.PageMain
{
    /// <summary>
    /// Логика взаимодействия для WindowCaptcha.xaml
    /// </summary>
    public partial class WindowCaptcha : Window
    {
		string text = String.Empty;
		DispatcherTimer _timer;
		TimeSpan _time;
		public WindowCaptcha()
		{
			InitializeComponent();
			ReloadCATPCHA();
		}

		private void Timer()
		{
			EnterCaptcha.IsEnabled = false;
			_time = TimeSpan.FromSeconds(10);
			_timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
			{
				TextCaptcha.Content = _time.ToString("c");
				if (_time == TimeSpan.Zero)
				{
					_timer.Stop();
					ReloadCATPCHA();
					EnterCaptcha.IsEnabled = true;
				}
				_time = _time.Add(TimeSpan.FromSeconds(-1));
			}, Application.Current.Dispatcher);
			_timer.Start();
		}
		private void ReloadCATPCHA()
		{
			text = "";
			Random rnd = new Random();
			string array = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
			for (int i = 0; i < 5; ++i)
				text += array[rnd.Next(array.Length)];
			TextCaptcha.Content = text;
		}
		private void EnterCaptcha_Click(object sender, RoutedEventArgs e)
		{
			if (TextBoxCAPTCHA.Text == text)
				this.Close();
			else
			{
				MessageBox.Show("Не правильный код, блокировка на 10 секунд");
				TextBoxCAPTCHA.Text = "";
				Timer();
			}
		}
    }
}
