using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUI.EventArgs
{
	public class OnMouseUpEventArgs : MouseEvent
	{
		public MouseButtons Button = MouseButtons.Left;
	}
}
