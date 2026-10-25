using Godot;

namespace MainNameSpace
{

//Table Input
	public partial class InputTables : Control
	{
		VBoxContainer funcUi;

		VBoxContainer tabl;
		public int colPerm = 17;
		public int colOgr = 16;

		RichTextLabel[] xLine = new RichTextLabel[17];
		public DrobUi[,] drobsTabl = new DrobUi[16, 17];
		HBoxContainer[] linesOfXs = new HBoxContainer[16];

		RichTextLabel[] funchXLine = new RichTextLabel[17];
		public DrobUi[] func = new DrobUi[17];



		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			tabl = GetNodeOrNull<VBoxContainer>("%Tabl");
			funcUi = GetNodeOrNull<VBoxContainer>("%Function");

			if (tabl == null || funcUi == null)
			{
				OS.Alert("ПрАизоЩла критичская оЩибка аАаАа!!!!\n Ctical eroor was hapend! aaaa!!!!");
				GD.Print("ПрАизоЩла критичская оЩибка аАаАа!!!!\n Ctical eroor was hapend! aaaa!!!!");
				GetTree().Quit(-1);
			}

			PackedScene drob_scene = ResourceLoader.Load<PackedScene>("res://DrobUi.tscn");

			tabl.AddChild(GenXLine(xLine));

			HBoxContainer generdFuncXLine = GenXLine(funchXLine);

			Label otstup = new Label();
			otstup.CustomMinimumSize = new Vector2(70, 0);
			generdFuncXLine.AddChild(otstup);
			generdFuncXLine.MoveChild(otstup, 0);

			funcUi.AddChild(generdFuncXLine);
			funcUi.MoveChild(generdFuncXLine, 0);

			HBoxContainer funcLine = new HBoxContainer();
			for (int j = 0; j < 17; j++)
			{
				DrobUi newDrob = drob_scene.Instantiate<DrobUi>();
				func[j] = newDrob;
				newDrob.X_J = j;
				newDrob.X_I = -1;
				//newDrob.XLabelViseble = true;
				funcLine.AddChild(newDrob);
			}
			funcUi.GetNode("FuncXLine").AddChild(funcLine);

			for (int i = 0; i < 16; i++)
			{
				HBoxContainer line = new HBoxContainer();
				linesOfXs[i] = line;
				for (int j = 0; j < 17; j++)
				{
					DrobUi newDrob = drob_scene.Instantiate<DrobUi>();
					drobsTabl[i, j] = newDrob;
					newDrob.X_J = j;
					newDrob.X_I = i;
					//newDrob.XLabelViseble = true;
					line.AddChild(newDrob);
				}
				tabl.AddChild(line);
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			//GD.Print("Hello from C# to Godot :)");

		}

		public void CountOgrValueChanged(float newColOgr)
		{
			colOgr = (int)newColOgr;
			for (int i = 0; i < colOgr; i++)
			{
				linesOfXs[i].Visible = true;
			}
			for (int i = colOgr; i < 16; i++)
			{
				linesOfXs[i].Visible = false;
			}
		}

		public void CountPermValueChanged(float newColPerm)
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < (int)newColPerm; j++)
				{
					drobsTabl[i, j].Visible = true;
				}
			}

			for (int i = 0; i < 16; i++)
			{
				for (int j = (int)newColPerm; j < 16; j++)
				{
					drobsTabl[i, j].Visible = false;
				}
			}

			for (int j = 0; j < (int)newColPerm; j++)
			{
				xLine[j].Visible = true;
			}

			for (int j = (int)newColPerm; j < 16; j++)
			{
				xLine[j].Visible = false;
			}


			for (int j = 0; j < (int)newColPerm; j++)
			{
				func[j].Visible = true;
				funchXLine[j].Visible = true;
			}

			for (int j =  (int)newColPerm; j < 16; j++)
			{
				func[j].Visible = false;
				funchXLine[j].Visible = false;
			}

			colPerm = (int)newColPerm + 1;
		}

		private static HBoxContainer GenXLine(RichTextLabel[] xLine)
		{
			HBoxContainer xs_line = new HBoxContainer();
			for (int i = 0; i < 16; i++)
			{
				RichTextLabel x_i = new RichTextLabel();
				x_i.BbcodeEnabled = true;
				x_i.Text = "[i][char=1D4B3][char=";

				int unicode_char = 2080;

				if (i >= 9)
				{
					x_i.Text += "2081][char=" + (unicode_char + i - 9) + "][/i]";

				}
				else
				{
					x_i.Text += unicode_char + (i + 1) + "][/i]";
				}

				x_i.CustomMinimumSize = new Vector2(59.0f, 25.0f);

				xLine[i] = x_i;
				xs_line.AddChild(x_i);
			}
			RichTextLabel b = new RichTextLabel();
			b.BbcodeEnabled = true;
			b.Text = "[i][char=1D6C3][/i]";
			b.CustomMinimumSize = new Vector2(59.0f, 25.0f);
			xLine[16] = b;
			xs_line.AddChild(b);

			return xs_line;
		}
	}
}