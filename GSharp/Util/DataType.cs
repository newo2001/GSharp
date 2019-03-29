using OpenTK.Graphics.OpenGL;

namespace HelloWorld {
	public enum DataType {
		Byte, Short, Int, Long, Float, Double, Decimal, Bool
	}

	public static class EnumDataType {
		public static int GetSize(this DataType type) {
			switch(type) {
				case DataType.Byte:
				case DataType.Bool:
					return 1;
				case DataType.Short:
					return 2;
				case DataType.Int:
				case DataType.Float:
					return 4;
				case DataType.Long:
				case DataType.Double:
					return 8;
				case DataType.Decimal:
					return 16;
				default:
					return -1;
			}
		}

		public static VertexAttribPointerType GetGL(this DataType type) {
			switch(type) {
				case DataType.Byte:
					return VertexAttribPointerType.Byte;
				case DataType.Short:
					return VertexAttribPointerType.Short;
				case DataType.Int:
					return VertexAttribPointerType.Int;
				case DataType.Float:
					return VertexAttribPointerType.Float;
				case DataType.Double:
					return VertexAttribPointerType.Double;
				default:
					return 0;
			}
		}
	}
}
