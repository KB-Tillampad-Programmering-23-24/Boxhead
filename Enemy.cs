using Godot;
using System;
using System.Threading.Tasks;

public partial class Enemy : CharacterBody2D
{
    [Export]
	public PackedScene projectileScene {get; set;}
    private Timer attackTimer;
    private int speed = 50;
    private State currentState;
    private RayCast2D ray;
    public Player target;
    

    enum State{
        PLAYER_SEEN = 0,
        PLAYER_HIDDEN = 1
    }

    public override void _Ready(){
        currentState = State.PLAYER_HIDDEN;
        ray = GetNode<RayCast2D>("RayCast2D");
        attackTimer = GetNode<Timer>("AttackCD");
    }

    public override void _Process(double delta){
        //GD.Print(target.Position);
        //GetNode<Timer>("AttackCD").Start(0.5);
        ray.TargetPosition = target.Position - Position;
        if(ray.IsColliding() && ray.GetCollider() == target){
            setState(State.PLAYER_SEEN);
        } else {
            setState(State.PLAYER_HIDDEN);
        }

        if(currentState == State.PLAYER_SEEN){
            Velocity = (target.Position - Position).Normalized()*speed;
        } else {
            Velocity = Vector2.Zero;
        }

        MoveAndSlide();
    }

    private void setState(State newState){
        if(currentState != newState){
            currentState = newState;
            if(newState == State.PLAYER_SEEN){
                attackTimer.Start(1);
            } else {
                attackTimer.Stop();
            }
        }
    }

    public void OnPlayerSeen(Node2D body){
        if(body == target){
            GD.Print("The player has been discoverd");
            setState(State.PLAYER_SEEN);
            ray.Enabled = true;
        }
    }

    public void OnPlayerHide(Node2D body){
        if(body == target){
            GD.Print("The player has hidden");
            setState(State.PLAYER_HIDDEN);
            ray.Enabled = false;
        }
    }

    public void OnAttackCDTimeout(){
        GD.Print("Attack");
        attackTimer.Start(1);
        /*
        var direction = (target.Position - Position).Normalized();
        ShootProjectile(direction);
        */
        //RapidAttack();
        var direction = (target.Position - Position).Normalized();
        ShootProjectile(direction);
        ShootProjectile(direction.Rotated((float)Math.PI/6));
        ShootProjectile(direction.Rotated(-(float)Math.PI/6));
    }

    public async void RapidAttack(){
        for(int i = 0; i < 3;i++){
            var direction = (target.Position - Position).Normalized();
            ShootProjectile(direction);
            await Task.Delay(150);
        }
    }

    public void ShootProjectile(Vector2 direction){
        
		var projectile = projectileScene.Instantiate<RigidBody2D>();
		Vector2 projectielPosition = direction * 100;
        projectile.Position = projectielPosition;
		Vector2 projectileVelocity = direction * 400;
        projectile.LinearVelocity = projectileVelocity;
        AddChild(projectile);
        /*
		Vector2 projectileVelocity = projectile.LinearVelocity*playerDirection;
		projectile.LinearVelocity = projectileVelocity;
		AddChild(projectile);
        */
	}
}
