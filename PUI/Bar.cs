using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;

namespace PUI
{
	public class Bar : Container
	{
		private float _Spacing = 5f;
		private float VerticalPadding = 5f;

		private void Resize()
		{
			float w = _Spacing;
			float h = Height;
			for (int i = 0; i < Controls.Count; i++)
			{
				Controls[i].Size = new Vector2(h);
				Controls[i].Position = new Vector2(w, VerticalPadding);
				w += _Spacing + Height;
			}
			Size = new Vector2(w, Height);
		}
		public override void Update()
		{
			base.Update();
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
		}
	}
}
