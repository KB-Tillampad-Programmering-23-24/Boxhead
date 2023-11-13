using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void LoseHPEventHandler(int currentHP);
	[Signal]
	public delegate void DiedEventHandler();

	public int currentHP = 3;
	public int Speed {get; set;} = 350;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("Die")){
			currentHP--;
			EmitSignal(SignalName.LoseHP, currentHP);
			GD.Print("Stop hitting yourself");

			if(currentHP <= 0){
				EmitSignal(SignalName.Died);
				GetTree().Paused = true;
			}
		}


		var myVelocity = Velocity;

		var myDirection = Input.GetVector("MoveLeft","MoveRight","MoveUp","MoveDown");

		if(myDirection != Vector2.Zero){
			myVelocity = myDirection * Speed;
		} else {
			myVelocity = Vector2.Zero;
		}

		Velocity = myVelocity;
		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta){
		for(int i = 0; i < GetSlideCollisionCount(); i++){
			var collision = GetSlideCollision(i);
			if(collision.GetCollider() is RigidBody2D){
				((RigidBody2D)collision.GetCollider()).ApplyForce(-collision.GetNormal()*1000);
			}
		}

	}
}
