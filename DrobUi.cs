using System;
using Godot;
using MainNameSpace;

public partial class DrobUi : HBoxContainer
{
	LineEdit top;
	LineEdit down;
	Panel line;
	RichTextLabel XLabel;
	RichTextLabel Plus;
	Button ChaendState;

	public string Top{
		get{
			return top.Text;
		}
		set{
			top.Text = value;
		}
	}

	public string Down{
		get{
			return down.Text;
		}
		set{
			down.Text = value;
		}
	}

	public enum StateEnum
	{
		Normal,
		Drob
	}

	private StateEnum _currentState = StateEnum.Normal;
	public StateEnum State
	{
		get
		{
			return State;
		}

		set
		{
			if (value == StateEnum.Drob)
			{
				down.Visible = true;
				line.Visible = true;
				
				if(!_tableView)
					ChaendState.Visible = true;

				XLabel.VerticalAlignment = VerticalAlignment.Center;

				Plus.SizeFlagsVertical = SizeFlags.ShrinkEnd | SizeFlags.Expand;
				Plus.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;

				down.GrabFocus();
			}
			else
			{
				down.Visible = false;
				line.Visible = false;
				ChaendState.Visible = false;

				XLabel.VerticalAlignment = VerticalAlignment.Bottom;

				Plus.SizeFlagsVertical = SizeFlags.Fill | SizeFlags.Expand;
				Plus.SizeFlagsHorizontal = SizeFlags.Fill;

				top.GrabFocus();
			}

			_currentState = value;
		}
	}

	private bool _tableView = true;
	public bool TableView
	{
		get
		{
			return _tableView;
		}

		set
		{
			_tableView = value;
			if (XLabel != null)
			{
				XLabel.Visible = !value;
			}
			if (Plus != null )
			{
				Plus.Visible = !value;
			}
			
			if(ChaendState != null && _currentState == StateEnum.Normal){
				ChaendState.Visible = false;
			}else{
				ChaendState.Visible = !value;
			}
			
		}
	}

	private int x_j = 1;
	public int X_J
	{
		get
		{
			return x_j;
		}
		set
		{
			x_j = value;
		}
	}

	private int x_i = 0;
	public int X_I
	{
		get
		{
			return x_i;
		}
		set
		{
			x_i = value;
		}
	}

	public Drob toDrob(){
		Drob drob ;
		if(_currentState == StateEnum.Drob)
			drob= new Drob(this.Top, this.Down);
		else if(this.Top == null)
			drob = new Drob(1,1);
		else
			drob = new Drob(this.Top);

		return drob;
	}

	public void ChendgeView(long viewInd){
		TableView = Convert.ToBoolean(viewInd);

	} 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		OptionButton viewModeButton;
		try
		{
			top = GetNode<LineEdit>("Body/Top");
			down = GetNode<LineEdit>("Body/Down");
			line = GetNode<Panel>("Body/Line");
			XLabel = GetNode<RichTextLabel>("XLabel");
			Plus = GetNode<RichTextLabel>("Left/Plus");
			ChaendState = GetNode<Button>("Left/ChendState");
			viewModeButton = GetTree().CurrentScene.GetNode<OptionButton>("%ViewModeButton");
		}
		catch
		{
			OS.Alert("Произошла критичская ошибка ааааа!!!!\n Ctical eroor was hapend! aaaa!!!!");
			GD.Print("Произошла критичская ошибка ааааа!!!!\n Ctical eroor was hapend! aaaa!!!!");
			GetTree().Quit(-1);
			return;
		}

		viewModeButton.ItemSelected += ChendgeView;

		XLabel.Visible = !_tableView;
		Plus.Visible = !_tableView;
		ChaendState.Visible = !_tableView;



		XLabel.BbcodeEnabled = true;

		if (x_j <= 15)
		{
			XLabel.Text = "[i][char=1D4B3][char=";

			int unicode_char = 2080;

			if (x_j >= 9)
			{
				XLabel.Text += "2081][char=" + (unicode_char + x_j - 9) + "][/i]";

			}
			else
			{
				XLabel.Text += unicode_char + (x_j + 1) + "][/i]";
			}
		}
		else
		{
			XLabel.Visible = false;
		}

		if (x_j == 0)
		{
			Plus.Text = "";
			//	Plus.Visible = false;
			Plus.CustomMinimumSize = Vector2.Zero;
		}
		else if (x_j == 16 && x_i > -1)
		{
			//Plus.Text = "≥";
			Plus.Text = "=";
		}


	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void TopTextChanged(String newText)
	{
		if(newText.Length == 0){
			return;
		}

		if (newText[newText.Length - 1] == '/')
		{
			this.State = StateEnum.Drob;
			top.Text = newText.Left(newText.Length - 1);
		}
	}

	public void ChendStateButtonPressed()
	{
		this.State = StateEnum.Normal;
	}

	public void DownGuiInput(InputEvent input){
		if(down.Text.Length == 0 && input.IsActionPressed("ui_text_backspace")){
			this.State = StateEnum.Normal;
		}
	}
}
