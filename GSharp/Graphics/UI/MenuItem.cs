using GSharp.Graphics.OpenGL;

namespace GSharp.Graphics.UI {
	public abstract class MenuItem {
		protected float X, Y;
		protected Mesh Mesh;
		protected bool Dirty = true;

		public MenuItem(float x, float y) {
			X = x;
			Y = y;
		}

		public void SetX(float x) {
			X = x;
			Dirty = true;
		}

		public void SetY(float y) {
			Y = y;
			Dirty = true;
		}

		public float GetX() {
			return X;
		}

		public float GetY() {
			return Y;
		}

		public abstract Mesh GetMesh();
	}
}
