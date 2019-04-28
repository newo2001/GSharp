using System;
using System.Collections.Generic;
using GSharp.Util;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace GSharp.Graphics.OpenGL {
	public class RenderBatch {
		private static Matrix4 ProjectionMatrix;

		private List<Texture> Textures;
		private TextureAtlas Atlas;
		private Shader Shader;
		private VAO VAO;

		private List<IRenderable> Renderables;
		private List<Buffer<float>> VBOs;
		private List<Buffer<int>> EBOs;
		private VertexComponent[] Format;
		private int MaxVertices, MaxIndices;
		private int Vertices = 0, Indices = 0;
		private bool Dirty = true;

		public RenderBatch(VertexComponent[] format, Shader shader, bool dynamic, int maxVertices = 3200000, int maxIndices = 320000) {
			Shader = shader;
			Format = format;
			MaxVertices = maxVertices;
			MaxIndices = maxIndices;

			VBOs = new List<Buffer<float>> {
				new Buffer<float>(maxVertices, dynamic)
			};
			EBOs = new List<Buffer<int>> {
				new Buffer<int>(maxIndices, dynamic)
			};
			Renderables = new List<IRenderable>();
			Textures = new List<Texture>();

			VAO = new VAO();
			foreach (VertexComponent component in format) {
				VAO.AddElement(Util.DataType.Float, component.Size());
			}
			VAO.Compile(VBOs[0]);
		}

		public static void SetProjectionMatrix(Matrix4 projection) {
			ProjectionMatrix = projection;
		}

		public void Add(IRenderable renderable) {
			#if (DEBUG)
				if (!renderable.GetVertexFormat().CompareEquals(Format)) {
					Logger.Log("Renderable was added to a batch with a different vertex format!", Severity.Warning);
				}
			#endif

			Renderables.Add(renderable);
			Dirty = true;

			if (renderable is AtlasSprite atlasSprite && Atlas != null) {
				atlasSprite.SetAtlas(Atlas);
			}

			if (renderable is Sprite sprite) {
				Textures.Add(sprite.GetTexture());
			}
		}

		public void BuildMesh() {
			Logger.Log("Building mesh", Severity.Info);

			ClearBuffers();

			foreach (IRenderable renderable in Renderables) {
				int buffer = VBOs.Count - 1;
				int[] indices = renderable.GetIndexData();
				float[] vertices = renderable.GetVertexData();

				if (Vertices + VBOs[buffer].GetSize() > MaxVertices || Indices + EBOs[buffer].GetSize() > MaxIndices) {
					NextBuffer();
					buffer++;
				}

				VBOs[buffer].AddData(vertices);
				EBOs[buffer].AddData(indices);
				Vertices += vertices.Length;
				Indices += indices.Length;
			}

			WriteToBuffers();

			Dirty = false;
		}

		public void Render() {
			#if (DEBUG)
				if (ProjectionMatrix == null) {
					Logger.Log("No projection matrix is set!", Severity.Warning);
				}
			#endif

			Shader.Use();
			Shader.SetProjectionMatrix(ProjectionMatrix);
			VAO.Bind();
			
			if (IsDirty()) {
				BuildMesh();
			}

			for (int i = 0; i < Textures.Count; i++) {
				if (Textures[i] != null) {
					Textures[i].Bind(i);
				}
			}

			if (Atlas != null) {
				Shader.SetUniform("atlasSize", Atlas.GetWidth(), Atlas.GetHeight());
			}

			for (int i = 0; i < Math.Min(VBOs.Count, EBOs.Count); i++) {
				if (VBOs[i].GetSize() == 0 || EBOs[i].GetSize() == 0) {
					return;
				}

				VAO.Clear();

				GL.DrawElements(PrimitiveType.Triangles, EBOs[i].GetSize(), DrawElementsType.UnsignedInt, 0);
			}
		}

		private void WriteToBuffers() {
			foreach (Buffer<float> buffer in VBOs) {
				buffer.WriteToBuffer();
			}
			foreach (Buffer<int> buffer in EBOs) {
				buffer.WriteToBuffer();
			}
		}

		public void NextBuffer() {
			VBOs[VBOs.Count - 1].WriteToBuffer();
			EBOs[EBOs.Count - 1].WriteToBuffer();
			VBOs.Add(new Buffer<float>(MaxVertices, true));
			EBOs.Add(new Buffer<int>(MaxIndices, true));
			Vertices = 0;
			Indices = 0;

			VAO.Compile(VBOs[VBOs.Count - 1]);
		}

		public void Clear() {
			ClearBuffers();
			Renderables.Clear();
		}

		public void ClearBuffers() {
			foreach (Buffer<float> buffer in VBOs) {
				buffer.ClearData();
			}
			foreach (Buffer<int> buffer in EBOs) {
				buffer.ClearData();
			}

			Vertices = 0;
			Indices = 0;
		}

		public void DeleteBuffers() {
			foreach (Buffer<float> buffer in VBOs) {
				buffer.Remove();
			}
			foreach (Buffer<int> buffer in EBOs) {
				buffer.Remove();
			}

			Renderables.Clear();
			ClearBuffers();
			VBOs.Clear();
			EBOs.Clear();
		}

		public bool IsDirty() {
			if (Dirty) {
				return true;
			}

			foreach (IRenderable renderable in Renderables) {
				if (renderable.IsDirty()) {
					return true;
				}
			}

			return false;
		}

		public void SetShader(Shader shader) {
			Shader = shader;
		}

		public void SetAtlas(TextureAtlas atlas) {
			Atlas = atlas;

			foreach (IRenderable renderable in Renderables) {
				if (renderable is AtlasSprite sprite) {
					sprite.SetAtlas(Atlas);
				}
			}
		}

		public void ClearTextures() {
			Textures.Clear();
		}
	}
}
