using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using GSharp.Graphics.OpenGL;
using GSharp.Util;
using GSharp.Events;
using GSharp.Graphics.Geometry;

namespace GSharp.Graphics {
	public class Window : GameWindow {
		private RenderBatch Batch;
		private RenderBatch TextureBatch;
		private RenderBatch AtlasBatch;
		private TextureAtlas Atlas;
		private int VirtualWidth, VirtualHeight;

		private ColoredCircle Circle;
		private AtlasSprite Sprite;

		public Window(int width, int height) : base(width, height, GraphicsMode.Default, "Hello World!") {
			//WindowState = WindowState.Fullscreen;
			VirtualWidth = width;
			VirtualHeight = height;
			VSync = VSyncMode.Off;
		}

		protected override void OnLoad(EventArgs e) {
			Events.EventHandler.RegisterEvents(this);
			Logger.Name = "GSharp";

			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Enable(EnableCap.ScissorTest);

			Atlas = new TextureAtlas("atlas.png", "atlas.desc", TextureFilter.Nearest);

			Shader shader = new Shader("Graphics/OpenGL/Shaders/vertex.glsl", "Graphics/OpenGL/Shaders/fragment.glsl");
			Shader textureShader = new Shader("Graphics/OpenGL/Shaders/vertexTex.glsl", "Graphics/OpenGL/Shaders/fragmentTex.glsl");
			Shader atlasShader = new Shader("Graphics/OpenGL/Shaders/vertexAtlas.glsl", "Graphics/OpenGL/Shaders/fragmentTex.glsl");

			Batch = new RenderBatch(new VertexComponent[] { VertexComponent.Coord, VertexComponent.Color }, shader, false);
			TextureBatch = new RenderBatch(new VertexComponent[] { VertexComponent.Coord, VertexComponent.TexCoord }, textureShader, false);
			AtlasBatch = new RenderBatch(new VertexComponent[] { VertexComponent.Coord, VertexComponent.TexCoord, VertexComponent.TexLocation }, atlasShader, false);
			AtlasBatch.SetAtlas(Atlas);

			LoadGeometry();

			base.OnLoad(e);
		}

		private void LoadGeometry() {
			Circle = new ColoredCircle(200, 200, 200, new Vector3(255, 0, 255), 32);
			Batch.Add(Circle);
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
			RenderBatch.SetProjectionMatrix(projection);

			base.OnResize(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e) {
			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			GL.Clear(ClearBufferMask.ColorBufferBit);

			Batch.Render();
			TextureBatch.Render();
			AtlasBatch.Render();

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
	}
}
