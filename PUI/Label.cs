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
		protected DynamicSpriteFont SpriteFont = Main.fontMouseText;
		protected float _PaddingLeft = 4f, _PaddingRight = 4f;
		protected bool MultiLine
		{
			get;
			set;
		}
		private string _Text = "";
		private float _TextVerticalPadding = 5f;
		public float TextVerticalPadding
		{
			get => _TextVerticalPadding;
			set => _TextVerticalPadding = value;
		}
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
		protected virtual List<string> _Line_Split(string str, float maxWidth)
		{
			if (ChatManager.GetStringSize(SpriteFont, str, Vector2.One).X <= maxWidth)
				return new List<string>() { str };
			for (int i = str.Length; i > 0; i--)
			{
				string s = str.Substring(0, i);
				if (ChatManager.GetStringSize(SpriteFont, s, Vector2.One).X <= maxWidth)
				{
					List<string> r = new List<string>();
					r.Add(s);
					r.AddRange(_Line_Split(str.Substring(i), maxWidth));
					return r;
				}
			}
			return new List<string>() { str };
		}
		protected virtual List<List<TextSnippet>> GetDrawStringMultiLine(string str)
		{
			string[] clipedString = str.Split(new string[] { "\n" }, StringSplitOptions.None);
			List<string> txt = new List<string>();
			foreach (var s in clipedString)
			{
				var e = _Line_Split(s, Width - (_PaddingLeft + _PaddingRight));
				txt.AddRange(e);
			}
			List<List<TextSnippet>> r = new List<List<TextSnippet>>();
			for (int i = 0; i < txt.Count; i++)
			{
				r.Add(ChatManager.ParseMessage(txt[i], TextColor));
			}
			return r;
		}
		protected virtual void DrawString(SpriteBatch batch, List<List<TextSnippet>> text)
		{
			float height_Text = ChatManager.GetStringSize(SpriteFont, "[i:3063]", Vector2.One).Y;
			float height_Per_Line = height_Text + TextVerticalPadding * 2;
			if (MultiLine)
			{
				for (int i = 0; i < text.Count; i++)
				{
					var line = text[i];
					var dPos = DrawPosition;
					dPos.Y += height_Per_Line * i;
					dPos.Y += height_Per_Line / 2 - height_Text / 2;
					dPos.X += _PaddingLeft;
					if (dPos.Y + height_Per_Line <= DrawPosition.Y + Height)
						ChatManager.DrawColorCodedStringWithShadow(batch, SpriteFont, line.ToArray(), dPos, 0f, Vector2.Zero, Vector2.One, out var hovered);
				}
			}
			else
			{
				Vector2 dPos = DrawPosition + new Vector2(_PaddingLeft, Height / 2 - height_Per_Line / 2);
				ChatManager.DrawColorCodedStringWithShadow(batch, SpriteFont, text[0].ToArray(), dPos, 0f, Vector2.Zero, Vector2.One, out var hovered);
			}
		}
		public override void Draw(SpriteBatch batch)
		{
			if (Visible)
			{
				base.Draw(batch);
				DynamicSpriteFont sf = Main.fontMouseText;
				List<List<TextSnippet>> lines = GetDrawStringMultiLine(Text);
				if (lines.Count == 0)
					lines.Add(new List<TextSnippet>() { new TextSnippet("") });
				DrawString(batch, lines);
			}
		}
	}
}
