using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using HelloWorld.Graphics;

namespace HelloWorld {
	class Window : GameWindow {

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
			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Enable(EnableCap.ScissorTest);

			// Rendering code
			Mesh mesh = new Mesh(4, 6);
			mesh.AddRelativeIndices(new int[] { 0, 1, 2, 0, 2, 3 });
			mesh.AddVertex(new Vertex(0f, 0f));
			mesh.AddVertex(new Vertex(400f, 0f));
			mesh.AddVertex(new Vertex(400f, 400f));
			mesh.AddVertex(new Vertex(0f, 400f));
			Renderer.AddMesh(mesh);

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
			base.OnKeyUp(e);

			if (e.Key == Key.Escape) {
				Exit();
			}
		}

		public Renderer GetRenderer() {
			return Renderer;
		}
	}
}
