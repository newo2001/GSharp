using GSharp.Graphics.OpenGL;

namespace GSharp.Graphics {
	public interface IRenderable {
		float[] GetVertexData();
		int[] GetIndexData();
		VertexComponent[] GetVertexFormat();
		bool IsDirty();
	}
}