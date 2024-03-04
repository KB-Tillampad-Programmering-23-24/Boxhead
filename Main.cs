using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene projectileScene {get; set;}
	Camera2D camera;
	Player player;
	int level = 1;
	Level currentLevel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = GetNode<Camera2D>("Camera2D");
		player = GetNode<Player>("Player");
		currentLevel = GetNode<Level>("Level"+level);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		camera.Position = player.Position;
	}

	public void OnLevelFinish(){
		GD.Print("Swap level");
		currentLevel.QueueFree();
		level++;
		var nextLevel = GD.Load<PackedScene>("res://Level"+level+".tscn").Instantiate<Level>();
		AddChild(nextLevel);
	}

	public void OnPlayerShoot(int playerDirection){
		var projectile = projectileScene.Instantiate<RigidBody2D>();
		Vector2 projectielPosition = player.Position;
		projectielPosition += new Vector2(100*playerDirection,0);
		projectile.Position = projectielPosition;

		Vector2 projectileVelocity = projectile.LinearVelocity*playerDirection;
		projectile.LinearVelocity = projectileVelocity;
		AddChild(projectile);
	}

}
