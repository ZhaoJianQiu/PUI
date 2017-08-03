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
	public sealed class ScrollBar : Container
	{
		private class ScrollSlider : Control
		{
			public static Texture2D ScrollbarSliderTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/ScrollbarSlider");
			public static Texture2D ScrollbarSliderFillTexture//from cheatsheet
			{
				get
				{
					Color[] array = new Color[ScrollbarSliderTexture.Width * ScrollbarSliderTexture.Height];
					ScrollbarSliderTexture.GetData(array);
					Color[] array2 = new Color[ScrollbarSliderTexture.Width];
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i] = array[i + (ScrollbarSliderTexture.Height - 1) * ScrollbarSliderTexture.Width];
					}
					var scrollbarFill = new Texture2D(Main.spriteBatch.GraphicsDevice, array2.Length, 1);
					scrollbarFill.SetData(array2);
					return scrollbarFill;
				}
			}
			public override void Draw(SpriteBatch batch)
			{
				base.Draw(batch);
				Vector2 drawPosition = DrawPosition;
				float num = (Height - (ScrollbarSliderTexture.Height * 2));
				float num2 = Width / ScrollbarSliderTexture.Width * _SliderWidthScale;
				drawPosition.X -= (_SliderWidthScale - 1f) / 2 * Width;//get the X

				batch.Draw(ScrollbarSliderTexture, drawPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(num2, 1f), SpriteEffects.None, 0f);
				drawPosition.Y += ScrollbarSliderTexture.Height;
				batch.Draw(ScrollbarSliderFillTexture, drawPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(num2, num), SpriteEffects.None, 0f);
				drawPosition.Y += num;
				batch.Draw(ScrollbarSliderTexture, drawPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(num2, 1f), SpriteEffects.FlipVertically, 0f);
			}
		}
		private const float _SliderWidthScale = 1.2f;
		private ScrollSlider Slider = new ScrollSlider();
		public static Texture2D ScrollbarBackgroundTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/ScrollbarBackground");
		private float _Speed = 0.3f;
		public float Speed
		{
			get => _Speed;
			set => _Speed = value;
		}
		private float _Unit = 0.2f;
		public float Unit
		{
			get => _Unit;
			set
			{
				if (value > 1f)
					_Unit = 1f;
				else if (value < 0.1f)
					_Unit = 0.1f;
				else
					_Unit = value;
			}
		}
		private float _Value = 0f;
		public float Value
		{
			get => _Value;
			set
			{
				if (value > 100f)
					_Value = 100f;
				else if (value < 0)
					_Value = 0;
				else
					_Value = value;
			}
		}

		public ScrollBar()
		{
			Slider.Position = new Vector2(0, 0);
			Controls.Add(Slider);
			OnMouseDown += ScrollBar_OnMouseDown;
			OnMouseUp += ScrollBar_OnMouseUp;
			Slider.OnMouseDown += Slider_OnMouseDown;
			Slider.OnMouseUp += Slider_OnMouseUp;
		}

		private void ScrollBar_OnMouseUp(object arg1, EventArgs.OnMouseUpEventArgs arg2)
		{
			_ScrollBar_Down = false;
		}

		private bool _ScrollBar_Down = false;
		private void ScrollBar_OnMouseDown(object arg1, EventArgs.OnMouseDownEventArgs arg2)
		{
			_ScrollBar_Down = true;
		}


		private void Slider_OnMouseUp(object arg1, EventArgs.OnMouseUpEventArgs arg2)
		{
			_Slider_Draging = false;
			_Slider_Mouse_Off = Vector2.Zero;
		}

		private bool _Slider_Draging = false;
		private Vector2 _Slider_Mouse_Off = Vector2.Zero;
		private void Slider_OnMouseDown(object arg1, EventArgs.OnMouseDownEventArgs arg2)
		{
			_Slider_Draging = true;
			_Slider_Mouse_Off = new Vector2(Main.mouseX, Main.mouseY) - Slider.DrawPosition;

		}

		private static Texture2D ScrollBackgroundFillTexture//from cheatsheet
		{
			get
			{
				Color[] array = new Color[ScrollbarBackgroundTexture.Width * ScrollbarBackgroundTexture.Height];
				ScrollbarBackgroundTexture.GetData(array);
				Color[] array2 = new Color[ScrollbarBackgroundTexture.Width];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = array[i + (ScrollbarBackgroundTexture.Height - 1) * ScrollbarBackgroundTexture.Width];
				}
				var scrollbgFill = new Texture2D(Main.spriteBatch.GraphicsDevice, array2.Length, 1);
				scrollbgFill.SetData(array2);
				return scrollbgFill;
			}
		}
		public override void Update()
		{
			base.Update();
			if (_Slider_Draging)
			{
				Vector2 sliderDrawPos = new Vector2(MouseState.X, MouseState.Y) - _Slider_Mouse_Off;
				float h = (sliderDrawPos - DrawPosition).Y;
				float t = Height - Slider.Height;
				Value = t == 0 ? 0f : h / (t / 100);
			}
			if (_ScrollBar_Down)
			{
				if (!Slider.Inside(MouseState.X, MouseState.Y))
				{
					if (MouseState.Y > Slider.DrawPosition.Y)
						Value += Speed;
					else if (MouseState.Y < Slider.DrawPosition.Y)
						Value -= Speed;
				}
			}
			float off = (Height - Slider.Height) / 100 * Value;
			Slider.Size = new Vector2(Width, Height * Unit);
			Slider.Position = new Vector2(0, off);
		}
		private void DrawBackground(SpriteBatch batch)
		{
			Vector2 drawPosition = DrawPosition;
			float num = Height - (ScrollbarBackgroundTexture.Height * 2);
			float num2 = Width / ScrollbarBackgroundTexture.Width;
			batch.Draw(ScrollbarBackgroundTexture, drawPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(num2, 1f), SpriteEffects.None, 0f);
			drawPosition.Y += ScrollbarBackgroundTexture.Height;
			batch.Draw(ScrollBackgroundFillTexture, drawPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(num2, num), SpriteEffects.None, 0f);
			drawPosition.Y += num;
			batch.Draw(ScrollbarBackgroundTexture, drawPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(num2, 1f), SpriteEffects.FlipVertically, 0f);
		}
		public override void Draw(SpriteBatch batch)
		{
			DrawBackground(batch);
			base.Draw(batch);
		}
	}
}
