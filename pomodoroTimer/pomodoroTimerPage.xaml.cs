using System;
using Xamarin.Forms;
using System.Threading;

namespace pomodoroTimer
{
	public partial class pomodoroTimerPage : ContentPage
	{
		int pomodoroCounter = 0;
		public int workTime;
		public int interval;
		public int longInterval;
		public int countdownTime = 0;
		private bool _isRunningTimer = false;
		public int[] repeatOrder;

		public const string workingColor = "#D04255";
		public const string intervalColor = "#3EBA2B";

		public pomodoroTimerPage(int workTime = 3000, int interval = 2000, int longInterval = 4000)
		{
			InitializeComponent();
			startTimerButton.Clicked += OnStartTimerButtonClicked;
			stopTimerButton.Clicked += OnStopTimerButtonClicked;
			resetTimerButton.Clicked += OnResetTimerButtonClicked;
			this.workTime = workTime;
			this.interval = interval;
			this.longInterval = longInterval;
			this.repeatOrder = new int[]{ workTime, interval, workTime, interval, workTime, interval, workTime, longInterval };
			clockDisplay(workTime);
		}



		// タイマースタートボタンを押したらポモドーロをスタートする
		private void OnStartTimerButtonClicked(object sender, EventArgs e)
		{
			if (countdownTime == 0)
			{
				countdownTime = workTime;
			}
			startCountdown();
			countdownTimer(countdownTime);

		}

		// 指定された時間をカウントダウン
		private void countdownTimer(int limit)
		{
			if (_isRunningTimer)
			{
				return;
			}
			_isRunningTimer = true;
			clockDisplay(limit);
			Device.StartTimer(TimeSpan.FromSeconds(1), () => {
				if (_isRunningTimer)
				{

					limit -= 1000;
					countdownTime = limit;
					clockDisplay(limit);
					if (limit <= 0)
					{
						pomodoroCounter++;
						loopPomodoro();
					}
					return limit > 0;
				}
				return false;
			});
		}

		// 残り時間の表示を変更
		private void clockDisplay(int time)
		{
			double minute = time / 1000 / 60;
			int second = time / 1000 % 60;
			leftTime.Text = Math.Floor(minute).ToString("00") + ":" + second.ToString("00");
		}

		// カウントダウン中は一時停止ボタンとリセットボタンを表示
		private void startCountdown()
		{
			startTimerButton.IsVisible = false;
			stopTimerButton.IsVisible = true;
		}

		// タイマーの時間を繰り返し回数に合わせて変更する
		private void loopPomodoro()
		{
			if (pomodoroCounter >= repeatOrder.Length)
			{
				pomodoroCounter = 0;
			}
			_isRunningTimer = false;
			switchColor(pomodoroCounter);
			countdownTimer(repeatOrder[pomodoroCounter]);
		}

		// 作業中・休憩中で表示の色を変更する
		private void switchColor(int counter)
		{
			if (pomodoroCounter % 2 == 0)
			{
				leftTime.TextColor = Color.FromHex(workingColor);
				startTimerButton.TextColor = Color.FromHex(workingColor);
				stopTimerButton.TextColor = Color.FromHex(workingColor);
			}
			else {
				leftTime.TextColor = Color.FromHex(intervalColor);
				startTimerButton.TextColor = Color.FromHex(intervalColor);
				stopTimerButton.TextColor = Color.FromHex(intervalColor);
			}
		}

		// 一時停止ボタンを押したらカウントダウンを停止
		private void OnStopTimerButtonClicked(object sender, EventArgs e)
		{
			stopCountdown();

		}
		private void stopCountdown()
		{
			_isRunningTimer = false;
			startTimerButton.IsVisible = true;
			stopTimerButton.IsVisible = false;
		}

		// リセットボタンを押したら初期化
		private void OnResetTimerButtonClicked(object sender, EventArgs e)
		{
			stopCountdown();
			countdownTime = 0;
			pomodoroCounter = 0;
			InitializeComponent();
			clockDisplay(workTime);
			startTimerButton.Clicked += OnStartTimerButtonClicked;
			stopTimerButton.Clicked += OnStopTimerButtonClicked;
			resetTimerButton.Clicked += OnResetTimerButtonClicked;
		}
	}
}
