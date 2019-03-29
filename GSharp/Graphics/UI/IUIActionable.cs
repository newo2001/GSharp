using System;

namespace GSharp.Graphics.UI {
	public interface IUIActionable {
		void PerformAction();
		void SetAction(Action action);
	}
}
