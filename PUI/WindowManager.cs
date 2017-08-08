using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PUI.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace PUI
{
	public class WindowManager
	{
		public int MaxWindows = 16;
		private List<Window> _Windows = new List<Window>();
		private List<Window> Windows
		{
			get => _Windows;
			set => _Windows = value;
		}
		public void Draw(SpriteBatch batch)
		{
			lock (Windows)
			{
				for (int i = 0; i < Windows.Count; i++)
				{
					var w = Windows[i];
					if (w.Visible && !w.Minimized)
						w.Draw(batch);
				}
			}
		}
		private bool _Down(ButtonState s)
		{
			return s == ButtonState.Pressed;
		}
		public void Update()
		{
			lock (Windows)
			{
				for (int i = 0; i < Windows.Count; i++)
				{
					var w = Windows[i];
					if (!w.Minimized)
						w.Update();
				}
			}
		}
		private Window WindowAt(Vector2 mouse)
		{
			lock (Windows)
			{
				for (int i = 0; i < Windows.Count; i++)
				{
					var W = Windows[Windows.Count - i - 1];
					if (W.Inside(mouse.X, mouse.Y))
						return W;
				}
			}
			return null;
		}
		private void FocusWindow(Window instance)//Move to last so that it won't be covered
		{
			lock (Windows)
			{
				if (Windows.Remove(instance))
				{
					for (int i = 0; i < Windows.Count; i++)
					{
						Windows[i].Focused = false;
					}
					instance.Focused = true;
					Windows.Add(instance);
				}
			}
		}
		public void Register(Window instance)
		{
			lock (Windows)
			{
				if (instance == null) return;
				if (Windows.Count >= MaxWindows) return;

				for (int i = 0; i < Windows.Count; i++)
				{
					var w = Windows[i];
					var v = instance.DrawPosition + new Vector2(5);
					if (w.Inside(v.X, v.Y))
					{
						instance.Position = new Vector2(v.X + 15, v.Y + 15);
					}
				}
				instance.WindowManager = this;
				instance.OnClosing += Instance_OnClosing;
				instance.OnMouseDown += Instance_OnMouseDown;
				Windows.Add(instance);
			}
		}

		private void Instance_OnClosing(object arg1, EventArgs.EventArgs arg2)
		{
			DisposeWindow((Window)arg1);
		}

		private void Instance_OnMouseDown(object arg1, OnMouseDownEventArgs arg2)
		{
			//FocusWindow((Window)arg1);
			if (WindowAt(new Vector2(Main.mouseX, Main.mouseY)) == arg1)
			{
				FocusWindow((Window)arg1);
			}
		}


		public bool DisposeWindow(Window instance)
		{
			if (instance == null) return false;

			lock (Windows)
			{
				if (Windows.Remove(instance))
				{
					instance.WindowManager = null;
					instance.OnMouseDown -= Instance_OnMouseDown;
					return true;
				}
			}
			return false;
		}
	}
}
