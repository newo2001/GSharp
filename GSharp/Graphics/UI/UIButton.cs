using System;
using GSharp.Graphics.OpenGL;
using GSharp.Graphics;

namespace GSharp.Graphics.UI {
	public class UIButton : MenuItem, IUIActionable {
		private Action Action;
		private float Width, Height;

		public UIButton(string name, float x, float y, float width, float height) : base(name, x, y) {
			Width = width;
			Height = height;
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

		public void PerformAction() {
			Action.Invoke();
		}

		public void SetAction(Action action) {
			Action = action;
		}
	}
}
