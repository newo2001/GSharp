namespace GSharp.Events {
	public class Event {
		private bool Canceled = false;

		public bool IsCanceled() {
			return Canceled;
		}

		public void Cancel() {
			Canceled = true;
		}
	}
}
