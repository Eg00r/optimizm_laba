using System;
using System.Collections.Generic;
using Godot;
using MainNameSpace;

public partial class AnserTabel : VBoxContainer
{
	List<SimpTabel> tabels = new List<SimpTabel>();
	InputTables inputTables;
	Label status;

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
			}
			this.AddChild(line);
		}

		inputTables = GetNode<InputTables>("%InputTables");
		status = GetNode<Label>("Status");
		status.Text="Статус: нету";
	}

	public void Start(){
		status.Text="Статус: решение вспомогательной задачи. Шаг 0";
		SimpTabel vspomog = new SimpTabel();
		
		vspomog.colOgr = inputTables.colOgr + 1;
		vspomog.colPerm = inputTables.colPerm;

		vspomog.tabl = new Drob[vspomog.colOgr, vspomog.colPerm];
		for(int i = 0; i < inputTables.colOgr; i++){
			for(int j = 0; j < inputTables.colPerm - 1; j++){
				vspomog.tabl[i,j] = inputTables.drobsTabl[i,j].toDrob();
			}
		}
		for(int i = 0; i < inputTables.colOgr; i++){
			vspomog.tabl[i, inputTables.colPerm -1 ] = inputTables.drobsTabl[i,16].toDrob();	
		}

		
	}

	public void CellSelected(uint i, uint j)
	{

	}
}
