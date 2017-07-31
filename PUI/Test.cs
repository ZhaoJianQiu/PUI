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
			w = new Window(new Rectangle(20, 180, 340, 340))
			{
				Title = "Title",
				BlockMouse = true,
			};
			TextBox tb = new TextBox()
			{
				Position = new Vector2(10, 40),
				Size = new Vector2(200, 30)
			};
			CheckBox cb = new CheckBox("CheckBox")
			{
				Position = new Vector2(10, 80),
				Size = new Vector2(150, 30),
			};
			ImageBox ib = new ImageBox()
			{
				Position = new Vector2(10, 120),
				Size = new Vector2(260, 200),
				Column = 6,
			};
			for (int i = 1; i < Main.maxItemTypes; i++)
			{
				Item t = new Item();
				t.SetDefaults(i);
				ib.Items.Add(new ImageBox.ImageBoxItem(Main.itemTexture[i], t.HoverName));
			}
			w.Controls.Add(tb);
			w.Controls.Add(cb);
			w.Controls.Add(ib);
			PHooks.Hooks.InterfaceLayersSetup.After += InterfaceLayersSetup_After;
			PHooks.Hooks.Update.After += Update_After;
		}

		private static void Update_After(object[] obj)
		{
			w.Update();
		}

		private static void InterfaceLayersSetup_After(object[] obj)
		{
			int MouseTextIndex = Main.instance._gameInterfaceLayers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				Main.instance._gameInterfaceLayers.Insert(MouseTextIndex+1, new LegacyGameInterfaceLayer(
					"Test: UI",
					delegate
					{
						w.Draw(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
