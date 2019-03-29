using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System;

namespace HelloWorld.Graphics {
	public class VAO {
		private int Handle;
		private List<VertexElement> Elements;

		public VAO() {
			Handle = GL.GenVertexArray();
			Elements = new List<VertexElement>();
			Bind();
		}

		public void Render(VBO VBO, EBO EBO = null) {
			Bind();
			VBO.Bind();

			if (EBO != null) {
				GL.DrawElements(PrimitiveType.Triangles, EBO.GetSize(), DrawElementsType.UnsignedInt, 0);
			} else {
				GL.DrawArrays(PrimitiveType.Triangles, 0, VBO.GetSize());
			}
		}

		public void AddElement(DataType type, int count, bool normalized = false) {
			int offset = 0;
			if (Elements.Count > 0) {
				offset = Elements[Elements.Count].Offset + Elements[Elements.Count].Size;
			}

			Elements.Add(new VertexElement(type, count, offset, normalized));
		}

		public void Compile(EBO EBO = null) {
			Bind();
			
			if (EBO != null) {
				EBO.Bind();
			}

			int stride = 0;
			foreach (VertexElement element in Elements) {
				stride += element.Size * element.Type.GetSize();
			}

			int index = 0;
			foreach (VertexElement element in Elements) {
				GL.VertexAttribPointer(index, element.Size, element.Type.GetGL(), element.Normalized, stride, 0);
				GL.EnableVertexAttribArray(index);
				index++;
			}
		}

		public void LogState() {
			double[] state = new double[1];
			for (int i = 0; i < Elements.Count; i++) {
				GL.GetVertexAttrib(i, VertexAttribParameter.ArrayEnabled, state);
				Console.WriteLine("[Attribute " + i + "] Enabled: " + state[0]);
				GL.GetVertexAttrib(i, VertexAttribParameter.ArrayNormalized, state);
				Console.WriteLine("[Attribute " + i + "] Normalized: " + state[0]);
				GL.GetVertexAttrib(i, VertexAttribParameter.ArraySize, state);
				Console.WriteLine("[Attribute " + i + "] Size: " + state[0]);
				GL.GetVertexAttrib(i, VertexAttribParameter.ArrayStride, state);
				Console.WriteLine("[Attribute " + i + "] Stride: " + state[0]);
				GL.GetVertexAttrib(i, VertexAttribParameter.ArrayType, state);
				Console.WriteLine("[Attribute " + i + "] Type: " + Enum.GetName(typeof(VertexAttribPointerType), (int) state[0]));
			}
		}

		public void Bind() {
			GL.BindVertexArray(Handle);
		}

		public void Unbind() {
			GL.BindVertexArray(0);
		}
	}

	public struct VertexElement {
		public DataType Type;
		public int Size;
		public int Offset;
		public bool Normalized;

		public VertexElement(DataType type, int size, int offset, bool normalized = false) {
			Type = type;
			Size = size;
			Offset = offset;
			Normalized = normalized;
		}
	}
}
