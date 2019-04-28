using GSharp.Graphics.OpenGL;
using OpenTK;
using System;

namespace GSharp.Graphics.Geometry {
	public abstract class Polygon : ITransformable, IRenderable {
		public Vector2 Origin;
		protected Matrix3 Transform;
		protected bool Dirty = true;

		public Polygon() {
			Origin = GetCenter();
			ClearTransform();
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
				angle %= 2 * (float)Math.PI;
			}

			if (angle < 0) {
				angle = 2 * (float)Math.PI + angle;
			}

			Translate(-Origin);
			Transform *= new Matrix3(
				(float)Math.Cos(angle), (float)Math.Sin(angle), 0,
				(float)-Math.Sin(angle), (float)Math.Cos(angle), 0,
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

		public void ClearTransform() {
			Transform = Matrix3.Identity;
		}

		public bool IsDirty() {
			return Dirty;
		}

		public virtual VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord };
		}

		public abstract void ApplyTransform();
		public abstract Vector2 GetCenter();
		public abstract float[] GetVertexData();
		public abstract int[] GetIndexData();
	}
}
