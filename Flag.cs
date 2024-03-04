using Godot;
using System;

public partial class Flag : StaticBody2D
{
	[Signal]
	public delegate void OnFlagTouchEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayerEntered(Node2D body){
		if(body is Player){
			GD.Print("Flag touched");
			EmitSignal(SignalName.OnFlagTouch);
		}
	}
}
