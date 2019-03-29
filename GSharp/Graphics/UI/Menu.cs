using GSharp.Graphics.OpenGL;
using System.Collections.Generic;

namespace GSharp.Graphics.UI {
	public class Menu {
		Dictionary<string, MenuItem> Items;

		public Menu() {
			Items = new Dictionary<string, MenuItem>();
		}

		public void AddItem(MenuItem item) {
			Items.Add(item.GetName(), item);
		}

		public Mesh GetMesh() {
			int vertexCount = 0, indexCount = 0;
			foreach (MenuItem item in Items.Values) {
				vertexCount += item.GetMesh().GetVertexCapacity();
				indexCount += item.GetMesh().GetIndexCapacity();
			}

			Mesh mesh = new Mesh(vertexCount, indexCount);
			foreach (MenuItem item in Items.Values) {
				mesh.AddMesh(item.GetMesh());
			}
			return mesh;
		}
	}
}
