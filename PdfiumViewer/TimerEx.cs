using System.Threading;

namespace PdfiumViewer
{
	internal class TimerEx
	{
		private TimerCallback _lpTimerFunc;
		private int _interval;
		private Timer _timer;

		public int TimerId { get; private set; }
		private SynchronizationContext _synchronizationContext { get; set; }


		public TimerEx(SynchronizationContext synchronizationContext, int interval, int timerid, TimerCallback lpTimerFunc)
		{
			this._synchronizationContext = synchronizationContext;
			this._interval = interval;
			this.TimerId = timerid;
			this._lpTimerFunc = lpTimerFunc;
			this._timer = new Timer(new System.Threading.TimerCallback(this.TimerCallback), this, Timeout.Infinite, interval);
		}

		internal void Start()
		{
			this._timer.Change(this._interval, this._interval);
		}

		internal void Stop()
		{
			this._timer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		private void TimerCallback(object stateInfo)
		{
			TimerEx tm = stateInfo as TimerEx;
			if (this._synchronizationContext == null)
			{
				tm._lpTimerFunc(tm.TimerId);
				return;
			}
			this._synchronizationContext.Send(delegate (object o)
			{
				tm._lpTimerFunc(tm.TimerId);
			}, tm);
		}
	}
}