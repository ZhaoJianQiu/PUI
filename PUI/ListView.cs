using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PUI.EventArgs;

namespace PUI
{
	public abstract class ListView<T> : Control
		where T : Control
	{
		public abstract void Add(T item);
		public abstract void Add(IEnumerable<T> items);
		public abstract bool Remove(T item);
		public abstract T ElementAt(int index);
	}
}
