using GSharp.Graphics.OpenGL;
using GSharp.Util;
using OpenTK;
using System;

namespace GSharp.Graphics.Geometry {
	public class Circle : Polygon {
		protected float X, Y, Radius;

		protected Vector2[] Points;
		protected float[] Vertices;
		private int[] Indices;

		public Circle(float x, float y, float radius, int detail = 32) : base() {
			X = x;
			Y = y;
			Radius = radius;

			Points = new Vector2[detail];
			Indices = new int[detail * 3];

			for (int i = 0; i < detail; i++) {
				double angle = 2 * Math.PI / detail * i;
				Points[i] = new Vector2(X + Radius * (float)Math.Cos(angle), Y + Radius * (float)Math.Sin(angle));
			}

			for (int i = 0; i < detail - 1; i++) {
				Indices[i * 3] = i;
				Indices[i * 3 + 2] = Points.Length;
				Indices[i * 3 + 1] = i + 1;
			}
			Indices[(detail - 1) * 3] = detail - 1;
			Indices[(detail - 1) * 3 + 1] = 0;
			Indices[(detail - 1) * 3 + 2] = Points.Length;
		}

		public Circle(Vector2 center, float radius, int detail = 32) : this(center.X, center.Y, radius, detail) { }

		public override void ApplyTransform() {
			foreach (Vector2 point in Points) {
				Transform.Multiply(point);
			}
			ClearTransform();
		}
		
		protected virtual void UpdateData() {
			Vertices = new float[Points.Length * 2];

			for (int i = 0; i < Points.Length; i++) {
				Vertices[i * 2] = Points[i].X;
				Vertices[i * 2] = Points[i].Y;
			}

			Dirty = false;
		}

		public override Vector2 GetCenter() {
			return new Vector2(X, Y);
		}

		public override int[] GetIndexData() {
			return Indices;
		}

		public override float[] GetVertexData() {
			if (Dirty) {
				UpdateData();
			}

			return Vertices;
		}

		public float GetX() {
			return X;
		}

		public float GetY() {
			return Y;
		}

		public float GetRadius() {
			return Radius;
		}
	}

	public class ColoredCircle : Circle {
		private Vector3 Color;

		public ColoredCircle(float x, float y, float radius, Vector3 color, int detail = 32) : base(x, y, radius, detail) {
			Color = color;
		}

		public ColoredCircle(float x, float y, float radius, int detail = 32) : this(x, y, radius, new Vector3(255, 255, 255), detail) { }

		public void SetColor(Vector3 color) {
			Color = color;
		}

		public Vector3 GetColor() {
			return Color;
		}

		protected override void UpdateData() {
			Vertices = new float[(Points.Length + 1) * 5];
			Vertices[Points.Length * 5] = X;
			Vertices[Points.Length * 5 + 1] = Y;
			Vertices[Points.Length * 5 + 2] = Color.X;
			Vertices[Points.Length * 5 + 3] = Color.Y;
			Vertices[Points.Length * 5 + 4] = Color.Z;


			Vertices[Points.Length * 5 + 1] = Y;
			for (int i = 0; i < Points.Length; i++) {
				Vertices[i * 5] = Points[i].X;
				Vertices[i * 5 + 1] = Points[i].Y;
				Vertices[i * 5 + 2] = Color.X;
				Vertices[i * 5 + 3] = Color.Y;
				Vertices[i * 5 + 4] = Color.Z;
			}
			

			Dirty = false;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.Color };
		}
	}
}
