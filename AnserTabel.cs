using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MainNameSpace;

public partial class AnserTabel : VBoxContainer
{
	List<SimpTabel> tabels;
	uint currentTabl = 0;

	InputTables inputTables;
	Label status;

	DrobTable[,] drobTablesUI = new DrobTable[17, 17];

	enum SolveState
	{
		Vspomog,
		Osnov
	}

	SolveState solveState = SolveState.Vspomog;

	public override void _Ready()
	{

		PackedScene drobTabel_scene = ResourceLoader.Load<PackedScene>("res://DrobTable.tscn");

		for (uint i = 0; i < 17; i++)
		{
			HBoxContainer line = new HBoxContainer();
			for (uint j = 0; j < 17; j++)
			{
				DrobTable drobTable = drobTabel_scene.Instantiate<DrobTable>();
				drobTable.i = i;
				drobTable.j = j;
				//drobTable.Visible = false;
				line.AddChild(drobTable);

				drobTablesUI[i, j] = drobTable;
			}
			this.AddChild(line);
		}

		inputTables = GetNode<InputTables>("%InputTables");
		status = GetNode<Label>("Status");
		status.Text = "Статус: нету";
	}

	public void Start()
	{
		tabels = new List<SimpTabel>();
currentTabl = 0;

		status.Text = "Статус: решение вспомогательной задачи. Шаг 0";
		SimpTabel vspomog = new SimpTabel();

		vspomog.colOgr = inputTables.colOgr + 1;
		vspomog.colPerm = inputTables.colPerm;

		vspomog.tabl = new Drob[vspomog.colOgr, vspomog.colPerm];
		for (int i = 0; i < inputTables.colOgr; i++)
		{
			for (int j = 0; j < inputTables.colPerm - 1; j++)
			{
				vspomog.tabl[i, j] = inputTables.drobsTabl[i, j].toDrob();
			}
		}

		for (int i = 0; i < inputTables.colOgr; i++)
		{
			vspomog.tabl[i, inputTables.colPerm - 1] = inputTables.drobsTabl[i, 16].toDrob();
		}

		for (int j = 0; j < vspomog.colPerm; j++)
		{
			vspomog.tabl[vspomog.colOgr - 1, j] = new Drob(0, 1);
			for (int i = 0; i < vspomog.colOgr - 1; i++)
			{
				vspomog.tabl[vspomog.colOgr - 1, j] += vspomog.tabl[i, j];
			}
			vspomog.tabl[vspomog.colOgr - 1, j] = -vspomog.tabl[vspomog.colOgr - 1, j];
		}

		vspomog.freePerm = new uint[vspomog.colPerm];
		for (uint i = 1; i <= vspomog.colPerm; i++)
		{
			vspomog.freePerm[i - 1] = i;
		}

		vspomog.bazPerm = new uint[vspomog.colOgr];
		for (uint i = 1; i <= vspomog.colOgr; i++)
		{
			vspomog.freePerm[i - 1] = i;
		}

		vspomog.step = 0;

		tabels.Add(vspomog);
		TableShow(vspomog);
	}

	private bool TableShow(SimpTabel tbl)
	{
		for (uint i = 0; i < 17; i++)
		{
			for (uint j = 0; j < 17; j++)
			{
				drobTablesUI[i, j].VisibleAll = false;
				drobTablesUI[i, j].isOpor = false;
			}
		}

		for (uint i = 0; i < tbl.colOgr; i++)
		{
			for (uint j = 0; j < tbl.colPerm; j++)
			{
				drobTablesUI[i, j].VisibleAll = true;

				drobTablesUI[i, j].Set(tbl.tabl[i, j]);
			}
		}

		(uint strk, uint stlb)[] oprs = SimpTabel.SimplOporns(tbl);
		foreach ((uint strk, uint stlb) in oprs)
		{
			drobTablesUI[strk, stlb].isOpor = true;
		}

		return oprs.Length == 0;
	}

	public void CellSelected(uint i, uint j)
	{
		SimpTabel sledTabel = SimpTabel.SimplStep(tabels[(int)currentTabl], i, j);
		currentTabl++;
		tabels.Add(sledTabel);
		if (solveState == SolveState.Vspomog)
		{
			if (TableShow(sledTabel))
			{
				solveState = SolveState.Osnov;

				if(sledTabel.tabl[ sledTabel.colOgr-1, sledTabel.colPerm-1] < 0){
					status.Text = $"Статус: Исходная система несовместна";
					return;
				}else 				if(sledTabel.tabl[ sledTabel.colOgr-1, sledTabel.colPerm-1] == 0){
					status.Text = $"Статус: Вспомогательная решена";
				}
				
				for (uint _i = 0; _i < 17; _i++)
				{
					for (uint _j = 0; _j < 17; _j++)
						drobTablesUI[_i, _j].isOpor = true;
				}
			}
			else
			{
				status.Text = $"Статус: решение вспомогательной задачи. Шаг {sledTabel.step}";
			}
		}
		else
		{
			status.Text = $"Статус: решение основной задачи. Шаг 0";
			
		}
	}
}
