using OpenTK;
using System;

namespace GSharp.Util {
	public static class VectorUtils {
		public static float Distance(this Vector2 vec, Vector2 vec2) {
			return (float) Math.Abs(Math.Sqrt(Math.Pow(vec.X - vec2.X, 2) + Math.Pow(vec.Y - vec2.Y, 2)));
		}

		public static Vector3 Multiply(this Matrix3 mat, Vector3 vec) {
			return new Vector3(
				vec.X * mat.M11 + vec.Y * mat.M21 + vec.Z * mat.M31,
				vec.X * mat.M12 + vec.Y * mat.M22 + vec.Z * mat.M32,
				vec.X * mat.M13 + vec.Y * mat.M23 + vec.Z * mat.M33
			);
		}

		public static Vector2 Multiply(this Matrix3 mat, Vector2 vec) {
			Vector3 vector = mat.Multiply(new Vector3(vec.X, vec.Y, 1));
			return new Vector2(vector.X, vector.Y);
		}
	}
}
