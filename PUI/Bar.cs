using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI.Chat;

namespace PUI
{
	public class Bar : Container
	{
		private Bar _SubBar = null;
		public Bar SubBar
		{
			get => _SubBar;
			set
			{
				if (value == null)
				{
					if (_SubBar != null)
					{
						_SubBar.ParentBar = null;
						_SubBar = null;
					}
				}
				else
				{
					if (_SubBar != null)
					{
						_SubBar.ParentBar = null;
						_SubBar = null;
					}
					_SubBar = value;
					_SubBar.ParentBar = this;
				}
			}
		}
		public Bar ParentBar
		{
			get;
			private set;
		}
		private float _Spacing = 8f;
		private float VerticalPadding = 10f;

		private void Resize()
		{
			float w = _Spacing;
			float h = Height;
			for (int i = 0; i < Controls.Count; i++)
			{
				float ty = h - 2 * VerticalPadding;
				Controls[i].Size = new Vector2(h * (ty / h), ty);
				Controls[i].Position = new Vector2(w, VerticalPadding);
				w += _Spacing + Controls[i].Width;
			}
		}
		public override void Update()
		{
			base.Update();
			if (SubBar != null)
				SubBar.Update();
		}
		private void DrawBackground(SpriteBatch batch)
		{
			Utils.DrawInvBG(batch, DrawPosition.X, DrawPosition.Y, Width, Height, Window.WindowBackground);
		}
		public override void Draw(SpriteBatch batch)
		{
			Resize();
			DrawBackground(batch);
			base.Draw(batch);
			if (SubBar != null)
			{
				SubBar.Position = new Vector2(X + (Width / 2 - SubBar.Width / 2), Y - SubBar.Height);
				SubBar.Draw(batch);
			}
			var aC = ControlAt(MouseState.X, MouseState.Y);
			if (aC != null)
			{
				if (aC.ToolTip != null && aC != this && aC.ToolTip.Trim() != "")
					ChatManager.DrawColorCodedStringWithShadow(batch, Main.fontMouseText, aC.ToolTip, new Vector2(MouseState.X, MouseState.Y) - new Vector2(20, 20), Color.White, 0f, Vector2.Zero, Vector2.One);
			}
		}
	}
}
