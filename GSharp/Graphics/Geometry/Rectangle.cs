using GSharp.Graphics.OpenGL;
using OpenTK;
using System;
using GSharp.Util;

namespace GSharp.Graphics.Geometry {
	public class Rectangle : Polygon {
		public Vector2 TopLeft, TopRight, BottomRight, BottomLeft;
		private int[] Indices = new int[] { 0, 1, 2, 0, 2, 3 };
		protected float[] Vertices;

		public Rectangle(float x, float y, float width, float height) : base() {
			TopLeft = new Vector2(x, y);
			TopRight = new Vector2(x + width, y);
			BottomRight = new Vector2(x + width, y + height);
			BottomLeft = new Vector2(x, y + height);
		}

		public override Vector2 GetCenter() {
			return (TopLeft + TopRight + BottomRight + BottomLeft) / 4;
		}

		public override void ApplyTransform() {
			TopLeft = Transform.Multiply(TopLeft);
			TopRight = Transform.Multiply(TopRight);
			BottomRight = Transform.Multiply(BottomRight);
			BottomLeft = Transform.Multiply(BottomLeft);
			ClearTransform();
		}

		protected virtual void UpdateData() {
			ApplyTransform();

			Vertices = new float[] {
				TopLeft.X, TopLeft.Y,
				TopRight.X, TopRight.Y,
				BottomRight.X, BottomRight.Y,
				BottomLeft.X, BottomLeft.Y
			};

			Dirty = false;
		}

		public float GetWidth() {
			return TopLeft.Distance(TopRight);
		}

		public float GetHeight() {
			return TopRight.Distance(BottomRight);
		}

		public override float[] GetVertexData() {
			if (Dirty) {
				UpdateData();
			}

			return Vertices;
		}

		public override int[] GetIndexData() {
			return Indices;
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
			ApplyTransform();

			Vertices = new float[] {
				TopLeft.X, TopLeft.Y, Color.X, Color.Y, Color.Z,
				TopRight.X, TopRight.Y, Color.X, Color.Y, Color.Z,
				BottomRight.X, BottomRight.Y, Color.X, Color.Y, Color.Z,
				BottomLeft.X, BottomLeft.Y, Color.X, Color.Y, Color.Z
			};

			Dirty = false;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.Color };
		}
	}
}
