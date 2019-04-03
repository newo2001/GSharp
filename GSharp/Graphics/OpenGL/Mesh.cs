using System.Collections.Generic;
using System;

namespace GSharp.Graphics.OpenGL {
	public enum MeshAttribute {
		TexCoords
	}

	public static class EnumMeshAttribute {
		public static int GetSize(this MeshAttribute attribute) {
			switch(attribute) {
				case MeshAttribute.TexCoords:
					return 2;
				default:
					return -1;
			}
		}
	}

	public class Mesh {
		private float[] Vertices;
		private int[] Indices;
		private int VerticesIndex = 0;
		private int IndicesIndex = 0;
		private int VertexSize;
		private List<MeshAttribute> Attributes;

		public Mesh(int vertices, int indices = 0) {
			Vertices = new float[vertices * 2];
			Indices = new int[indices];
			Attributes = new List<MeshAttribute>();
			VertexSize = 2;
		}

		public Mesh(MeshAttribute[] attributes, int vertices, int indices = 0) {
			Indices = new int[indices];

			VertexSize = 2;
			foreach (MeshAttribute attribute in attributes) {
				VertexSize += attribute.GetSize();
			}
			Vertices = new float[VertexSize * vertices];
			Attributes = new List<MeshAttribute>(attributes);
		}

		public bool HasAttribute(MeshAttribute attribute) {
			return Attributes.Contains(attribute);
		}

		public void AddMesh(Mesh mesh) {
			mesh.Vertices.CopyTo(Vertices, VerticesIndex);
			VerticesIndex += mesh.GetVertexCapacity();
			mesh.Indices.CopyTo(Vertices, VerticesIndex);
			IndicesIndex += mesh.GetIndexCapacity();
		}

		public Mesh AddVertex(float x, float y) {
			Vertices[VerticesIndex * VertexSize] = x;
			Vertices[VerticesIndex * VertexSize + 1] = y;
			VerticesIndex++;
			return this;
		}

		public Mesh AddVertex(float x, float y, float texX, float texY) {
			if (!HasAttribute(MeshAttribute.TexCoords)) {
				throw new System.Exception("Texture coordinates are not enabled for this mesh");
			}

			Vertices[VerticesIndex * VertexSize] = x;
			Vertices[VerticesIndex * VertexSize + 1] = y;
			Vertices[VerticesIndex * VertexSize + 2] = texX;
			Vertices[VerticesIndex * VertexSize + 3] = texY;
			VerticesIndex++;
			return this;
		}

		public Mesh AddVertices(float[] vertices) {
			vertices.CopyTo(Vertices, VerticesIndex * VertexSize);
			VerticesIndex += vertices.Length / VertexSize;
			return this;
		}

		public Mesh AddRelativeIndex(int index) {
			Indices[IndicesIndex] = index + VerticesIndex;
			IndicesIndex++;
			return this;
		}

		public Mesh AddRelativeIndices(int[] indices) {
			for (int i = 0; i < indices.Length; i++) {
				indices[i] += VerticesIndex;
			}

			indices.CopyTo(Indices, IndicesIndex);
			IndicesIndex += indices.Length;
			return this;
		}

		public Mesh AddIndex(int index) {
			Indices[IndicesIndex] = index;
			IndicesIndex++;
			return this;
		}

		public Mesh AddIndices(int[] indices) {
			indices.CopyTo(Indices, IndicesIndex);
			IndicesIndex += indices.Length;
			return this;
		}

		public int GetVertexCount() {
			return VerticesIndex;
		}

		public int GetVertexCapacity() {
			return Vertices.Length / VertexSize;
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

		public int[] GetIndices() {
			return Indices;
		}

		public int GetIndex(int index) {
			return Indices[index];
		}

		public int GetVertexSize() {
			return VertexSize;
		}
	}
}
