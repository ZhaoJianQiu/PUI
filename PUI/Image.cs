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
		public override Vector2 Size
		{
			get => new Vector2(Texture.Width, Texture.Height) * Scale;
		}
		public Color Color
		{
			get;
			set;
		}
		public Vector2 Scale
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

		public Texture2D Texture
		{
			get;
			set;
		}
		public Image(Texture2D texture)
		{
			Color = Color.White;
			Scale = new Vector2(1f, 1f);
			SpriteEffects = SpriteEffects.None;
			Texture = texture;
			DrawingRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
		}
		public override void Draw(SpriteBatch batch)
		{
			base.Draw(batch);
			batch.Draw(Texture, DrawPosition, DrawingRectangle, Color, 0, Vector2.Zero, Scale, SpriteEffects, 0f);
		}
	}
}
