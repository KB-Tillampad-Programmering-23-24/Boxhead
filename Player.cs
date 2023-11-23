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
	AnimatedSprite2D animationHandler;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationHandler = GetNode<AnimatedSprite2D>("Sprite2D");
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

		if(Input.IsActionJustPressed("Punch")){
			GD.Print("punch");
			animationHandler.Play("punch");
		}

		if(myDirection != Vector2.Zero){
			myVelocity = myDirection * Speed;
			animationHandler.Play("walk");
			if(myDirection.X < 0){
				animationHandler.FlipH = true;
			} else {
				animationHandler.FlipH = false;
			}
			
		} else {
			myVelocity = Vector2.Zero;
			animationHandler.Play("idle");
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
