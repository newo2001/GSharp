using GSharp.Util;
using GSharp.Graphics.UI;
using System.Collections.Generic;

namespace GSharp.Graphics.OpenGL {
	public class Renderer {
		private Shader Shader;
		private bool UseElements;
		private VAO VAO;
		private Buffer<float> VBO;
		private Buffer<int> EBO;

		public Renderer(Shader shader, bool useElements, MeshAttribute[] attributes) {
			UseElements = useElements;
			Shader = shader;
			List<MeshAttribute> attribs = new List<MeshAttribute>(attributes);

			VBO = new Buffer<float>(1000, false);

			if (UseElements) {
				EBO = new Buffer<int>(1000, false);
			}

			VAO = new VAO();
			VAO.AddElement(DataType.Float, 2);
			
			if (attribs.Contains(MeshAttribute.TexCoords)) {
				VAO.AddElement(DataType.Float, 2);
			}

			if (UseElements) {
				VAO.Compile(EBO);
			} else {
				VAO.Compile();
			}
		}

		public Renderer(Shader shader, bool useElements) : this(shader, useElements, new MeshAttribute[] { }) { }

		public void Render() {
			Shader.Use();
			VAO.Bind();

			if (!VBO.IsReady()) {
				VBO.WriteToBuffer();
			}

			if (UseElements) {
				if (!EBO.IsReady()) {
					EBO.WriteToBuffer();
				}

				VAO.Render(VBO, EBO);
			} else {
				VAO.Render(VBO);
			}

			#if (DEBUG)
				bool empty = true;
				foreach (double d in Shader.GetUniform("projection")) {
					if (d != 0) {
						empty = false;
						break;
					}
				}
				if (empty) {
					Logger.Log("No projection matrix is set!", Severity.Warning);
				}
			#endif
		}

		public void AddMesh(Mesh mesh) {
			VBO.AddData(mesh.GetVertices());

			if (UseElements) {
				EBO.AddData(mesh.GetIndices());
			}
		}

		public void AddMeshes(Mesh[] meshes) {
			foreach (Mesh mesh in meshes) {
				AddMesh(mesh);
			}
		}

		public void AddMenu(Menu menu) {
			AddMesh(menu.GetMesh());
		}

		public Shader GetShader() {
			return Shader;
		}
	}
}
