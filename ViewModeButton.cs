using System;
using Godot;

public partial class ViewModeButton : OptionButton {

	HBoxContainer funcXLine;
	HBoxContainer tablXLine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		CallDeferred(nameof(AtEndOfFraemeInit));
	}

	private void AtEndOfFraemeInit() {
		try {
			funcXLine = GetTree().CurrentScene.GetNode("%Function").GetChild<HBoxContainer>(0);
			tablXLine = GetTree().CurrentScene.GetNode("%Tabl").GetChild<HBoxContainer>(0);
		}
		catch (InvalidCastException) {
			GD.Print("EROORRRR!!! funcXLine or tablXLine = null in ViewModeButton.cs");
			OS.Alert("EROORRRR!!! funcXLine or tablXLine = null in ViewModeButton.cs");
			//GetTree().Quit(-1);
			return;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

	public void ViewSelected(int item) {
		switch (item) {
			case 0:  //not tabl
				funcXLine.Visible = false;
				tablXLine.Visible = false;
				break;

			case 1: //tabl
				funcXLine.Visible = true;
				tablXLine.Visible = true;
				break;
		}
	}
}
