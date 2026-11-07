using System;
using Godot;

public partial class DrobTable : PanelContainer
{
	public uint i = 0;
	public uint j = 0;

	private bool _isOpor = false;
	public bool IsOpor
	{
		get { return _isOpor; }
		set
		{
			_isOpor = value;
			if (value)
				this.AddThemeStyleboxOverride("panel", opor);
			else
				this.AddThemeStyleboxOverride("panel", norm);
		}
	}

	public AnswerTabel mother;
	public Label top;
	public Label down;

	private bool _visible = true;
	public bool VisibleAll
	{
		get { return _visible; }
		set
		{
			_visible = value;
			top.Visible = value;
			down.Visible = value;
			this.Visible = value;
			hBox.Visible = value;
			line.Visible = value;
		}
	}

	private StyleBoxFlat opor;
	private StyleBoxFlat norm;
	private VBoxContainer hBox;
	private Panel line;


	private enum DrobUIStatus
	{
		Line,
		Drob
	}
	private DrobUIStatus _drobUIStatus = DrobUIStatus.Line;


	public override void _Ready()
	{
		opor = ResourceLoader.Load<StyleBoxFlat>("uid://c7tk1ohbwsi75");
		norm = ResourceLoader.Load<StyleBoxFlat>("uid://dnr14aka3jxof");
		
		mother = GetNode<AnswerTabel>("../..");
		top = GetNode<Label>("HBox/Top");
		down = GetNode<Label>("HBox/Down");
		hBox = GetNode<VBoxContainer>("HBox");
		line = GetNode<Panel>("HBox/Line");
	}

	public void Set(Drob d)
	{
		top.Text = d.Top.ToString();
		down.Text = d.Down.ToString();

		if (d.Down == 1)
		{
			this._drobUIStatus = DrobUIStatus.Line;
			down.Visible = false;
			line.Visible = false;
		}
		else
		{
			this._drobUIStatus = DrobUIStatus.Drob;
			if (_visible)
			{
				down.Visible = true;
				line.Visible = true;
			}
		}
	}

	public void OnGuiInput(InputEvent ev)
	{
		if (ev is not InputEventMouseButton)
			return;

		InputEventMouseButton ev_mouse = (InputEventMouseButton)ev;

		if (ev_mouse.ButtonIndex == MouseButton.Left && ev_mouse.IsReleased())
		{
			GD.Print($"нажали {i},{j}");
			if (IsOpor)
				mother.CellSelected(i, j);
		}
	}
}
