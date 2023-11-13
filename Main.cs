using Godot;
using System;

public partial class Main : Node
{
Camera2D camera;
Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = GetNode<Camera2D>("Camera2D");
		player = GetNode<Player>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		camera.Position = player.Position;
	}

}
