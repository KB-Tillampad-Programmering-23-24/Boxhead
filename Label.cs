using Godot;
using System;

public partial class Label : Godot.Label
{
	public void OnPlayerLoseHP(int currentHP){
		Text = "HP: " + currentHP;
	}

}
