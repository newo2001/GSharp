using OpenTK.Graphics.OpenGL;
using System;

namespace HelloWorld.Graphics {
	class Renderer {
		private Shader Shader;
		private bool UseElements;
		private VAO VAO;
		private VBO VBO;
		private EBO EBO;

		public Renderer(Shader shader, bool useElements = true) {
			UseElements = useElements;
			Shader = shader;

			VBO = new VBO(1000, false);

			if (UseElements) {
				EBO = new EBO(VBO.GetCapacity() / 2, false);
			}

			VAO = new VAO();
			VAO.AddElement(DataType.Float, 2);

			if (UseElements) {
				VAO.Compile(EBO);
			} else {
				VAO.Compile();
			}
		}

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
		}

		public void AddMesh(Mesh mesh) {
			VBO.AddVertices(mesh.GetVertices());

			if (UseElements) {
				EBO.AddData(mesh.GetIndices());
			}
		}

		public void AddMeshes(Mesh[] meshes) {
			foreach (Mesh mesh in meshes) {
				AddMesh(mesh);
			}
		}

		public Shader GetShader() {
			return Shader;
		}
	}
}
