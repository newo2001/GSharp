using GSharp.Graphics.OpenGL;

namespace GSharp.Graphics {
	public static class Geometry {
		public static Mesh Rectangle(float x, float y, float width, float height) {
			Mesh mesh = new Mesh(4, 6);
			mesh.AddIndices(new int[] { 0, 1, 2, 0, 2, 3 });
			mesh.AddVertices(new float[] {
				x, y,
				x + width, y,
				x + width, y + height,
				x, y + height
			});
			return mesh;
		}

		public static Mesh Triangle(float x1, float y1, float x2, float y2, float x3, float y3) {
			Mesh mesh = new Mesh(3, 3);
			mesh.AddIndices(new int[] { 0, 1, 2 });
			mesh.AddVertices(new float[] {
				x1, y1,
				x2, y2,
				x3, y3
			});
			return mesh;
		}
	}
}
