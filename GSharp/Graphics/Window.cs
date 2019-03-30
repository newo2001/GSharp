using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using GSharp.Graphics.OpenGL;
using GSharp.Graphics.UI;
using GSharp.Events;

namespace GSharp.Graphics {
	public class Window : GameWindow {

		private Renderer Renderer;
		private int VirtualWidth, VirtualHeight;

		public Window(int width, int height) : base(width, height, GraphicsMode.Default, "Hello World!") {
			//WindowState = WindowState.Fullscreen;
			VirtualWidth = width;
			VirtualHeight = height;
			VSync = VSyncMode.Off;

			Shader shader = new Shader("../../Graphics/OpenGL/Shaders/vertex.glsl", "../../Graphics/OpenGL/Shaders/fragment.glsl");
			shader.SetUniform("color", 1.0f, 0f, 0f);

			Renderer = new Renderer(shader, true);
		}

		protected override void OnLoad(EventArgs e) {
			GSharp.Events.EventHandler.RegisterEvents(this);

			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Enable(EnableCap.ScissorTest);

			// Rendering code

			base.OnLoad(e);
		}

		protected override void OnResize(EventArgs e) {
			float aspect = (float) VirtualWidth / VirtualHeight;
			int vpWidth = Width;
			int vpHeight = (int) (Width / aspect + 0.5f);

			if (vpHeight > Height) {
				vpHeight = Height;
				vpWidth = (int) (Height * aspect + 0.5f);
			}

			int vpX = (Width / 2) - (vpWidth / 2);
			int vpY = (Height / 2) - (vpHeight / 2);
			GL.Viewport(vpX, vpY, vpWidth, vpHeight);
			GL.Scissor(vpX, vpY, vpWidth, vpHeight);

			Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0f, VirtualWidth, VirtualHeight, 0f, 0f, 1f);
			Renderer.GetShader().SetUniform("projection", projection);

			base.OnResize(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e) {
			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			GL.Clear(ClearBufferMask.ColorBufferBit);

			Renderer.Render();

			Context.SwapBuffers();

			base.OnRenderFrame(e);
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e) {
			KeyUpEvent.Call(e.Key);
			base.OnKeyUp(e);
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e) {
			if (e.IsRepeat) {
				KeyRepeatEvent.Call(e.Key);
			} else {
				KeyDownEvent.Call(e.Key);
			}

			base.OnKeyDown(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e) {
			MousePressEvent.Call(e.Button, e.X, e.Y);
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e) {
			MouseReleaseEvent.Call(e.Button, e.X, e.Y);
			base.OnMouseUp(e);
		}

		public Renderer GetRenderer() {
			return Renderer;
		}
	}
}
