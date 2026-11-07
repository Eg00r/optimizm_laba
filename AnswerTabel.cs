using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MainNameSpace;

public partial class AnswerTabel : VBoxContainer {
	List<SimpTabel> tabels;
	uint currentTabl = 0;

	InputTables inputTables;
	Label status;

	DrobTable[,] drobTablesUI = new DrobTable[17, 17];

	RichTextLabel[] freePermUI = new RichTextLabel[17];
	RichTextLabel[] bazPermUI = new RichTextLabel[17];

	Label counterUI = new Label();
	GridContainer anserGrid;

	enum SolveState {
		Vspomog,
		Osnov
	}

	SolveState solveState = SolveState.Vspomog;

	public override void _Ready() {
		//GD.Print("\n\n\n0_0\n\n\n\n");
		anserGrid = GetNode<GridContainer>("AnswerGrid");
		anserGrid.Columns = 18;

		PackedScene drobTabel_scene = ResourceLoader.Load<PackedScene>("uid://o2qkaxddpbun");

		counterUI.Text = "0";
		anserGrid.AddChild(counterUI);

		GenXLine(1, 16, freePermUI);

		for (uint i = 0; i < 17; i++) {
			//HBoxContainer line = new HBoxContainer();
			for (uint j = 0; j < 17; j++) {
				DrobTable drobTable = drobTabel_scene.Instantiate<DrobTable>();
				drobTable.i = i;
				drobTable.j = j;
				//drobTable.Visible = false;
				//line.AddChild(drobTable);
				anserGrid.AddChild(drobTable);
				drobTablesUI[i, j] = drobTable;
			}
			//anserGrid.AddChild(line);
		}

		GenXLine(17, 32, bazPermUI, "1D40F");
		for (int i = 0; i < 17; i++) {
			anserGrid.MoveChild(bazPermUI[i], 18 + 18 * i);
		}

		inputTables = GetNode<InputTables>("%InputTables");
		status = GetNode<Label>("Status");
		status.Text = "Статус: нету";
	}

	private static string GenTextXi(uint i) {
		string ret = "[i][char=1D4B3][char=";

		uint unicode_char = 2080;

		uint frst = i % 10 + unicode_char;

		uint scnd = (i / 10) % 10 + unicode_char;

		if (i >= 10) {
			ret += $"{scnd}][char={frst}][/i]";
		}
		else {
			ret += frst + "][/i]";
		}

		return ret;
	}

	private void GenXLine(int i_form, int i_to, RichTextLabel[] permsUI, string b_code = "1D6C3") {
		for (int i = i_form, permsUI_ind = 0; i <= i_to && permsUI_ind < permsUI.Length; i++, permsUI_ind++) {
			RichTextLabel x_i = new RichTextLabel();
			x_i.BbcodeEnabled = true;
			x_i.Text = GenTextXi((uint)i);

			x_i.CustomMinimumSize = new Vector2(30.0f, 25.0f);

			permsUI[permsUI_ind] = x_i;
			anserGrid.AddChild(x_i);
		}
		RichTextLabel b = new RichTextLabel();
		b.BbcodeEnabled = true;
		b.Text = $"[i][char={b_code}][/i]";
		b.CustomMinimumSize = new Vector2(30.0f, 25.0f);
		permsUI[permsUI.Length - 1] = b;
		anserGrid.AddChild(b);

		//	return xs_line;
	}


	public void Start() {
		tabels = new List<SimpTabel>();
		currentTabl = 0;
		solveState = SolveState.Vspomog;

		status.Text = "Статус: решение вспомогательной задачи. Шаг 0";
		SimpTabel vspomog = new SimpTabel(inputTables.colPerm, inputTables.colOgr + 1);

		for (int i = 0; i < inputTables.colOgr; i++) {
			for (int j = 0; j < inputTables.colPerm - 1; j++) {
				vspomog.tabl[i, j] = inputTables.drobsTabl[i, j].toDrob();
			}
		}

		for (int i = 0; i < inputTables.colOgr; i++) {
			vspomog.tabl[i, inputTables.colPerm - 1] = inputTables.drobsTabl[i, 16].toDrob();
		}

		SimpTabel.InitVspomogLastLine(vspomog);

		tabels.Add(vspomog);
		TableShow(vspomog);
	}

	private bool TableShow(SimpTabel tbl) {
		counterUI.Text = tbl.step.ToString();

		anserGrid.Columns = (int)tbl.colPerm + 1;
		for (uint i = 0; i < 17; i++) {
			for (uint j = 0; j < 17; j++) {
				drobTablesUI[i, j].VisibleAll = false;
				drobTablesUI[i, j].IsOpor = false;
			}
		}

		for (uint i = 0; i < tbl.colOgr; i++) {
			for (uint j = 0; j < tbl.colPerm; j++) {
				drobTablesUI[i, j].VisibleAll = true;

				drobTablesUI[i, j].Set(tbl.tabl[i, j]);
			}
		}

		(uint strk, uint stlb)[] oprs = SimpTabel.SimplOporns(tbl);
		foreach ((uint strk, uint stlb) in oprs) {
			drobTablesUI[strk, stlb].IsOpor = true;
		}


		for (int i = tbl.freePerm.Length - 1; i < freePermUI.Length - 1; i++) {
			freePermUI[i].Visible = false;
		}

		for (int i = tbl.bazPerm.Length - 1; i < bazPermUI.Length; i++) {
			bazPermUI[i].Visible = false;
		}

		for (uint i = 0; i < tbl.bazPerm.Length - 1; i++) {
			bazPermUI[i].Visible = true;
			bazPermUI[i].Text = GenTextXi(tbl.bazPerm[i]);
		}
		//P`шка
		bazPermUI[tbl.bazPerm.Length - 1].Visible = true;
		bazPermUI[tbl.bazPerm.Length - 1].Text = $"[i][char=1D40F][/i]";

		for (uint i = 0; i < tbl.freePerm.Length - 1; i++) {
			freePermUI[i].Visible = true;
			freePermUI[i].Text = GenTextXi(tbl.freePerm[i]);
		}

		return oprs.Length == 0;
	}

	public void CellSelected(uint i, uint j) {
		SimpTabel sledTabel = SimpTabel.SimplStep(tabels[(int)currentTabl], i, j);
		currentTabl++;
		tabels.Add(sledTabel);
		if (solveState == SolveState.Vspomog) {
			if (TableShow(sledTabel)) {
				solveState = SolveState.Osnov;

				if (sledTabel.tabl[sledTabel.colOgr - 1, sledTabel.colPerm - 1] < 0) {
					status.Text = $"Статус: Исходная система несовместна";
					return;
				}
				else if (sledTabel.tabl[sledTabel.colOgr - 1, sledTabel.colPerm - 1] == 0) {
					status.Text = $"Статус: Вспомогательная решена";
				}

				for (uint _i = 0; _i < 17; _i++) {
					for (uint _j = 0; _j < 17; _j++)
						drobTablesUI[_i, _j].IsOpor = true;
				}
			}
			else {
				status.Text = $"Статус: решение вспомогательной задачи. Шаг {sledTabel.step}";
			}
		}
		else {
			status.Text = $"Статус: решение основной задачи. Шаг 0";

		}
	}
}
