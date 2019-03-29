namespace HelloWorld.Graphics {
	class Mesh {
		private Vertex[] Vertices;
		private int[] Indices;
		private int VerticesIndex = 0;
		private int IndicesIndex = 0;

		public Mesh(int vertices, int indices = 0) {
			Vertices = new Vertex[vertices];
			Indices = new int[indices];
		}

		public void AddVertex(Vertex vertex) {
			Vertices[VerticesIndex] = vertex;
			VerticesIndex++;
		}

		public void AddVertices(Vertex[] vertices) {
			vertices.CopyTo(Vertices, VerticesIndex);
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
			return Vertices.Length;
		}

		public int GetIndexCount() {
			return IndicesIndex;
		}

		public int GetIndexCapacity() {
			return Indices.Length;
		}

		public Vertex[] GetVertices() {
			return Vertices;
		}

		public Vertex GetVertex(int index) {
			return Vertices[index];
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
