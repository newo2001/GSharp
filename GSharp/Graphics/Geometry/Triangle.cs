using GSharp.Graphics.OpenGL;
using GSharp.Util;
using OpenTK;

namespace GSharp.Graphics.Geometry {
	public class Triangle : Polygon {
		public Vector2 Point1, Point2, Point3;
		private int[] Indices = new int[] { 0, 1, 2 };
		protected float[] Vertices;

		public Triangle(Vector2 p1, Vector2 p2, Vector2 p3) {
			Point1 = p1;
			Point2 = p2;
			Point3 = p3;
		}

		public Triangle(float x1, float y1, float x2, float y2, float x3, float y3) {
			Point1 = new Vector2(x1, y1);
			Point2 = new Vector2(x2, y2);
			Point3 = new Vector2(x3, y3);
		}

		public override void ApplyTransform() {
			Point1 = Transform.Multiply(Point1);
			Point2 = Transform.Multiply(Point2);
			Point3 = Transform.Multiply(Point3);
			ClearTransform();
		}

		public override Vector2 GetCenter() {
			return (Point1 + Point2 + Point3) / 3;
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

		protected virtual void UpdateData() {
			ApplyTransform();

			Vertices = new float[] {
				Point1.X, Point1.Y,
				Point2.X, Point2.Y,
				Point3.X, Point3.Y
			};

			Dirty = false;
		}
	}

	public class ColoredTriangle : Triangle {
		private Vector3 Color;

		public ColoredTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector3 color) : base(p1, p2, p3) {
			Color = color;
		}

		public ColoredTriangle(float x1, float y1, float x2, float y2, float x3, float y3) : this(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), new Vector3(255, 255, 255)) { }
		public ColoredTriangle(float x1, float y1, float x2, float y2, float x3, float y3, Vector3 color) : this(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3), color) {}
		public ColoredTriangle(Vector2 p1, Vector2 p2, Vector2 p3) : this(p1, p2, p3, new Vector3(255, 255, 255)) { }

		public Vector3 GetColor() {
			return Color;
		}

		public void SetColor(Vector3 color) {
			Color = color;
			Dirty = true;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.Color };
		}

		protected override void UpdateData() {
			ApplyTransform();

			Vertices = new float[] {
				Point1.X, Point1.Y, Color.X, Color.Y, Color.Z,
				Point2.X, Point2.Y, Color.X, Color.Y, Color.Z,
				Point3.X, Point3.Y, Color.X, Color.Y, Color.Z
			};

			Dirty = false;
		}
	}
}
