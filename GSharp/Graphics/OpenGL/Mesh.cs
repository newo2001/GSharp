namespace GSharp.Graphics.OpenGL {
	public class Mesh {
		private float[] Vertices;
		private int[] Indices;
		private int VerticesIndex = 0;
		private int IndicesIndex = 0;

		public Mesh(int vertices, int indices = 0) {
			Vertices = new float[vertices * 2];
			Indices = new int[indices];
		}

		public void AddMesh(Mesh mesh) {
			mesh.Vertices.CopyTo(Vertices, VerticesIndex);
			mesh.Indices.CopyTo(Indices, IndicesIndex);
			VerticesIndex += mesh.GetVertexCapacity();
			IndicesIndex += mesh.GetIndexCapacity();
		}

		public void AddVertex(float x, float y) {
			Vertices[VerticesIndex * 2] = x;
			Vertices[VerticesIndex * 2 + 1] = y;
			VerticesIndex++;
		}

		public void AddVertices(float[] vertices) {
			vertices.CopyTo(Vertices, VerticesIndex * 2);
			VerticesIndex += vertices.Length / 2;
		}

		public void AddVertex(Vertex vertex) {
			AddVertex(vertex.X, vertex.Y);
		}

		public void AddVertices(Vertex[] vertices) {
			vertices.CopyTo(Vertices, VerticesIndex * 2);
			VerticesIndex += vertices.Length;
		}

		public void AddRelativeIndex(int index) {
			Indices[IndicesIndex] = index + VerticesIndex;
			IndicesIndex++;
		}

		public void AddRelativeIndices(int[] indices) {
			for (int i = 0; i < indices.Length; i++) {
				indices[i] += VerticesIndex;
			}

			indices.CopyTo(Indices, IndicesIndex);
			IndicesIndex += indices.Length;
		}

		public void AddIndex(int index) {
			Indices[IndicesIndex] = index;
			IndicesIndex++;
		}

		public void AddIndices(int[] indices) {
			indices.CopyTo(Indices, IndicesIndex);
			IndicesIndex += indices.Length;
		}

		public int GetVertexCount() {
			return VerticesIndex;
		}

		public int GetVertexCapacity() {
			return Vertices.Length / 2;
		}

		public int GetIndexCount() {
			return IndicesIndex;
		}

		public int GetIndexCapacity() {
			return Indices.Length;
		}

		public float[] GetVertices() {
			return Vertices;
		}

		public Vertex GetVertex(int index) {
			return new Vertex(Vertices[index / 2], Vertices[index / 2 + 1]);
		}

		public int[] GetIndices() {
			return Indices;
		}

		public int GetIndex(int index) {
			return Indices[index];
		}
	}

	public struct Vertex {
		public float X, Y;

		public Vertex(float x, float y) {
			X = x;
			Y = y;
		}
	}
}
