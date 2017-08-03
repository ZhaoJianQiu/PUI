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
		public Bar SubBar
		{
			get;
			set;
		}
		private float _Spacing = 8f;
		private float VerticalPadding = 5f;

		private void Resize()
		{
			float w = _Spacing;
			float h = Height;
			for (int i = 0; i < Controls.Count; i++)
			{
				Controls[i].Size = new Vector2(h, h - 2 * VerticalPadding);
				Controls[i].Position = new Vector2(w, VerticalPadding);
				w += _Spacing + Height;
			}
			//Size = new Vector2(w, Height);
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
				SubBar.Draw(batch);
				SubBar.Position = new Vector2(X + (Width / 2 - SubBar.Width / 2), Y - SubBar.Height);
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
