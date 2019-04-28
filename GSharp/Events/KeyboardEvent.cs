using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace GSharp.Events {
	public class KeyEvent : IEvent {
		protected Key Key;
		protected bool Canceled = false;

		protected KeyEvent(Key key) {
			Key = key;
		}

		public void Cancel() {
			Canceled = true;
		}

		public Key GetKey() {
			return Key;
		}

		public bool IsCanceled() {
			return Canceled;
		}
	}

	public class KeyRepeatEvent : KeyEvent {
		private static List<Action<KeyRepeatEvent>> Handlers = new List<Action<KeyRepeatEvent>>();

		private KeyRepeatEvent(Key key) : base(key) { }

		public static void Subscribe(Action<KeyRepeatEvent> handler) {
			Handlers.Add(handler);
		}

		public static void Call(Key key) {
			KeyRepeatEvent e = new KeyRepeatEvent(key);
			foreach (Action<KeyRepeatEvent> handler in Handlers) {
				handler.Invoke(e);
				if (e.IsCanceled()) {
					break;
				}
			}
		}
	}

	public class KeyUpEvent : KeyEvent {
		private static List<Action<KeyUpEvent>> Handlers = new List<Action<KeyUpEvent>>();

		private KeyUpEvent(Key key) : base(key) { }

		public static void Subscribe(Action<KeyUpEvent> handler) {
			Handlers.Add(handler);
		}

		public static void Call(Key key) {
			KeyUpEvent e = new KeyUpEvent(key);
			foreach (Action<KeyUpEvent> handler in Handlers) {
				handler.Invoke(e);
				if (e.IsCanceled()) {
					break;
				}
			}
		}
	}

	public class KeyDownEvent : KeyEvent {
		private static List<Action<KeyDownEvent>> Handlers = new List<Action<KeyDownEvent>>();

		private KeyDownEvent(Key key) : base(key) { }

		public static void Subscribe(Action<KeyDownEvent> handler) {
			Handlers.Add(handler);
		}

		public static void Call(Key key) {
			KeyDownEvent e = new KeyDownEvent(key);
			foreach (Action<KeyDownEvent> handler in Handlers) {
				handler.Invoke(e);
				if (e.IsCanceled()) {
					break;
				}
			}
		}
	}
}
