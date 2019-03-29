using System.IO;
using System.Text;

namespace HelloWorld.Util {
	class TextFile {
		private string Path;
		private string Text;

		public TextFile(string path) {
			Path = path;

			FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
				Text = reader.ReadToEnd();
			}
		}

		public string GetPath() {
			return Path;
		}

		public string GetText() {
			return Text;
		}
	}
}
