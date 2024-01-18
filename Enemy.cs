using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    private int speed = 50;
    public Player target;

    public override void _Ready(){

    }

    public override void _Process(double delta){
        GD.Print(target.Position);
        Velocity = (target.Position - Position).Normalized()*speed;
        MoveAndSlide();
    }
}
