using System;

using Xamarin.Forms;

namespace pomodoroTimer
{
	public class CircularProgressBarView : View
	{
		public CircularProgressBarView()
		{
		}

		public static readonly BindableProperty ProgressColorProperty = 
			BindableProperty.Create<CircularProgressBarView, Color> (p => p.ProgressColor, Color.Red);

		public Color ProgressColor
		{
			get { return (Color)GetValue(ProgressColorProperty); }
			set { SetValue(ProgressColorProperty, value); }
		}
	}
}