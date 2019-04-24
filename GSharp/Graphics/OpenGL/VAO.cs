using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using GSharp.Graphics.OpenGL;
using GSharp.Util;
using System;

namespace GSharp.Graphics.OpenGL {
	public class VAO {
		private int Handle;
		private List<VertexElement> Elements;

		public VAO() {
			Handle = GL.GenVertexArray();
			Elements = new List<VertexElement>();
			Bind();
		}

		public void AddElement(Util.DataType type, int count, bool normalized = false) {
			int offset = 0;
			if (Elements.Count > 0) {
				offset = Elements[Elements.Count - 1].Offset + Elements[Elements.Count - 1].Size * Elements[Elements.Count - 1].Type.GetSize();
			}

			Elements.Add(new VertexElement(type, count, offset, normalized));
		}

		public void Compile(Buffer<float> VBO) {
			Bind();
			VBO.Bind();

			int stride = 0;
			foreach (VertexElement element in Elements) {
				stride += element.Size * element.Type.GetSize();
			}

			int index = 0;
			foreach (VertexElement element in Elements) {
				GL.VertexAttribPointer(index, element.Size, element.Type.GetGL(), element.Normalized, stride, element.Offset);
				GL.EnableVertexAttribArray(index);
				index++;
			}
		}

		public void Clear() {
			Elements.Clear();
		}

		#if (DEBUG)
			public void LogState() {
				double[] state = new double[1];
				for (int i = 0; i < Elements.Count; i++) {
					GL.GetVertexAttrib(i, VertexAttribParameter.ArrayEnabled, state);
					Logger.Log("[VAO Attribute " + i + "] Enabled: " + state[0]);
					GL.GetVertexAttrib(i, VertexAttribParameter.ArrayNormalized, state);
					Logger.Log("[VAO Attribute " + i + "] Normalized: " + state[0]);
					GL.GetVertexAttrib(i, VertexAttribParameter.ArraySize, state);
					Logger.Log("[VAO Attribute " + i + "] Size: " + state[0]);
					GL.GetVertexAttrib(i, VertexAttribParameter.ArrayStride, state);
					Logger.Log("[VAO Attribute " + i + "] Stride: " + state[0]);
					GL.GetVertexAttrib(i, VertexAttribParameter.ArrayType, state);
					Logger.Log("[VAO Attribute " + i + "] Type: " + Enum.GetName(typeof(VertexAttribPointerType), (int) state[0]));
				}
			}
		#endif

		public void Bind() {
			GL.BindVertexArray(Handle);
		}

		public static void Unbind() {
			GL.BindVertexArray(0);
		}
	}

	public struct VertexElement {
		public Util.DataType Type;
		public int Size;
		public int Offset;
		public bool Normalized;

		public VertexElement(Util.DataType type, int size, int offset, bool normalized = false) {
			Type = type;
			Size = size;
			Offset = offset;
			Normalized = normalized;
		}
	}
}
