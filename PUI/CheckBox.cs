﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace PUI
{
	public class CheckBox : Control
	{
		public static Texture2D CheckBoxTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/CheckBox");
		public static Texture2D CheckBoxMarkedTexture = Main.instance.OurLoad<Texture2D>("Qiu/UI/CheckMark");

		private Container Content = new Container();
		private Image CheckState = new Image(CheckBoxTexture);
		private Label CheckText = new Label();

		public string Text
		{
			get => CheckText.Text;
			set => CheckText.Text = value;
		}

		public CheckBox(string Text)
		{
			this.Text = Text;

			Content.OnClick += Content_OnClick;
			CheckState.OnClick += CheckState_OnClick1;
			Content.Controls.Add(CheckState);
			Content.Controls.Add(CheckText);
		}

		private void CheckState_OnClick1(object arg1, EventArgs.OnClickEventArgs arg2)
		{
			Main.NewText("abcd");
		}

		private void Content_OnClick(object arg1, EventArgs.OnClickEventArgs arg2)
		{
			Checked = !Checked;
		}

		private void CheckState_OnClick(object arg1, EventArgs.OnClickEventArgs arg2)
		{
		}

		public CheckBox() : this("")
		{

		}

		private bool _Checked = false;
		public bool Checked
		{
			get => _Checked;
			set
			{
				_Checked = value;
			}
		}
		public override void Update()
		{
			Content.Update();
			base.Update();
		}
		public override void Draw(SpriteBatch batch)
		{
			Content.Size = Size;
			Content.Position = DrawPosition;
			CheckState.Position = new Vector2(0, 0);
			CheckText.Position = new Vector2(CheckState.Width, 0);
			CheckText.Size = new Vector2(Content.Width - CheckText.X, Content.Height);
			Content.Draw(batch);
			base.Draw(batch);
			if(Checked)
				batch.Draw(CheckBoxMarkedTexture, CheckState.DrawPosition, Color.White);
		}
	}
}
