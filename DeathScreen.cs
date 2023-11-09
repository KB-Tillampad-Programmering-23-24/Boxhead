using Godot;
using System;

public partial class DeathScreen : RichTextLabel
{
	public override void _Process(double delta){
		if(Input.IsActionJustPressed("Respawn")){
			GD.Print("Respawn");
			var tree = GetTree();
			tree.ReloadCurrentScene();
			tree.Paused = false;
		}
	}

	public void OnPlayerDied(){
		GD.Print("I died");
		Show();
	}
}
