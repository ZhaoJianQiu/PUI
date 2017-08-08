using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
#pragma warning disable CS0809

namespace PUI
{
	public class Window : Container
	{
		public WindowManager WindowManager
		{
			get;
			internal set;
		}
		public static Texture2D CloseButtonTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/CloseButton");
		public static Texture2D MinimizeButtonTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/MinimizeButton");
		//PanelBackground
		public static Color WindowBackground = new Color(33, 15, 91, 255) * 0.685f;
		private Container TitleBar = new Container();
		private Image CloseButton = new Image(CloseButtonTexture);
		private Image MiniMizeButton = new Image(MinimizeButtonTexture);
		private Image IconImage = new Image(Main.itemTexture[ItemID.FragmentStardust]);
		public Texture2D Icon
		{
			get => IconImage.Texture;
			set => IconImage.Texture = value;
		}
		private Label TitleLabel = new Label("");
		[Obsolete("Window can't be added into another container", true)]
		public override Container Parent
		{
			get => base.Parent;
			internal set
			{

			}
		}
		public bool Minimized
		{
			get;
			set;
		}
		public string Title
		{
			get => TitleLabel.Text;
			set => TitleLabel.Text = value;
		}

		public Window(Rectangle bound)
		{
			Position = new Vector2(bound.X, bound.Y);
			Size = new Vector2(bound.Width, bound.Height);

			TitleBar.Position = new Vector2(0, 0);
			TitleBar.Size = new Vector2(Width, 30);

			IconImage.Size = new Vector2(20, 20);
			IconImage.Position = new Vector2(5, 1);

			TitleLabel.Size = new Vector2(Width - 55, TitleBar.Height);
			TitleLabel.Position = new Vector2(25, 0);
			CloseButton.AnchorPosition = AnchorPosition.TopRight;
			CloseButton.Position = new Vector2(5, 5);
			CloseButton.Size = new Vector2(16, 15);
			CloseButton.OnClick += CloseButton_OnClick;

			MiniMizeButton.AnchorPosition = AnchorPosition.TopRight;
			MiniMizeButton.Position = new Vector2(24, 5);
			MiniMizeButton.Size = new Vector2(16, 15);
			MiniMizeButton.OnClick += MiniMizeButton_OnClick;

			TitleBar.Controls.Add(TitleLabel);
			TitleBar.Controls.Add(IconImage);
			//TitleBar.Controls.Add(MiniMizeButton);
			TitleBar.Controls.Add(CloseButton);
			TitleBar.OnMouseDown += TitleBar_OnMouseDown;
			TitleBar.OnMouseUp += TitleBar_OnMouseUp;
			Controls.Add(TitleBar);
		}

		public event Action<object, EventArgs.EventArgs> OnMinimizing;
		private void MiniMizeButton_OnClick(object arg1, EventArgs.OnClickEventArgs arg2)
		{
			var e = new EventArgs.EventArgs();
			OnMinimizing?.Invoke(this, e);
			if (e.Handled) return;
			Minimized = true;
		}

		public event Action<object, EventArgs.EventArgs> OnClosing;
		private void CloseButton_OnClick(object arg1, EventArgs.OnClickEventArgs arg2)
		{
			OnClosing?.Invoke(this, new EventArgs.EventArgs());
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
			}
			base.Draw(batch);
		}
	}
}
