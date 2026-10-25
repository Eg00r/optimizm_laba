using System;
using System.Text.Json;
using Godot;
using MainNameSpace;

public partial class MenuBarFile : PopupMenu
{
	FileDialog dialog;
	InputTables inputTables;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dialog = GetNodeOrNull<FileDialog>("FileDialog");
		if (dialog == null)
		{
			GD.Print("ERRROORRR!!! FileDialog wasnt faund!!!");
			OS.Alert("ERRROORRR!!! FileDialog wasnt faund!!!");
			GetTree().Quit(-1);
			return;
		}

		CallDeferred(nameof(InitAtEndOfFrame));
	}

	private void InitAtEndOfFrame()
	{
		inputTables = GetTree().CurrentScene.GetNode<InputTables>("%InputTables");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void IndexPressed(int ind)
	{
		switch (ind)
		{
			case 0: //open	
				dialog.Visible = true;
				dialog.FileMode = FileDialog.FileModeEnum.OpenFile;
				break;

			case 2: //save
				dialog.Visible = true;
				dialog.FileMode = FileDialog.FileModeEnum.SaveFile;
				break;
		}
	}

	public void FileSelected(String filePath)
	{
		switch (dialog.FileMode)
		{
			case FileDialog.FileModeEnum.OpenFile:
				break;

			case FileDialog.FileModeEnum.SaveFile:
				TaskSaveContaner saveCont = new TaskSaveContaner(inputTables);
				var options = new JsonSerializerOptions
				{
					IncludeFields = true,
					WriteIndented = true // для красивого отступа
				};
				string tmp = JsonSerializer.Serialize(saveCont, options);
				GD.Print(tmp);

				break;
		}
	}
}

class TaskSaveContaner
{
	public int colPerm = 1;
	public int colOgr = 1;
	public int taskType = 1;
	
	public (string, string)[] func = new (string, string)[17];
	public (string, string)[][] drobsTabl = new (string, string)[16][];

	public TaskSaveContaner() { }

	public TaskSaveContaner(InputTables inputTables)
	{
		colOgr = inputTables.colOgr;
		colPerm = inputTables.colPerm;
		FFF = 5678;
		taskType = 99;

		for (int j = 0; j < 17; j++)
		{
			func[j] = (inputTables.func[j].Top, inputTables.func[j].Down);
		}

		for (int i = 0; i < 16; i++)
		{
			drobsTabl[i] = new (string, string)[17];
			for (int j = 0; j < 17; j++)
			{
				drobsTabl[i][j] = (inputTables.drobsTabl[i, j].Top, inputTables.drobsTabl[i, j].Down);
			}
		}

	}

}