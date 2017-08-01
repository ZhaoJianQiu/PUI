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
using Terraria.GameInput;

namespace PUI
{
	class Test
	{
		/*private static Window window1, itemsWindow;
		private static WindowManager wm = new WindowManager();
		static Test()
		{
			window1 = new Window(new Rectangle(20, 180, 280, 340))
			{
				Title = "Demo Window",
				BlockMouse = true,
			};
			TextBox tb = new TextBox(true)
			{
				Position = new Vector2(10, 40),
				Size = new Vector2(200, 90),
			};
			tb.Text = "";
			window1.Controls.Add(tb);


			itemsWindow = new Window(new Rectangle(300, 180, 240, 240))
			{
				Title = "Items Window",
				BlockMouse = true,
			};
			ImageBox ib = new ImageBox()
			{
				ToolTip = true,
				Position = new Vector2(10, 30),
				Size = new Vector2(215, 200),
				Column = 6,
			};
			for (int i = 1; i < Main.maxItemTypes; i++)
			{
				Item t = new Item();
				t.SetDefaults(i);
				ib.Items.Add(new ImageBox.ImageBoxItem(Main.itemTexture[i], t.HoverName));
			}
			itemsWindow.Controls.Add(ib);

			wm.Register(window1);
			wm.Register(itemsWindow);

			PHooks.Hooks.InterfaceLayersSetup.After += InterfaceLayersSetup_After;
			PHooks.Hooks.Update.After += Update_After;
			PHooks.Hooks.Update.Pre += Update_Pre;
		}


		private static bool Update_Pre(object[] arg)
		{
			return true;
		}

		private static void Update_After(object[] obj)
		{
			wm.Update();
		}

		private static void InterfaceLayersSetup_After(object[] obj)
		{
			int MouseTextIndex = Main.instance._gameInterfaceLayers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				Main.instance._gameInterfaceLayers.Insert(MouseTextIndex + 1, new LegacyGameInterfaceLayer(
					"Test: UI",
					delegate
					{
						wm.Draw(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}*/
	}
}
