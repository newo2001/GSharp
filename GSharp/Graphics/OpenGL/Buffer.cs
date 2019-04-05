using OpenTK.Graphics.OpenGL;
using GSharp.Util;
using System;

namespace GSharp.Graphics.OpenGL {
	public class Buffer<T> where T : struct {
		private int Handle;
		private bool Ready = false;
		private BufferTarget Target;
		private bool Dynamic;
		private int Index = 0;
		private int Size;
		private T[] Data;

		public Buffer(int size, bool dynamic) {
			if (typeof(T) == typeof(int)) {
				Target = BufferTarget.ElementArrayBuffer;
			} else if (typeof(T) == typeof(float)) {
				Target = BufferTarget.ArrayBuffer;
			} else {
				Logger.Log("Buffer can only be constructed with the types: int, float", Severity.Error);
				return;
			}

			Handle = GL.GenBuffer();

			Size = size;
			Dynamic = dynamic;
			Data = new T[Size];

			Bind();
		}

		public bool IsReady() {
			return Ready;
		}

		public int GetCapacity() {
			return Size;
		}

		public void Bind() {
			GL.BindBuffer(Target, Handle);
		}

		protected void LogState(string name, BufferTarget target) {
			int state;
			GL.GetBufferParameter(target, BufferParameterName.BufferSize, out state);
			Logger.Log("[" + name + "] Size: " + state);
			GL.GetBufferParameter(target, BufferParameterName.BufferUsage, out state);
			Logger.Log("[" + name + "] Usage: " + Enum.GetName(typeof(BufferUsageHint), state));
		}

		public void AddData(T[] data) {
			data.CopyTo(Data, Index);
			Index += data.Length;
		}

		public int GetSize() {
			return Index;
		}

		public void WriteToBuffer() {
			Bind();
			BufferUsageHint usage = Dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw;
			GL.BufferData(Target, Index * 4, Data, usage);
			Ready = true;
		}

		public void ClearData() {
			Data = new T[Size];
			Index = 0;
		}

		public void Unbind() {
			GL.BindBuffer(Target, 0);
		}

		public static void UnbindVBO() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public static void UnbindEBO() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}
	}
}
