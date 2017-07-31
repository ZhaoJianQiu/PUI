using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace PUI
{
	public class Label : Control
	{
		protected const float _Spacing = 3f;
		private TextAlign _TextAlign = TextAlign.Left;
		public TextAlign TextAlign
		{
			get => _TextAlign;
			set => _TextAlign = value;
		}
		private string _Text = "";
		public string Text
		{
			get => _Text;
			set => _Text = value;
		}
		private Color _TextColor = Color.White;
		public Color TextColor
		{
			get => _TextColor;
			set => _TextColor = value;
		}
		public Label(string Text)
		{
			this.Text = Text;
		}
		public Label()
		{

		}
		protected virtual List<TextSnippet> GetDrawString()
		{
			return ChatManager.ParseMessage(Text, TextColor);
		}
		public override void Draw(SpriteBatch batch)
		{
			if (Visible)
			{
				base.Draw(batch);
				DynamicSpriteFont sf = Main.fontMouseText;
				Text = _Limit(sf, Text, Width - _Spacing * 2);
				Vector2 size = ChatManager.GetStringSize(sf, Text, Vector2.One);
				/*if (Text.Trim() == "")
				{*/
					size.Y = ChatManager.GetStringSize(sf, "[i:3063]", Vector2.One).Y;//To get the default Height
				//}
				List<TextSnippet> snippets = GetDrawString();
				ChatManager.DrawColorCodedStringWithShadow(batch, sf, snippets.ToArray(), AlignPos(size), 0f, Vector2.Zero, Vector2.One, out var hovered);
			}
		}
		protected static string _Limit(DynamicSpriteFont dsf, string src, float maxWidth)
		{
			for (int i = 0; i <= src.Length; i++)
			{
				string s = src.Substring(0, i);
				if (ChatManager.GetStringSize(dsf, s, Vector2.One).X > maxWidth)
					return src.Substring(0, i - 1);
			}
			return src;
		}
		private Vector2 AlignPos(Vector2 fontSize)
		{
			switch (TextAlign)
			{
				case TextAlign.Left:
					return new Vector2(DrawPosition.X + _Spacing, DrawPosition.Y + Height / 2 - fontSize.Y / 2);
				case TextAlign.Right:
					return new Vector2(DrawPosition.X + (Width - _Spacing - fontSize.X), DrawPosition.Y + (Height / 2 - fontSize.Y / 2));
				case TextAlign.Center:
					return new Vector2(DrawPosition.X + (Width / 2 - fontSize.X / 2), DrawPosition.Y + (Height / 2 - fontSize.Y / 2));
			}
			return DrawPosition;
		}
	}
}
