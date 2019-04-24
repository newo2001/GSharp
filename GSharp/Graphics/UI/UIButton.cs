/*using System;
using GSharp.Graphics.OpenGL;
using GSharp.Events;
using OpenTK.Input;

namespace GSharp.Graphics.UI {
	public class UIButton : MenuItem {
		private Action Action;
		private float Width, Height;

		public UIButton(float x, float y, float width, float height) : base(x, y) {
			Width = width;
			Height = height;

			MousePressEvent.Subscribe(delegate(MousePressEvent e) {
				if (e.GetButton() == MouseButton.Left) {
					if (e.GetX() >= X && e.GetX() < X + Width && e.GetY() >= Y && e.GetY() < Y + Height) {
						Action.Invoke();
					}
				}
			});
		}

		public void SetWidth(float width) {
			Width = width;
			Dirty = true;
		}

		public float GetWidth() {
			return Width;
		}

		public float GetHeight() {
			return Height;
		}

		public void SetHeight(float height) {
			Height = height;
			Dirty = true;
		}

		public override Mesh GetMesh() {
			if (Dirty) {
				Mesh = Geometry.Rectangle(X, Y, Width, Height);
				Dirty = false;
			}

			return Mesh;
		}

		public UIButton SetAction(Action action) {
			Action = action;
			return this;
		}
	}
}
*/