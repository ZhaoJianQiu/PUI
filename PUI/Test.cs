using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using PUI.EventArgs;

namespace PUI
{
	/// <summary>
	/// have a simple test
	/// it displayed a window that with some controls
	/// close button(built-in)
	/// title(built-in)
	/// textbox(added)
	/// scrollbar(useless, shows how it works)
	/// </summary>
	class Test
	{
		static Window w;
		static Test()
		{
			w = new Window(new Rectangle(200, 300, 200, 200))
			{
				Title = "Title",
				BlockMouse = true,
			};
			TextBox tb = new TextBox()
			{
				Position = new Vector2(0, 40),
				Size = new Vector2(150, 30)
			};
			ScrollBar ss = new ScrollBar();
			ss.AnchorPosition = AnchorPosition.TopRight;
			ss.Position = new Vector2(10, 30);
			ss.Size = new Vector2(15, 170);
			ss.Value = 100f;
			w.Controls.Add(tb);
			w.Controls.Add(ss);
			PHooks.Hooks.InterfaceLayersSetup.After += InterfaceLayersSetup_After;
		}

		private static void InterfaceLayersSetup_After(object[] obj)
		{
			int MouseTextIndex = Main.instance._gameInterfaceLayers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				Main.instance._gameInterfaceLayers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"Test: UI",
					delegate
					{
						w.Update();
						w.Draw(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
