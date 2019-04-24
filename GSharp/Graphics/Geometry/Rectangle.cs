using GSharp.Graphics.OpenGL;
using OpenTK;
using GSharp.Util;

namespace GSharp.Graphics.Geometry {
	public class Rectangle : IRenderable {
		protected float X, Y, Width, Height;
		protected int[] Indices = new int[] { 0, 1, 2, 0, 2, 3 };
		protected float[] Vertices;
		protected bool Dirty = true;

		public Rectangle(float x, float y, float width, float height) {
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		protected virtual void UpdateData() {
			Vertices = new float[] {
				X, Y,
				X + Width, Y,
				X + Width,  Y + Height,
				X, Y + Height
			};

			Dirty = false;
		}

		public float GetX() {
			return X;
		}

		public float GetY() {
			return Y;
		}

		public float GetWidth() {
			return Width;
		}

		public float GetHeight() {
			return Height;
		}

		public void SetLocation(float x, float y) {
			X = x;
			Y = y;

			Dirty = true;
		}

		public void SetDimensions(float width, float height) {
			Width = width;
			Height = height;

			Dirty = true;
		}

		public virtual float[] GetVertexData() {
			UpdateData();

			return Vertices;
		}

		public virtual int[] GetIndexData() {
			return Indices;
		}

		public virtual VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord };
		}

		public bool IsDirty() {
			return Dirty;
		}
	}

	public class ColoredRectangle : Rectangle, IRenderable {
		Vector3 Color;

		public ColoredRectangle(float x, float y, float width, float height, Vector3 color) : base(x, y, width, height) {
			Color = color;
		}

		public ColoredRectangle(float x, float y, float width, float height) : this(x, y, width, height, new Vector3(255, 255, 255)) { }

		public Vector3 GetColor() {
			return Color;
		}

		public void SetColor(Vector3 color) {
			Color = color;

			Dirty = true;
		}

		protected override void UpdateData() {
			Vertices = new float[] {
				X, Y, Color.X, Color.Y, Color.Z,
				X + Width, Y, Color.X, Color.Y, Color.Z,
				X + Width, Y + Height, Color.X, Color.Y, Color.Z,
				X, Y + Height, Color.X, Color.Y, Color.Z
			};

			Dirty = false;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.Color };
		}
	}
}
