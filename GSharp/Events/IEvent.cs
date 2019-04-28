namespace GSharp.Events {
	public interface IEvent {
		bool IsCanceled();
		void Cancel();
	}
}
