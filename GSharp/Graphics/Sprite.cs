using GSharp.Graphics.OpenGL;
using GSharp.Graphics.Geometry;
using OpenTK;

namespace GSharp.Graphics {
	public class Sprite : Rectangle, IRenderable {
		private Texture Texture;

		public Sprite(float x, float y, float width, float height, Texture texture) : base(x, y, width, height) {
			Texture = texture;
		}

		public virtual Texture GetTexture() {
			return Texture;
		}

		protected override void UpdateData() {
			ApplyTransform();

			Vertices = new float[] {
				TopLeft.X, TopLeft.Y, 0f, 1f,
				TopRight.X, TopRight.Y, 1f, 1f,
				BottomRight.X, BottomRight.Y, 1f, 0f,
				BottomLeft.X, BottomLeft.Y, 0f, 0f
			};

			Dirty = false;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.TexCoord };
		}
	}

	public class AtlasSprite : Rectangle, IRenderable {
		private int TexX, TexY, TexW, TexH;
		private string Location;

		public AtlasSprite(float x, float y, float width, float height, string texture) : base(x, y, width, height) {
			Location = texture;
		}

		public void SetAtlas(TextureAtlas atlas) {
			Vector4 location = atlas.GetLocation(Location);

			TexX = (int)location.X;
			TexY = (int)location.Y;
			TexW = (int)location.Z;
			TexH = (int)location.W;

			UpdateData();
		}

		protected override void UpdateData() {
			ApplyTransform();

			Vertices = new float[] {
				TopLeft.X, TopLeft.Y, 0f, 1f, TexX, TexY, TexW, TexH,
				TopRight.X, TopRight.Y, 1f, 1f, TexX, TexY, TexW, TexH,
				BottomRight.X, BottomRight.Y, 1f, 0f, TexX, TexY, TexW, TexH,
				BottomLeft.X, BottomLeft.Y, 0f, 0f, TexX, TexY, TexW, TexH
			};

			Dirty = false;
		}

		public override VertexComponent[] GetVertexFormat() {
			return new VertexComponent[] { VertexComponent.Coord, VertexComponent.TexCoord, VertexComponent.TexLocation };
		}
	}
}
