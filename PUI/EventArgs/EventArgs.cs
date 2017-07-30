using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUI.EventArgs
{
	public class EventArgs
	{
		public bool Handled = false;//标识该事件是否已经被处理，如果被处理了，父控件将不会收到此消息
	}
}
