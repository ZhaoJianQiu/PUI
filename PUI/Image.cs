using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace PUI
{
	public class Image : Control
	{
		public Color Color
		{
			get;
			set;
		}
		public SpriteEffects SpriteEffects
		{
			get;
			set;
		}
		public Rectangle DrawingRectangle
		{
			get;
			set;
		}
		private Texture2D _Texture = null;
		public Texture2D Texture
		{
			get=>_Texture;
			set
			{
				_Texture = value;
				DrawingRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
			}
		}
		public Image(Texture2D texture)
		{
			Color = Color.White;
			SpriteEffects = SpriteEffects.None;
			Texture = texture;
			DrawingRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
		}
		public override void Draw(SpriteBatch batch)
		{
			base.Draw(batch);
			batch.Draw(Texture, new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, (int)Width, (int)Height), DrawingRectangle, Color, 0f, Vector2.Zero, SpriteEffects, 0f);
		}
	}
}
