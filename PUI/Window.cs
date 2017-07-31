using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terraria;
#pragma warning disable CS0809

namespace PUI
{
	public class Window : Container
	{
		public static Texture2D CloseButtonTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/CloseButton");
		public static Color WindowBackground = new Color(33, 15, 91, 255) * 0.685f;
		private Container TitleBar = new Container();
		private Image CloseButton = new Image(CloseButtonTexture);
		private Label TitleLabel = new Label("");
		[Obsolete("Window can't be added into another container", true)]
		public override Container Parent
		{
			get => base.Parent;
			internal set
			{

			}
		}
		public string Title
		{
			get => TitleLabel.Text;
			set => TitleLabel.Text = value;
		}

		public Window()
		{

		}

		public Window(Rectangle bound)
		{
			Position = new Vector2(bound.X, bound.Y);
			Size = new Vector2(bound.Width, bound.Height);

			TitleBar.Position = new Vector2(0, 0);
			TitleBar.Size = new Vector2(Width, 36);

			TitleLabel.Size = new Vector2(Width - 30, TitleBar.Height);
			CloseButton.AnchorPosition = AnchorPosition.TopRight;
			CloseButton.Position = new Vector2(5, 5);
			TitleBar.Controls.Add(TitleLabel);
			TitleBar.Controls.Add(CloseButton);
			TitleBar.OnMouseDown += TitleBar_OnMouseDown;
			TitleBar.OnMouseUp += TitleBar_OnMouseUp;
			Controls.Add(TitleBar);
		}

		private void TitleBar_OnMouseUp(object arg1, EventArgs.OnMouseUpEventArgs arg2)
		{
			_Window_Draging = false;
		}

		private bool _Window_Draging = false;
		private Vector2 _TitleBar_Mouse_Off = Vector2.Zero;
		private void TitleBar_OnMouseDown(object arg1, EventArgs.OnMouseDownEventArgs arg2)
		{
			_Window_Draging = true;
			_TitleBar_Mouse_Off = new Vector2(MouseState.X, MouseState.Y) - DrawPosition;
		}

		public override void Update()
		{
			base.Update();
			if (_Window_Draging)
			{
				Vector2 WindowDrawPos = new Vector2(MouseState.X, MouseState.Y) - _TitleBar_Mouse_Off;
				Position = WindowDrawPos;
			}
		}
		public override void Draw(SpriteBatch batch)
		{
			if (Visible)
			{
				Utils.DrawInvBG(batch, DrawPosition.X, DrawPosition.Y, Width, Height, WindowBackground);//background, so before the child-controls
				base.Draw(batch);
			}
		}
	}
}
