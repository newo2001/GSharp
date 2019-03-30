using GSharp.Graphics.OpenGL;
using System.Collections.Generic;

namespace GSharp.Graphics.UI {
	public class Menu {
		List<MenuItem> Items;

		public Menu() {
			Items = new List<MenuItem>();
		}

		public void AddItem(MenuItem item) {
			Items.Add(item);
		}

		public Mesh GetMesh() {
			int vertexCount = 0, indexCount = 0;
			foreach (MenuItem item in Items) {
				vertexCount += item.GetMesh().GetVertexCapacity();
				indexCount += item.GetMesh().GetIndexCapacity();
			}

			Mesh mesh = new Mesh(vertexCount, indexCount);
			foreach (MenuItem item in Items) {
				mesh.AddMesh(item.GetMesh());
			}
			return mesh;
		}
	}
}
