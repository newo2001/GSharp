using System;
using OpenTK;

namespace GSharp.Util {
	public static class Logger {
		public static string Name;

		public static void Log(string message, Severity severity = Severity.Debug) {
			ConsoleColor oldColor = Console.ForegroundColor;
			switch (severity) {
				case Severity.Debug:
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				case Severity.Info:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case Severity.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case Severity.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case Severity.Fatal:
					Console.ForegroundColor = ConsoleColor.Magenta;
					throw new Exception("A fatal error occured: " + message);
			}

			Console.WriteLine("[" + Name + "] [" + severity.ToString() + "] " + message);
			Console.ForegroundColor = oldColor;
		}

		public static void Log(Vector2 vec, Severity severity = Severity.Debug) {
			Log(vec.X + ", " + vec.Y, severity);
		}

		public static void Log(Vector3 vec, Severity severity = Severity.Debug) {
			Log(vec.X + ", " + vec.Y + ", " + vec.Z, severity);
		}

		public static void Log(Vector4 vec, Severity severity = Severity.Debug) {
			Log(vec.X + ", " + vec.Y + ", " + vec.Z + ", " + vec.W, severity);
		}
	}

	public enum Severity {
		Debug, Info, Warning, Error, Fatal
	}
}
