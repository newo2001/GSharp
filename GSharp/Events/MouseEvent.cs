using System;
using System.Collections.Generic;
using OpenTK.Input;

namespace GSharp.Events {
	public class MouseEvent : IEvent {
		protected float X, Y;
		protected MouseButton Button;
		protected bool Canceled = false;

		protected MouseEvent(MouseButton button, float x, float y) {
			X = x;
			Y = y;
			Button = button;
		}

		public void Cancel() {
			Canceled = true;
		}

		public MouseButton GetButton() {
			return Button;
		}

		public float GetX() {
			return X;
		}

		public float GetY() {
			return Y;
		}

		public bool IsCanceled() {
			return Canceled;
		}
	}
	

	public class MousePressEvent : MouseEvent {
		private static List<Action<MousePressEvent>> Handlers = new List<Action<MousePressEvent>>();

		private MousePressEvent(MouseButton button, float x, float y) : base(button, x, y) { }

		public static void Subscribe(Action<MousePressEvent> handler) {
			Handlers.Add(handler);
		}

		public static void Call(MouseButton button, float x, float y) {
			MousePressEvent e = new MousePressEvent(button, x, y);
			foreach (Action<MousePressEvent> handler in Handlers) {
				handler.Invoke(e);
				if (e.IsCanceled()) {
					break;
				}
			}
		}
	}

	public class MouseReleaseEvent : MouseEvent { 
		private static List<Action<MouseReleaseEvent>> Handlers = new List<Action<MouseReleaseEvent>>();

		private MouseReleaseEvent(MouseButton button, float x, float y) : base(button, x, y) { }

		public static void Subscribe(Action<MouseReleaseEvent> handler) {
			Handlers.Add(handler);
		}

		public static void Call(MouseButton button, float x, float y) {
			MouseReleaseEvent e = new MouseReleaseEvent(button, x, y);
			foreach (Action<MouseReleaseEvent> handler in Handlers) {
				handler.Invoke(e);
				if (e.IsCanceled()) {
					break;
				}
			}
		}
	}
}
