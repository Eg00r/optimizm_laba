using Godot;
using System;

public partial class DrobTable : ColorRect
{
	public uint i = 0;
	public uint j = 0;
	
	public bool isAnser = false;

	public AnserTabel mother;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mother = GetNode<AnserTabel>("../..");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnGuiInput(InputEvent ev){
		if(ev is not InputEventMouseButton)
			return;

		InputEventMouseButton ev_mouse = (InputEventMouseButton)ev ;

		if(ev_mouse.ButtonIndex == MouseButton.Left && ev_mouse.IsReleased()){
			GD.Print($"нажали {i},{j}");
			if(isAnser)
				mother.CellSelected(i, j);
		}
	}
}
