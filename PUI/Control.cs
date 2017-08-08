using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using PUI.EventArgs;
using Terraria.GameInput;

namespace PUI
{
	public abstract class Control
	{
		public string ToolTip
		{
			get;
			set;
		}
		private bool _Down_Left = false, _Down_Right = false;
		private bool _LastInside = false;
		private bool _Visible = true;
		private bool _BlockMouse = true;
		public bool BlockMouse
		{
			get => _BlockMouse;
			set => _BlockMouse = value;
		}
		public AnchorPosition AnchorPosition
		{
			get;
			set;
		}
		protected MouseState LastMouseState = Mouse.GetState(), MouseState = Mouse.GetState();
		protected KeyboardState LastKeyboardState = Keyboard.GetState(), KeyboardState = Keyboard.GetState();
		private Container _Parent = null;
		public virtual Container Parent
		{
			get => _Parent;
			internal set
			{
				_Parent = value;
			}
		}
		private bool _Focused;
		public bool Focused
		{
			get => _Focused;
			set
			{
				var c = this;
				while (c.Parent != null)
				{
					c = c.Parent;
				}
				if (c is Container)
					UnfocuseAll((Container)c);
				if (Focusable)
				{
					_Focused = value;
					if (_Focused)
						OnFocus?.Invoke(this, new EventArgs.EventArgs());
					else
						OnFocusLost?.Invoke(this, new EventArgs.EventArgs());
				}
			}
		}
		public bool Focusable
		{
			get;
			protected set;
		}
		private static void UnfocuseAll(Container c)
		{
			foreach (var a in c.Controls)
			{
				if (a is Container)
				{
					UnfocuseAll((Container)a);
				}
				else
				{
					a._Focused = false;
				}
			}
		}
		public bool Visible
		{
			get => _Visible;
			set => _Visible = value;
		}
		public Vector2 Position
		{
			get;
			set;
		}
		public virtual Vector2 Size
		{
			get;
			set;
		}
		public string Name { get; set; }

		public float X
		{
			get => Position.X;
		}
		public float Y
		{
			get => Position.Y;
		}

		public float Width
		{
			get => Size.X;
		}
		public float Height
		{
			get => Size.Y;
		}

		public Vector2 DrawPosition
		{
			get => GetDrawPosition();
		}

		private Vector2 GetDrawPosition()
		{
			if (Parent == null)
			{
				return new Vector2(X, Y);
			}
			return new Vector2(Parent.DrawPosition.X, Parent.DrawPosition.Y) + Anchor_Calc(AnchorPosition, Parent.Size, Size, Position);
		}

		private static Vector2 Anchor_Calc(AnchorPosition anchor, Vector2 pw, Vector2 sw, Vector2 dis)
		{
			switch (anchor)
			{
				case AnchorPosition.TopLeft:
					return dis;
				case AnchorPosition.TopRight:
					return new Vector2(pw.X - dis.X - sw.X, dis.Y);
				case AnchorPosition.BottomLeft:
					return new Vector2(dis.X, pw.Y - dis.Y - sw.Y);
				case AnchorPosition.BottomRight:
					return pw - dis - sw;
			}
			return dis;
		}


		public event Action<object, OnClickEventArgs> OnClick;
		public event Action<object, OnMouseDownEventArgs> OnMouseDown = (s, e) =>
		{
			Control c = s as Control;
			if (e.Button == MouseButtons.Left) c._Down_Left = true;
			else c._Down_Right = true;
		};
		public event Action<object, OnMouseUpEventArgs> OnMouseUp = (s, e) =>
		{
			Control c = s as Control;
			if (e.Button == MouseButtons.Left) c._Down_Left = false;
			else c._Down_Right = false;
		};
		public event Action<object, MouseEvent> OnHover;
		public event Action<object, OnMouseWheelEventArgs> OnMouseWheel;
		public event Action<object, OnMouseEnterEventArgs> OnMouseEnter;
		public event Action<object, OnMouseLeaveEventArgs> OnMouseLeave;
		public event Action<object, EventArgs.EventArgs> OnFocus;
		public event Action<object, EventArgs.EventArgs> OnFocusLost;

		private void Click_Pass(OnClickEventArgs cea)
		{
			OnClick?.Invoke(this, cea);
		}

		private void Up_Pass(OnMouseUpEventArgs mue)
		{
			OnMouseUp?.Invoke(this, mue);
			if (Parent != null)
			{
				if (Parent.Inside(MouseState.X, MouseState.Y) && !mue.Handled)
				{
					Parent.Up_Pass(mue);
				}
			}
		}
		private void Down_Pass(OnMouseDownEventArgs mde)
		{
			OnMouseDown?.Invoke(this, mde);
			if (Parent != null)
			{
				if (Parent.Inside(MouseState.X, MouseState.Y) && !mde.Handled)
				{
					Parent.Down_Pass(mde);
				}
			}
		}

