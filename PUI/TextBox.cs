using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;

namespace PUI
{
	public class TextBox : Label
	{
		public static Texture2D TextBoxEdge = Main.instance.OurLoad<Texture2D>("Qiu/UI/TextboxEdge");
		private long _Timer = 0;
		private bool _Blink = false;
		private ScrollBar ScrollBar = new ScrollBar();
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

		public event Action<object, EventArgs.EventArgs> OnEnterPress;

		public TextBox(string Text, bool MultiLine) : base(Text)
		{
			this.MultiLine = MultiLine;
			if (MultiLine)
				_PaddingRight = 10f;
			Focusable = true;
			OnClick += TextBox_OnClick;
			OnInput += TextBox_OnInput;
			OnEnterPress += TextBox_OnEnterPress;
		}

		private void TextBox_OnEnterPress(object arg1, EventArgs.EventArgs arg2)
		{
			if (MultiLine)
				Text += "\n";
		}

		private void TextBox_OnInput(object arg1, EventArgs.EventArgs arg2)
		{
			ScrollBar.Value = 100f;
		}

		private void TextBox_OnClick(object arg1, EventArgs.OnClickEventArgs arg2)
		{
			Focused = true;
		}

		public TextBox(bool MultiLine) : this("", MultiLine)
		{

		}
		public override void Update()
		{
			base.Update();
			ScrollBar.Update();
		}
		private void DrawEdge(SpriteBatch batch)
		{
			float fillWidth = Width - 2 * TextBoxEdge.Width;
			float h = Height / TextBoxEdge.Height;
			Vector2 pos = DrawPosition;
			pos.Y -= 1f;
			batch.Draw(TextBoxEdge, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(1f, h), SpriteEffects.None, 0f);
			pos.X += TextBoxEdge.Width;
			batch.Draw(TextboxFill, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(fillWidth, h), SpriteEffects.None, 0f);
			pos.X += fillWidth;
			batch.Draw(TextBoxEdge, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(1f, h), SpriteEffects.FlipHorizontally, 0f);
		}
		public event Action<object, EventArgs.EventArgs> OnInput;
		private void Input_Handle(SpriteBatch batch)
		{
			if (Focused)
			{
				Main.drawingPlayerChat = false;
				Main.chatRelease = true;
				Main.blockInput = true;
				Terraria.GameInput.PlayerInput.WritingText = true;
				if (KeyboardState.IsKeyDown(Keys.Enter) && !LastKeyboardState.IsKeyDown(Keys.Enter))
				{
					OnEnterPress?.Invoke(this, new EventArgs.EventArgs());
				}
				Main.instance.HandleIME();
				string oldText = Text;
				Text = Main.GetInputText(Text);
				if (oldText != Text)
				{
					OnInput?.Invoke(this, new EventArgs.EventArgs());
				}
				batch.End();
				batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
				Main.instance.DrawWindowsIMEPanel(new Vector2(98f, (float)(Main.screenHeight - 36)), 0f);
			}
		}
		private void DrawScrollBar(SpriteBatch batch)
		{
			ScrollBar.Size = new Vector2(_PaddingRight, Height);
			ScrollBar.Position = DrawPosition + new Vector2(Width - _PaddingRight, 0);
			ScrollBar.Draw(batch);
		}
		protected override void DrawString(SpriteBatch batch, List<List<TextSnippet>> text)
		{
			float height_Text = ChatManager.GetStringSize(SpriteFont, "[i:3063]", Vector2.One).Y;
			float height_Per_Line = height_Text + TextVerticalPadding * 2;
			if (Height < height_Per_Line)
				return;
			int lines_Per_Page = (int)Math.Floor((double)Height / height_Per_Line);
			int pages = (int)Math.Ceiling((double)text.Count / lines_Per_Page);
			int begin = 0;
			if (text.Count > lines_Per_Page)
			{
				begin = (int)Math.Round(ScrollBar.Value / 100f * (text.Count - lines_Per_Page));
			}
			if (MultiLine)
			{
				if (Focused)
					text.Last().Add(new TextSnippet(_Blink ? "|" : "", Color.Black));
				int j = 0;
				for (int i = begin; i < text.Count; i++)
				{
					var line = text[i];
					var dPos = DrawPosition;
					dPos.Y += height_Per_Line * j;
					dPos.Y += height_Per_Line / 2 - height_Text / 2;
					dPos.X += _PaddingLeft;
					if (dPos.Y + height_Per_Line <= DrawPosition.Y + Height)
						ChatManager.DrawColorCodedStringWithShadow(batch, SpriteFont, line.ToArray(), dPos, 0f, Vector2.Zero, Vector2.One, out var hovered);
					j++;
				}
			}
			else
			{
				Vector2 dPos = DrawPosition + new Vector2(_PaddingLeft, Height / 2 - height_Per_Line / 2);
				Text = string.Concat(from s in text[0] select s.TextOriginal);//limit
				if (Focused)
					text[0].Add(new TextSnippet(_Blink ? "|" : "", Color.Black));

				ChatManager.DrawColorCodedStringWithShadow(batch, SpriteFont, text[0].ToArray(), dPos, 0f, Vector2.Zero, Vector2.One, out var hovered);
			}
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
				DrawEdge(batch);
				base.Draw(batch);
				Input_Handle(batch);
				if (MultiLine)
					DrawScrollBar(batch);
			}
		}
	}
}
