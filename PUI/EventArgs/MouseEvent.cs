using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUI.EventArgs
{
	public enum MouseButtons
	{
		Left, Right
	}
	public class MouseEvent : EventArgs
	{
		public Vector2 Position = Vector2.Zero;

		public MouseEvent(Vector2 pos, bool handled = false)
		{
			Position = pos;
			Handled = handled;
		}
		public MouseEvent(float x = 0, float y = 0, bool handled = false)
		{
			Position = new Vector2(x, y);
			Handled = handled;
		}
	}
}
