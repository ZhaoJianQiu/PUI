using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using Terraria.UI.Chat;

namespace PUI
{
	public class TextBox : Label
	{
		public static Texture2D TextBoxEdge = Main.instance.OurLoad<Texture2D>("Qiu/UI/TextboxEdge");
		private long _Timer = 0;
		private bool _Blink = false;
		public static Texture2D TextboxFill //from CheatSheet
		{
			get
			{
				Texture2D textboxFill = null;
				Color[] edgeColors = new Color[TextBoxEdge.Width * TextBoxEdge.Height];
				TextBoxEdge.GetData(edgeColors);
				Color[] fillColors = new Color[TextBoxEdge.Height];
				for (int y = 0; y < fillColors.Length; y++)
				{
					fillColors[y] = edgeColors[TextBoxEdge.Width - 1 + y * TextBoxEdge.Width];
				}
				textboxFill = new Texture2D(Main.spriteBatch.GraphicsDevice, 1, fillColors.Length);
				textboxFill.SetData(fillColors);
				return textboxFill;
			}
		}
		public TextBox(string Text) : base(Text)
		{
			Focusable = true;
			OnClick += TextBox_OnClick;
		}

		private void TextBox_OnClick(object arg1, EventArgs.OnClickEventArgs arg2)
		{
			Focused = true;
		}

		public TextBox() : this("")
		{

		}

		protected override List<TextSnippet> GetDrawString()
		{
			var s = base.GetDrawString();
			if (Focused)
				s.Add(new TextSnippet(_Blink ? "" : "|", Color.Green));
			return s;
		}

		public override void Draw(SpriteBatch batch)
		{
			if (Visible)
			{
				Main.blockInput = false;
				_Timer++;
				if (_Timer == 30)
				{
					_Blink = !_Blink;
					_Timer = 0;
				}
				batch.Draw(TextBoxEdge, DrawPosition, null, Color.White);
				int fillWidth = (int)Width - 2 * TextBoxEdge.Width;
				Vector2 pos = DrawPosition;
				pos.X += TextBoxEdge.Width;
				batch.Draw(TextboxFill, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(fillWidth, 1f), SpriteEffects.None, 0f);
				pos.X += fillWidth;
				batch.Draw(TextBoxEdge, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
				base.Draw(batch);//To draw the text
				if (Focused)
				{
					Main.blockInput = true;
					Terraria.GameInput.PlayerInput.WritingText = true;
					Main.instance.HandleIME();
					string oldText = Text;
					Text = _Limit(Main.fontItemStack, Main.GetInputText(Text), Width - _Spacing * 2);//necessary
					batch.End();
					batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
					Main.instance.DrawWindowsIMEPanel(new Vector2(98f, (float)(Main.screenHeight - 36)), 0f);
				}

			}
		}
	}
}
