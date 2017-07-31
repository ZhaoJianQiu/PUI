﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.UI.Chat;

namespace PUI
{
	public class ImageBox : Control
	{
		public class ImageBoxItem : Control
		{
			public Texture2D Texture
			{
				get;
				set;
			}
			public bool Hovered = false;
			private float _Padding_Scale = 0.2f;
			private string _ToolTip = "";
			public string ToolTip
			{
				get => _ToolTip;
				set => _ToolTip = value;
			}
			public ImageBoxItem(Texture2D Texture)
			{
				this.Texture = Texture;
				OnHover += ImageBoxItem_OnHover;
			}

			private void ImageBoxItem_OnHover(object arg1, EventArgs.MouseEvent arg2)
			{
				Hovered = true;
			}

			public ImageBoxItem(Texture2D Texture, string ToolTip) : this(Texture)
			{
				this.ToolTip = ToolTip;
			}
			public override void Update()
			{
				Hovered = false;
				base.Update();
			}
			private void DrawBackground(SpriteBatch batch)
			{
				batch.Draw(Main.inventoryBack9Texture, new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, (int)Size.X, (int)Size.Y), Color.White);
			}
			private void DrawContent(SpriteBatch batch)
			{
				Vector2 scale = (Size / Texture.Size()) * (1 - (2 * _Padding_Scale));
				batch.Draw(Texture, DrawPosition + new Vector2(Width, Height) * new Vector2(_Padding_Scale), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			}
			public override void Draw(SpriteBatch batch)
			{
				base.Draw(batch);
				DrawBackground(batch);
				DrawContent(batch);
				//ChatManager.DrawColorCodedStringWithShadow(batch, Main.fontMouseText, ToolTip, new Vector2(MouseState.X, MouseState.Y) + new Vector2(20, 20), Color.White, 0f, Vector2.Zero, Vector2.One);
			}
		}
		private int _Column = 5;
		public int Column
		{
			get => _Column;
			set => _Column = value;
		}
		private Container Content = new Container();
		private ScrollBar ScrollBar = new ScrollBar();

		private bool _Item_Hovered = false;
		private string _Hover_Text = "";
		private float _ScrollBar_Width = 15f, _Spacing = 1f;
		private List<ImageBoxItem> DrawingItems = new List<ImageBoxItem>();
		public List<ImageBoxItem> Items
		{
			get;
			private set;
		}
		public bool Selectable
		{
			get;
			set;
		}
		public bool ToolTip
		{
			get;
			set;
		}

		public ImageBox()
		{
			Items = new List<ImageBoxItem>();
			ScrollBar.AnchorPosition = AnchorPosition.TopRight;
			ScrollBar.Speed = 0.05f;
			Content.Controls.Add(ScrollBar);
		}

		public override void Update()
		{
			base.Update();
			{
				DrawingItems.Clear();

				float elementSize = (Width - _ScrollBar_Width - ((Column) * _Spacing)) / Column;
				int X = (int)Math.Floor((double)Width / elementSize);
				int Y = (int)Math.Floor((double)Height / elementSize);
				int items_Per_Page = X * Y;
				int begin = (int)Math.Floor(ScrollBar.Value / 100f * (Items.Count - items_Per_Page));
				int j = 0;
				for (int i = begin; i < Items.Count; i++)
				{
					DrawingItems.Add(Items[i]);
					j++;
					if (j == items_Per_Page) break;
				}
			}
			for (int i = 0; i < DrawingItems.Count; i++)
			{
				DrawingItems[i].Update();
			}
			_Item_Hovered = false;
			Content.Update();
		}
		private void DrawBackground(SpriteBatch batch)
		{
			Utils.DrawInvBG(batch, DrawPosition.X, DrawPosition.Y, Width, Height, Window.WindowBackground);
		}
		private static void DrawSizedTexture(SpriteBatch batch, Texture2D texture, Color color, Vector2 pos, Vector2 size)
		{
			Vector2 tSize = texture.Size();
			Vector2 scale = size / tSize;
			batch.Draw(texture, pos, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
		}
		private void DrawElements(SpriteBatch batch)
		{
			if (DrawingItems.Count > 0)
			{
				float elementSize = (Width - _ScrollBar_Width - ((Column) * _Spacing)) / Column;
				for (int i = 0; i < DrawingItems.Count; i++)
				{
					int iX = i % Column;
					int iY = (int)Math.Floor(((double)i / Column));
					Vector2 rPos = new Vector2(iX, iY) * (_Spacing + elementSize);
					Vector2 dPos = DrawPosition + rPos;
					dPos.X += 1f;
					DrawingItems[i].Position = dPos;
					DrawingItems[i].Size = new Vector2(elementSize, elementSize);
					DrawingItems[i].Draw(batch);
					if (DrawingItems[i].Hovered)
					{
						_Hover_Text = DrawingItems[i].ToolTip;
						_Item_Hovered = true;
					}
				}
			}
		}
		public override void Draw(SpriteBatch batch)
		{
			base.Draw(batch);
			Content.Size = new Vector2(Width, Height);
			Content.Position = DrawPosition;

			ScrollBar.Position = new Vector2(0, 0);
			ScrollBar.Size = new Vector2(_ScrollBar_Width, Height);

			DrawBackground(batch);
			Content.Draw(batch);
			DrawElements(batch);
			if (_Item_Hovered)
				ChatManager.DrawColorCodedStringWithShadow(batch, Main.fontMouseText, _Hover_Text, new Vector2(MouseState.X, MouseState.Y) + new Vector2(20, 20), Color.White, 0f, Vector2.Zero, Vector2.One);
		}
	}
}