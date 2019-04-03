using OpenTK.Graphics.OpenGL;
using GSharp.Util;
using System;

namespace GSharp.Graphics.OpenGL {
	public abstract class Buffer {
		private int Handle;
		protected bool Ready = false;
		protected BufferTarget Target;
		protected bool Dynamic;
		protected int Index = 0;
		protected int Size;

		public Buffer(int size, BufferTarget target, bool dynamic) {
			Handle = GL.GenBuffer();

			Size = size;
			Dynamic = dynamic;
			Target = target;

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

		public abstract int GetSize();
		public abstract void ClearData();
		public abstract void WriteToBuffer();
		public abstract void LogState();
		
	}

	public class VBO : Buffer {
		private float[] Data;

		public VBO(int size, bool dynamic = true) : base(size, BufferTarget.ArrayBuffer, dynamic) {
			Data = new float[size];
		}

		public void AddData(float[] data) {
			data.CopyTo(Data, Index);
			Index += data.Length;
		}

		public override int GetSize() {
			return Index;
		}

		public override void WriteToBuffer() {
			Bind();
			BufferUsageHint usage = Dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw;
			GL.BufferData(Target, Index * 4, Data, usage);
			Ready = true;
		}

		public override void ClearData() {
			Data = new float[Size];
			Index = 0;
		}

		public override void LogState() {
			base.LogState("VBO", BufferTarget.ArrayBuffer);
		}

		public static void Unbind() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}

	public class EBO : Buffer {
		private int[] Data;

		public EBO(int size, bool dynamic = true) : base(size, BufferTarget.ElementArrayBuffer, dynamic) {
			Data = new int[size];
		}

		public override int GetSize() {
			return Index;
		}

		public override void WriteToBuffer() {
			Bind();
			BufferUsageHint usage = Dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw;
			GL.BufferData(Target, Index * 4, Data, usage);
			Ready = true;
		}

		public override void ClearData() {
			Data = new int[Size];
			Index = 0;
		}

		public void AddData(int[] data) {
			data.CopyTo(Data, Index);
			Index += data.Length;
		}

		public override void LogState() {
			base.LogState("EBO", BufferTarget.ElementArrayBuffer);
		}

		public static void Unbind() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}
	}
}
