using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PUI
{
	public class Container : Control
	{
		public ControlsList Controls
		{
			get;
			private set;
		}

		public Container()
		{
			Controls = new ControlsList();
			Controls.OnAdd += Controls_OnAdd;
			Controls.OnRemove += Controls_OnRemove;
		}

		private void Controls_OnRemove(Control item)
		{
			item.Parent = null;
		}

		private void Controls_OnAdd(Control item)
		{
			item.Parent = this;
		}

		public Control ControlAt(Vector2 coor) => ControlAt(coor.X, coor.Y);

		public Control ControlAt(float X, float Y)
		{
			if (!Inside(X, Y)) return null;
			for (int i = 0; i < Controls.Count; i++)
			{
				var control = Controls[Controls.Count - i - 1];

				if (control.Inside(X, Y))
				{
					if (control is Container)
					{
						return ((Container)control).ControlAt(X, Y);
					}
					return control;
				}
			}
			return this;
		}

		public override void Draw(SpriteBatch batch)
		{
			base.Draw(batch);
			if (Visible)
				for (int i = 0; i < Controls.Count; i++)
					Controls[i].Draw(batch);
		}

		public override void Update()
		{
			base.Update();
			foreach (var c in Controls)
				for (int i = 0; i < Controls.Count; i++)
					Controls[i].Update();
		}
		public class ControlsList : IList<Control>
		{
			private List<Control> Value = new List<Control>();
			public Control this[int index]
			{
				get => Value[index];
				set
				{
					return;
				}
			}

			public int Count => Value.Count;

			public bool IsReadOnly => false;

			internal event Action<Control> OnAdd = null;
			internal event Action<Control> OnRemove = null;

			public void Add(Control item)
			{
				OnAdd?.Invoke(item);
				Value.Add(item);
			}


			public void Clear() => Value.Clear();

			public bool Contains(Control item) => Value.Contains(item);


			public void CopyTo(Control[] array, int arrayIndex) => Value.CopyTo(array, arrayIndex);


			public IEnumerator<Control> GetEnumerator() => Value.GetEnumerator();

			public int IndexOf(Control item) => Value.IndexOf(item);


			public void Insert(int index, Control item) => Value.Insert(index, item);


			public bool Remove(Control item)
			{
				if (Value.Remove(item))
				{
					OnRemove?.Invoke(item);
					return true;
				}
				return false;
			}


			public void RemoveAt(int index)
			{
				OnRemove?.Invoke(Value[index]);
				Value.RemoveAt(index);
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		}
	}

}
