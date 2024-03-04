using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void LoseHPEventHandler(int currentHP);
	[Signal]
	public delegate void DiedEventHandler();
	[Signal]
	public delegate void ShootEventHandler(int playerDirection);

	public int currentHP = 3;
	public int Speed {get; set;} = 350;
	int playerDirection = 1;
	AnimationPlayer animationHandler;
	Sprite2D playerSprite;
	AudioStreamPlayer effectPlayer;

	public enum State {
		IDLE = 0,
		WALKING = 1,
		PUNCHING = 2
	}

	public State currentState;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationHandler = GetNode<AnimationPlayer>("AnimationPlayer");
		playerSprite = GetNode<Sprite2D>("Sprite2D");
		currentState = State.IDLE;
		effectPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
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
			effectPlayer.Play();
			currentState = State.PUNCHING;
		}

		if(myDirection != Vector2.Zero){
			myVelocity = myDirection * Speed;
			if(currentState != State.PUNCHING){
				currentState = State.WALKING;
			}
			if(myDirection.X < 0){
				playerSprite.Scale = new Vector2(-1,1);
				playerDirection = -1;
			} else {
				playerSprite.Scale = new Vector2(1,1);
				playerDirection = 1;
			}
			
		} else {
			myVelocity = Vector2.Zero;
			if(currentState != State.PUNCHING){
				currentState = State.IDLE;
			}
		}

		Velocity = myVelocity;

		switch (currentState)
		{
			case State.PUNCHING:
				animationHandler.Play("punch");
				break;
			case State.WALKING:
				animationHandler.Play("walk");
				break;
			case State.IDLE:
				animationHandler.Play("idle");
				break;
			default:
				animationHandler.Play("idle");
				break;
		}
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

	public void OnAnimationFinished(string name){
		currentState = State.IDLE;
		if(name == "punch"){
			EmitSignal(SignalName.Shoot, playerDirection);
		}
	}
}
