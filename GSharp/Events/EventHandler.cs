using GSharp.Events;
using GSharp.Graphics;
using OpenTK.Input;
using System;

namespace GSharp.Events {
	public static class EventHandler {
		public static void RegisterEvents(Window window) {
			KeyUpEvent.Subscribe(delegate(KeyUpEvent e) {
				if (e.GetKey() == Key.Escape) {
					window.Exit();
				}
			});
		}
	}
}
