using System;
using GSharp.Graphics;

namespace GSharp {
	static class Program {
		static Window Window;

		[STAThread]
		static void Main() {
			Window = new Window(1280, 720);
			Window.Run(60);
		}
	}
}
