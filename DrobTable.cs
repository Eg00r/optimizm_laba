using Godot;
using System;

public partial class DrobTable : PanelContainer
{
	public uint i = 0;
	public uint j = 0;
	private bool _isOpor = false;
	public bool isOpor{
		get{
			return _isOpor;
		}
		set{
			_isOpor = value;
			if(value){

				this.AddThemeStyleboxOverride("panel", opor);
				}
			else{

				this.AddThemeStyleboxOverride("panel", norm);
				}
		}
	}
	public AnswerTabel mother;
	public Label top;
	public Label down; 
	
	private bool _visible = true;
	public bool VisibleAll{
		get{
			return _visible;
		}
		set{
			_visible = value;
			top.Visible = value;
			down.Visible = value;
			this.Visible = value;
			hBox.Visible = value;
			line.Visible = value;
		}
	}

	StyleBoxFlat opor ;
StyleBoxFlat norm ;

	private VBoxContainer hBox;
	private Panel line;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		 opor = ResourceLoader.Load<StyleBoxFlat>("res://DrobTabelOpor_StyleBoxFlat.tres");
		 norm = ResourceLoader.Load<StyleBoxFlat>("res://DrobTabelNorm_StyleBoxFlat.tres");
		mother = GetNode<AnswerTabel>("../..");
		top = GetNode<Label>("HBox/Top");
		down = GetNode<Label>("HBox/Down");
		hBox = GetNode<VBoxContainer>("HBox");
		line = GetNode<Panel>("HBox/Line");
	}

	public void Set(Drob d){
		top.Text = d.Top.ToString();
		down.Text = d.Down.ToString();

		this.Size = new Vector2(Math.Max(top.Size.X, down.Size.X) , this.Size.Y);
		GD.Print($"{i} {j} :" + top.Size.X + " " + down.Size.X + " " + this.Size + "\n");

		CallDeferred(nameof(after));
	}

	public void after(){
		this.Size = new Vector2(Math.Max(top.Size.X, down.Size.X) , this.Size.Y);
		GD.Print($"AFTER {i} {j} :" + top.Size.X + " " + top.Size.Y + " " + this.Size + "\n");
	}

	public void OnGuiInput(InputEvent ev){
		if(ev is not InputEventMouseButton)
			return;

		InputEventMouseButton ev_mouse = (InputEventMouseButton)ev ;

		if(ev_mouse.ButtonIndex == MouseButton.Left && ev_mouse.IsReleased()){
			GD.Print($"нажали {i},{j}");
			if(isOpor)
				mother.CellSelected(i, j);
		}
	}
}
