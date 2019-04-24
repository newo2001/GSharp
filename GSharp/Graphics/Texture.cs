using System.Drawing;
using GSharp.Util;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System;
using System.Collections.Generic;

namespace GSharp.Graphics {
	public enum TextureFilter {
		Linear, Nearest
	}

	public class Texture {
		private int Handle;
		private int Slot;
		private int Width, Height;

		public Texture(string path, TextureFilter filter = TextureFilter.Linear) {
			Image img = Image.FromFile("../../" + path);
			Bitmap bmp = new Bitmap(img);
			Width = bmp.Width;
			Height = bmp.Height;
			int minFilter = filter == TextureFilter.Linear ? (int) TextureMinFilter.Linear : (int) TextureMinFilter.Nearest;
			int magFilter = filter == TextureFilter.Linear ? (int) TextureMagFilter.Linear : (int) TextureMagFilter.Nearest;

			byte[] data = new byte[Width * Height * 4];
			
			for (int y = 0; y < bmp.Height; y++) {
				for (int x = 0; x < bmp.Width; x++) {
					Color color = bmp.GetPixel(x, y);
					data[(bmp.Height - y - 1) * bmp.Width * 4 + x * 4] = color.R;
					data[(bmp.Height - y - 1) * bmp.Width * 4 + x * 4 + 1] = color.G;
					data[(bmp.Height - y - 1) * bmp.Width * 4 + x * 4 + 2] = color.B;
					data[(bmp.Height - y - 1) * bmp.Width * 4 + x * 4 + 3] = color.A;
				}
			}

			Handle = GL.GenTexture();
			Bind();
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, minFilter);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, magFilter);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

			img.Dispose();
		}

		public void Bind(int slot = 0) {
			GL.ActiveTexture(TextureUnit.Texture0 + slot);
			GL.BindTexture(TextureTarget.Texture2D, Handle);
			Slot = slot;
		}

		public static void Unbind(int slot = 0) {
			GL.ActiveTexture(TextureUnit.Texture0 + slot);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public int GetWidth() {
			return Width;
		}

		public int GetHeight() {
			return Height;
		}

		public int GetLastSlot() {
			return Slot;
		}
	}

	public class TextureAtlas : Texture {
		private Dictionary<string, Vector4> locations;

		public TextureAtlas(string texturePath, string descriptorPath, TextureFilter filter = TextureFilter.Linear) : base(texturePath, filter) {
			locations = new Dictionary<string, Vector4>();
			string[] lines = new TextFile("../../" + descriptorPath).GetText().Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');

			for (int i = 0; i < lines.Length; i++) {
				string[] tokens = lines[i].Split(' ');
				if (tokens.Length != 5) {
					Logger.Log("Parsing texture atlas descriptor failed, line " + i + " did not have 5 tokens", Severity.Error);
					return;
				}

				if (!Int32.TryParse(tokens[1], out int x) || !Int32.TryParse(tokens[2], out int y) || !Int32.TryParse(tokens[3], out int w) || !Int32.TryParse(tokens[4], out int h)) {
					Logger.Log("Parsing texture atlas descriptor failed, line " + i + " had a NaN coordinate");
					return;
				} else {
					locations.Add(tokens[0], new Vector4(x, y, w, h));
				}
			}
		}

		public Vector4 GetLocation(string name) {
			if (!locations.TryGetValue(name, out Vector4 location)) {
				Logger.Log("Failed to get texture location for " + name + " in the texture atlas", Severity.Error);
			}
			return location;
		}
	}
}
