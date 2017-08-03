using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;

namespace PUI
{
	public class Button : Label
	{
		private bool _Hovered = false;
		public Button()
		{
			OnHover += Button_OnHover;
		}

		private void Button_OnHover(object arg1, EventArgs.MouseEvent arg2)
		{
			_Hovered = true;
		}


		public override void Update()
		{
			_Hovered = false;
			base.Update();
		}
		public override void Draw(SpriteBatch batch)
		{
			Utils.DrawInvBG(batch, DrawPosition.X, DrawPosition.Y, Width, Height, Window.WindowBackground);
			if (_Hovered)
				batch.Draw(Main.inventoryBack9Texture,
						new Rectangle((int)DrawPosition.X,
						(int)DrawPosition.Y,
						(int)Width,
						(int)Height),
						Color.White);
			base.Draw(batch);
		}
	}
}
