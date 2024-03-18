using Godot;
using Godot.Collections;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene projectileScene {get; set;}
	Camera2D camera;
	Player player;
	int level = 1;
	Level currentLevel;
	AudioStreamPlayer musicPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = GetNode<Camera2D>("Camera2D");
		player = GetNode<Player>("Player");
		currentLevel = GetNode<Level>("Level"+level);
		musicPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		camera.Position = player.Position;
	}

	public override void _Input(InputEvent @event){
		//GD.Print(@event);

		if(Input.IsActionJustPressed("Mute")){
			AudioServer.SetBusMute(1, true);
		}

		if(Input.IsActionJustPressed("OpenMenu")){
			GetNode<Button>("UI/QuitButton").Visible = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		if(Input.IsActionJustPressed("Save")){
			using var saveGame = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Write);
			saveGame.StoreLine(Json.Stringify(player.Save()));
			GD.Print("Save");
		}

		if(Input.IsActionJustPressed("Load")){
			GD.Print("Load");
			if(!FileAccess.FileExists("user://savegame.json")){
				GD.Print("File doesn't exists please save the game first");
				return;
			}

			using var saveGame = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Read);
			var json = new Json();
			var data = json.Parse(saveGame.GetLine());
			var dictionaryData = new Dictionary<string, Variant>((Dictionary)json.Data);
			player.Load(dictionaryData);


			//int data = Convert.ToInt32(saveGame.GetLine());
			//player.currentHP = data;
			//GetNode<Label>("UI/Label").OnPlayerLoseHP(data);
		}
	}

	public void OnQuitButtonPressed(){
		GetTree().Quit();
	}

	public void OnAudioStreamPlayerFinished(){
		musicPlayer.Play();
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
