using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terraria;

namespace PUI
{
	public class Window : Container
	{
		public static Texture2D CloseButtonTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/CloseButton");
		public static Color WindowBackground;
		private Container TitleBar = new Container();
		private Image CloseButton = new Image(CloseButtonTexture);
		private Label TitleLabel = new Label("");
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
			Controls.Add(TitleBar);
		}

		public override void Update()
		{
			base.Update();
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
