using GSharp.Graphics.OpenGL;
using GSharp.Graphics.Geometry;
using OpenTK;

namespace GSharp.Graphics {
	public class Sprite : Rectangle, IRenderable {
		protected Texture Texture;

		public Sprite(float x, float y, Texture texture) : base(x, y, texture.GetWidth(), texture.GetHeight()) {
			Texture = texture;
		}

		public virtual Texture GetTexture() {
			return Texture;
		}

		protected override void UpdateData() {
			Vertices = new float[] {
				X, Y, 0f, 1f,
				X + Texture.GetWidth(), Y, 1f, 1f,
				X + Texture.GetWidth(), Y + Texture.GetHeight(), 1f, 0f,
				X, Y + Texture.GetHeight(), 0f, 0f
			};

			Dirty = false;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.TexCoord };
		}
	}

	public class AtlasSprite : Sprite, IRenderable {
		new TextureAtlas Texture;
		private int TexX, TexY, TexW, TexH;

		public AtlasSprite(float x, float y, TextureAtlas atlas, string texture) : base(x, y, atlas) {
			Texture = atlas;

			Vector4 location = atlas.GetLocation(texture);
			TexX = (int) location.X;
			TexY = (int) location.Y;
			TexW = (int) location.Z;
			TexH = (int) location.W;
		}

		new public TextureAtlas GetTexture() {
			return Texture;
		}

		protected override void UpdateData() {
			Vertices = new float[] {
				X, Y, 0f, 1f, TexX, TexY, TexW, TexH,
				X + Texture.GetWidth(), Y, 1f, 1f, TexX, TexY, TexW, TexH,
				X + Texture.GetWidth(), Y + Texture.GetHeight(), 1f, 0f, TexX, TexY, TexW, TexH,
				X, Y + Texture.GetHeight(), 0f, 0f, TexX, TexY, TexW, TexH
			};

			Dirty = false;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.TexCoord, VertexComponent.TexLocation };
		}
	}
}