		private void Wheel_Pass(OnMouseWheelEventArgs mwe)
		{
			OnMouseWheel?.Invoke(this, mwe);
			if (Parent != null)
			{
				if (Parent.Inside(MouseState.X, MouseState.Y) && !mwe.Handled)
				{
					Parent.Wheel_Pass(mwe);
				}
			}
		}

		public virtual void Update()
		{
			LastMouseState = MouseState;
			MouseState = Mouse.GetState();

			LastKeyboardState = KeyboardState;
			KeyboardState = Keyboard.GetState();

			if ((ButtonPressed(MouseState.LeftButton) && !ButtonPressed(LastMouseState.LeftButton)) || (ButtonPressed(MouseState.RightButton) && !ButtonPressed(LastMouseState.RightButton)) && Main.hasFocus)
			{
				var c = this;
				while (c.Parent != null)
				{
					c = c.Parent;
				}
				if (c is Container)
					UnfocuseAll((Container)c);
			}
			if ((!ButtonPressed(MouseState.LeftButton) && ButtonPressed(LastMouseState.LeftButton)) && Main.hasFocus)
			{
				if (_Down_Left)
				{
					Up_Pass(new OnMouseUpEventArgs() { Position = new Vector2(MouseState.X, MouseState.Y) });
					Click_Pass(new OnClickEventArgs() { Position = new Vector2(MouseState.X, MouseState.Y) });
				}
			}
			else if ((!ButtonPressed(MouseState.RightButton) && ButtonPressed(LastMouseState.RightButton)) && Main.hasFocus)
			{
				if (_Down_Right)
				{

					Up_Pass(new OnMouseUpEventArgs() { Position = new Vector2(MouseState.X, MouseState.Y), Button = MouseButtons.Right });
					Click_Pass(new OnClickEventArgs() { Position = new Vector2(MouseState.X, MouseState.Y), Button = MouseButtons.Right });
				}
			}

			if (!Inside(MouseState.X, MouseState.Y) && Main.hasFocus)
			{
				//Leave
				if (_LastInside)
				{
					OnMouseLeaveEventArgs mle = new OnMouseLeaveEventArgs() { Position = new Vector2(MouseState.X, MouseState.Y) };
					OnMouseLeave?.Invoke(this, mle);
				}
			}
			else
			{
				if (!_LastInside)
				{
					//Enter
					OnMouseEnterEventArgs mee = new OnMouseEnterEventArgs() { Position = new Vector2(MouseState.X, MouseState.Y) };
					OnMouseEnter?.Invoke(this, mee);
				}
			}


			if (this is Container)
			{
				if (((Container)this).ControlAt(MouseState.X, MouseState.Y) == this)
					CheckMouseEvent();
			}
			else
			{
				CheckMouseEvent();
			}
			_LastInside = Inside(MouseState.X, MouseState.Y);//上一个状态 标识鼠标是否在控件内
		}

		public virtual void Draw(SpriteBatch batch)
		{
			if (Inside(MouseState.X, MouseState.Y))
			{
				Main.LocalPlayer.mouseInterface = BlockMouse;
			}
		}

		private bool ButtonPressed(ButtonState bs)
		{
			return bs == ButtonState.Pressed;
		}


		private void CheckMouseEvent()
		{
			int mouseX = MouseState.X;
			int mouseY = MouseState.Y;

			if (Inside(mouseX, mouseY) && Main.hasFocus)
			{
				OnHoverEventArgs mhe = new OnHoverEventArgs() { Position = new Vector2(mouseX, mouseY) };
				OnHover?.Invoke(this, mhe);
				if (Parent != null)//存在父控件
				{
					if (Parent.Inside(mouseX, mouseY) && !mhe.Handled)//鼠标在父控件内，事件消息并未被处理
					{
						Parent.OnHover?.Invoke(Parent, mhe);
					}
				}
				if (MouseState.ScrollWheelValue != LastMouseState.ScrollWheelValue)
				{
					OnMouseWheelEventArgs mwe = new OnMouseWheelEventArgs() { Position = new Vector2(mouseX, mouseY), Value = MouseState.ScrollWheelValue - LastMouseState.ScrollWheelValue };
					Wheel_Pass(mwe);
				}
				if (ButtonPressed(MouseState.LeftButton) && !ButtonPressed(LastMouseState.LeftButton))
				{
					OnMouseDownEventArgs mde = new OnMouseDownEventArgs() { Position = new Vector2(mouseX, mouseY) };
					Down_Pass(mde);
				}
				if (ButtonPressed(MouseState.RightButton) && !ButtonPressed(LastMouseState.RightButton))
				{
					OnMouseDownEventArgs mde = new OnMouseDownEventArgs() { Position = new Vector2(mouseX, mouseY), Button = MouseButtons.Right };
					Down_Pass(mde);
				}
			}
		}

		public bool Inside(float X, float Y)
		{
			return (X >= DrawPosition.X && X <= DrawPosition.X + Width && Y >= DrawPosition.Y && Y <= DrawPosition.Y + Height);
		}
	}
}
