using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUI
{
	public class WindowManager
	{
		private List<Window> _Windows = new List<Window>();
		public List<Window> Windows
		{
			get=>_Windows;
			private set
			{
				_Windows = value;
			}
		}
		public void Draw(SpriteBatch batch)
		{
			foreach (var w in Windows)
			{
				if (w.Visible && !w.Minimized)
					w.Draw(batch);
			}
		}
		public void Update()
		{
			foreach (var w in Windows)
			{
				if (!w.Minimized)
					w.Update();
			}
		}
		public void Register(Window instance)
		{
			Windows.Add(instance);
		}
		public bool DisposeWindow(Window instance)
		{
			return Windows.Remove(instance);
		}
	}
}
