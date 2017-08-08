using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PUI.EventArgs;
using Terraria;

namespace PUI
{
	public class ItemListView : ListView<Label>
	{
		private List<Label> Items = new List<Label>();
		private Container Content = new Container();
		private Container LabelContent = new Container();
		private ScrollBar ScrollBar = new ScrollBar();
		private bool _Hovered = false;
		private int _Hovered_Index = 0;
		private float _ScrollBarWidth = 15f;
		public event Action<Label, OnClickEventArgs> OnItemClick;
		public Label this[int k]
		{
			get => Items[k];
		}
		private float _ItemHeight = 30f;
		public float ItemHeight
		{
			get => _ItemHeight;
			set => _ItemHeight = value;
		}
		public ItemListView()
		{
			ScrollBar.AnchorPosition = AnchorPosition.TopRight;
			ScrollBar.Position = new Vector2(0, 0);
			LabelContent.OnMouseWheel += LabelContent_OnMouseWheel;
			Content.Controls.Add(ScrollBar);
			Content.Controls.Add(LabelContent);
		}

		private void LabelContent_OnMouseWheel(object arg1, OnMouseWheelEventArgs arg2)
		{
			ScrollBar.Value -= arg2.Value / 120;
		}

		private void ReRank()
		{
			LabelContent.Controls.Clear();
			float maxHeight = _ItemHeight * Items.Count;
			int items_Per_Page = (int)Math.Floor((double)Height / _ItemHeight);
			int begin = 0;
			if (Items.Count > items_Per_Page)
			{
				ScrollBar.Unit = 1f - (float)(Items.Count - items_Per_Page) / Items.Count;
				begin = (int)Math.Floor((ScrollBar.Value / 100f) * (Items.Count - items_Per_Page));
			}
			else
			{
				ScrollBar.Unit = 1f;
			}
			int j = 0;
			for (int i = begin; i < Items.Count; i++)
			{
				Items[i].Position = new Vector2(0, j * _ItemHeight);
				Items[i].Size = new Vector2(LabelContent.Width, _ItemHeight);
				LabelContent.Controls.Add(Items[i]);
				j++;
				if (j == items_Per_Page) break;
			}
		}
		public override void Update()
		{
			_Hovered = false;
			base.Update();
			ReRank();
			Content.Update();
		}
		private void DrawBackground(SpriteBatch batch)
		{
			Utils.DrawInvBG(batch, DrawPosition.X, DrawPosition.Y, Width, Height, Window.WindowBackground);
			for (int i = 0; i < LabelContent.Controls.Count; i++)
			{
				Utils.DrawInvBG(batch, LabelContent.Controls[i].DrawPosition.X, LabelContent.Controls[i].DrawPosition.Y, LabelContent.Controls[i].Width, LabelContent.Controls[i].Height, Window.WindowBackground);
			}
			if (_Hovered)
			{
				if (_Hovered_Index < LabelContent.Controls.Count)
				{
					try//sometimes it causes crashing,i don't know why it did it
					{
						batch.Draw(Main.inventoryBack9Texture,
							new Rectangle((int)LabelContent.Controls[_Hovered_Index].DrawPosition.X,
							(int)LabelContent.Controls[_Hovered_Index].DrawPosition.Y,
							(int)LabelContent.Controls[_Hovered_Index].Width,
							(int)LabelContent.Controls[_Hovered_Index].Height),
							Color.White);
					}
					catch (Exception e)
					{
						e.GetBaseException();
					}
				}
			}
		}
		public override void Draw(SpriteBatch batch)
		{
			base.Draw(batch);
			ScrollBar.Size = new Vector2(_ScrollBarWidth, Height);
			Content.Position = DrawPosition;
			Content.Size = Size;
			LabelContent.Size = new Vector2(Content.Width - _ScrollBarWidth, Content.Height);
			DrawBackground(batch);
			Content.Draw(batch);
		}

		public override void Add(Label item)
		{
			Items.Add(item);
			item.OnClick += Item_OnClick;
			item.OnHover += Item_OnHover;
		}

		private void Item_OnHover(object arg1, MouseEvent arg2)
		{
			_Hovered = true;
			float rH = MouseState.Y - DrawPosition.Y;
			_Hovered_Index = (int)(Math.Floor((double)rH / ItemHeight));
		}

		private void Item_OnClick(object sender, OnClickEventArgs cea)
		{
			OnItemClick?.Invoke((Label)sender, cea);
		}

		public override bool Remove(Label item)
		{
			if (Items.Contains(item))
			{
				item.OnClick -= Item_OnClick;
				item.OnHover -= Item_OnHover;
				return Items.Remove(item);
			}
			return false;
		}

		public override void Add(IEnumerable<Label> items)
		{
			foreach (var i in items)
				Items.Add(i);
		}

		public override Label ElementAt(int index)
		{
			return this[index];
		}
	}
}
