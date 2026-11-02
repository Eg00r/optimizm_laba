using Godot;
using System;

public partial class DrobTable : ColorRect
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
			if(value)
				this.Color = new Color("58ad46f5");
			else
				this.Color = new Color("bf8768f5");
		}
	}
	public AnserTabel mother;
	public RichTextLabel top;
	public RichTextLabel down; 
	
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

	private VBoxContainer hBox;
	private Panel line;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mother = GetNode<AnserTabel>("../..");
		top = GetNode<RichTextLabel>("HBox/Top");
		down = GetNode<RichTextLabel>("HBox/Down");
		hBox = GetNode<VBoxContainer>("HBox");
		line = GetNode<Panel>("HBox/Line");
	}

	public void Set(Drob d){
		top.Text = d.Top.ToString();
		down.Text = d.Down.ToString();
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
