namespace GSharp.Graphics.OpenGL {
	public enum VertexComponent {
		Coord, Normal, TexCoord, Color, TexLocation
	}

	public static class EnumVertexComponent {
		public static int Size(this VertexComponent component) {
			switch(component) {
				case VertexComponent.Coord:
				case VertexComponent.Normal:
				case VertexComponent.TexCoord:
					return 2;
				case VertexComponent.Color:
					return 3;
				case VertexComponent.TexLocation:
					return 4;
				default:
					return 0;
			}
		}
	}
}
