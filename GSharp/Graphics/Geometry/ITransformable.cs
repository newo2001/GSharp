using OpenTK;

namespace GSharp.Graphics.Geometry {
	public interface ITransformable {
		void Scale(float x, float y);
		void Scale(float factor);
		void Scale(Vector2 scale);
		void Rotate(float Angle);
		void Translate(float x, float y);
		void Translate(Vector2 location);
		void ApplyTransform();
	}
}
