using OpenTK;

namespace GSharp.Util {
	public static class ArrayUtils {
		public static bool CompareEquals<T>(this T[] arr, T[] arr2) {
			if (arr.Length != arr2.Length) {
				return false;
			}

			for (int i = 0; i < arr.Length; i++) {
				if (!arr[i].Equals(arr2[i])) {
					return false;
				}
			}

			return true;
		}
	}
}
