using System.Drawing;
using System.IO;
using System;
using OpenTK.Graphics.OpenGL;

namespace GSharp.Graphics {
	public class Texture {
		private int Handle;
		private int Slot;

		public Texture(string path) {
			Image img = Image.FromFile("../../" + path);
			Bitmap bmp = new Bitmap(img);
			byte[] data = new byte[bmp.Width * bmp.Height * 4];
			
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
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
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

		public int GetLastSlot() {
			return Slot;
		}
	}
}
