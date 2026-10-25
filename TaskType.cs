using Godot;
using System;

public partial class TaskType : OptionButton
{
	//SpinBox CountOgrLineEdit = null;
	SpinBox CountPermLineEdit = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//CountOgrLineEdit = GetNode<SpinBox>("%CountOgrLineEdit");
		CountPermLineEdit = GetNode<SpinBox>("%CountPermLineEdit");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ItemSelected(int ind){
		if(ind == 0){
			CountPermLineEdit.MaxValue = 2;
		}else{
			CountPermLineEdit.MaxValue = 16;
		}
	}
}
