using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using HelloWorld.Util;
using System;

namespace HelloWorld.Graphics {
	class Shader {
		private int Handle;
		private Dictionary<string, int> UniformCache;

		public Shader(string vertexPath, string fragmentPath) {
			UniformCache = new Dictionary<string, int>();

			string info;
			int status;

			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, new TextFile(vertexPath).GetText());
			GL.CompileShader(vertexShader);
			GL.GetShaderInfoLog(vertexShader, out info);
			GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status);

			if (status != 1) {
				throw new ApplicationException("Vertex Shader compilation failed:\n " + info);
			}

			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, new TextFile(fragmentPath).GetText());
			GL.CompileShader(fragmentShader);
			GL.GetShaderInfoLog(fragmentShader, out info);
			GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status);

			if (status != 1) {
				throw new ApplicationException("Fragment Shader compilation failed:\n " + info);
			}

			Handle = GL.CreateProgram();
			GL.AttachShader(Handle, vertexShader);
			GL.AttachShader(Handle, fragmentShader);
			GL.LinkProgram(Handle);

			info = GL.GetProgramInfoLog(Handle);
			if (info != "") {
				Console.WriteLine("Shader program compilation failed: " + info);
			}

			Use();
		}

		public void Use() {
			GL.UseProgram(Handle);
		}

		public void SetUniform(string name, params float[] values) {
			int location = GetUniformLocation(name);

			switch (values.Length) {
				case 1:
					GL.Uniform1(location, values[0]);
					break;
				case 2:
					GL.Uniform2(location, values[0], values[1]);
					break;
				case 3:
					GL.Uniform3(location, values[0], values[1], values[2]);
					break;
				case 4:
					GL.Uniform4(location, values[0], values[1], values[3], values[4]);
					break;
			}
		}

		public void SetUniform(string name, params int[] values) {
			float[] floats = (float[]) values.Clone();
			SetUniform(name, values);
		}

		public void SetUniform(string name, Matrix4 value) {
			GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
		}

		private int GetUniformLocation(string name) {
			if (UniformCache.ContainsKey(name)) {
				UniformCache.TryGetValue(name, out int location);
				return location;
			} else {
				int location = GL.GetUniformLocation(Handle, name);
				UniformCache.Add(name, location);
				return location;
			}
		}
	}
}
