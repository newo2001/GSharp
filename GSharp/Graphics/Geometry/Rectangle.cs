using GSharp.Graphics.OpenGL;
using OpenTK;
using System;
using GSharp.Util;

namespace GSharp.Graphics.Geometry {
	public class Rectangle : IRenderable, ITransformable {
		public Vector2 TopLeft;
		public Vector2 TopRight;
		public Vector2 BottomRight;
		public Vector2 BottomLeft;
		public Vector2 Origin;

		protected int[] Indices = new int[] { 0, 1, 2, 0, 2, 3 };
		protected float[] Vertices;
		protected bool Dirty = true;
		protected Matrix3 Transform;

		public Rectangle(float x, float y, float width, float height) {
			TopLeft = new Vector2(x, y);
			TopRight = new Vector2(x + width, y);
			BottomRight = new Vector2(x + width, y + height);
			BottomLeft = new Vector2(x, y + height);

			Transform = Matrix3.Identity;
			Origin = GetCenter();
		}

		public Vector2 GetCenter() {
			return (TopLeft + TopRight + BottomRight + BottomLeft) / 4;
		}

		public void ApplyTransform() {
			TopLeft = Transform.Multiply(TopLeft);
			TopRight = Transform.Multiply(TopRight);
			BottomRight = Transform.Multiply(BottomRight);
			BottomLeft = Transform.Multiply(BottomLeft);
			Transform = Matrix3.Identity;
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

		public void Scale(float x, float y) {
			Transform *= new Matrix3(
				x, 0, 0,
				0, y, 0,
				0, 0, 1
			);

			Dirty = true;
		}

		public void Scale(float factor) {
			Scale(factor, factor);
		}

		public void Scale(Vector2 scale) {
			Scale(scale.X, scale.Y);
		}

		public void Rotate(float angle) {
			if (angle > 2 * Math.PI) {
				angle %= 2 * (float) Math.PI;
			}

			if (angle < 0) {
				angle = 2 * (float) Math.PI + angle;
			}

			Translate(-Origin);
			Transform *= new Matrix3(
				(float) Math.Cos(angle), (float) Math.Sin(angle), 0,
				(float) -Math.Sin(angle), (float) Math.Cos(angle), 0,
				0, 0, 1
			);
			Translate(Origin);

			Dirty = true;
		}

		public void Translate(float x, float y) {
			Transform *= new Matrix3(
				1, 0, 0,
				0, 1, 0,
				x, y, 1
			);
			Dirty = true;
		}

		public void Translate(Vector2 location) {
			Translate(location.X, location.Y);
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
