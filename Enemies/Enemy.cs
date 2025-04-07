using Godot;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public partial class Enemy : CharacterBody3D
{
	[Export] Health health;
	[Export] Marker3D shootingPoint;
	[Export] EnemyStats stats;
	[Export] NavigationAgent3D navAgent;
	[Export] AnimationPlayer animPlayer;
	[Export] Area3D detectArea;
	[Export] Area3D shootArea;
	[Export] PackedScene projectile;
	[Export] Timer aimingTimer;
	[Export] Timer shootTimer;
	[Export] Timer deleteTimer;
	[Export] PhysicalBoneSimulator3D ragdollSimulator;

	[Export] CollisionShape3D collisionShape;

	[Export] AudioStreamPlayer3D shootSound;

	[Export] GpuParticles3D particles_Shoot;
	//[Export] CheckForObstacles obstacleCheck;

	public bool ragdolled = false;

	Vector3 velocity = Vector3.Zero;
	Vector3 nextPosition = Vector3.Zero;

	Node3D targetNode;

	bool aimingTimerStarted = false;

	int navUpdateAfterNumberOfIterations = 20;
	int iteration = 0;
	int navAgentUpdateOffset = 0;
	


	public enum S
	{
		Idle,
		Running,
		Aiming,
		Shooting,
		Ragdoll
	}
	public S st = S.Running;
	
	public override void _Ready()

	{
		health.hit += gotHit;
		health.died += onDeath;

		targetNode = GetTree().GetFirstNodeInGroup("Player") as Node3D;

		detectArea.BodyEntered += detectAreaEntered;
		shootArea.BodyEntered += shootAreaEntered;
		shootArea.BodyExited += shootAreaExited;
		aimingTimer.Timeout += shoot;
		shootTimer.Timeout += spawnProjectile;
		shootTimer.Timeout += particles_Shoot.Restart;
		shootTimer.Timeout += ()=> shootSound.Play();
		animPlayer.AnimationFinished += animFinished;

		iteration = new Random().Next(0, navUpdateAfterNumberOfIterations);
		
		
	}
	public override void _PhysicsProcess(double delta)
	{

		applyGravity(delta);
		switch (st)
		{
			case S.Idle:

				break;
			case S.Running:
				if (animPlayer.CurrentAnimation != "Run")
				{
					if (targetNode != null)
					{
						LookAt(targetNode.GlobalPosition, Vector3.Up);
					}
					animPlayer.Play("Run");

				}
				updateTargetPosition();
				applyMovement(delta);
				break;
			case S.Aiming:


				if (!aimingTimerStarted)
				{
					aimingTimer.Start();
					aimingTimerStarted = true;
				}
				//animPlayer.Pause();
				animPlayer.Play("Idle");

				break;

			case S.Shooting:
				LookAt(targetNode.GlobalPosition, Vector3.Up);
				Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
				break;
			case S.Ragdoll:

				break;

		}

		Velocity = velocity;
		MoveAndSlide();
	}
	void gotHit(float damage)
	{
		if (st == S.Idle && st != S.Ragdoll)
		{
			st = S.Running;
		}
	}
	void shoot()
	{
		if (ragdolled)
		{
			return;
		}
		
		LookAt(targetNode.GlobalPosition, Vector3.Up);
		Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
		st = S.Shooting;
		animPlayer.Play("Shoot");
		shootTimer.Start();
		aimingTimerStarted = false;

	}
	void animFinished(StringName name)
	{
		if (name == "Shoot" && st != S.Ragdoll)
		{
			st = S.Aiming;
		}
	}
	public void spawnProjectile()
	{
		LookAt(targetNode.GlobalPosition, Vector3.Up);
		var ball = projectile.Instantiate() as BallProjectile;
		ball.LookAt(targetNode.GlobalPosition, Vector3.Up);
		ball.GlobalPosition = shootingPoint.GlobalPosition;
		ball.direction = (targetNode.GlobalPosition - shootingPoint.GlobalPosition).Normalized();
		GetParent().AddChild(ball);

	}
	void applyGravity(double delta)
	{
		if (!IsOnFloor())
		{
			velocity.Y -= 9.8f * (float)delta;
		}
	}
	void applyMovement(double delta)
	{


		Vector3 direction = (navAgent.GetNextPathPosition() - GlobalPosition).Normalized() * stats.speed;
		velocity.X = direction.X;
		velocity.Z = direction.Z;
		//this.LookAt(nextPosition, Vector3.Up);
	}
	public void detectAreaEntered(Node body)
	{
		if (body is player && st == S.Idle && st != S.Ragdoll)
		{
			st = S.Running;
		}
	}
	public void shootAreaEntered(Node body)
	{
		if (body is player && !ragdolled)
		{
			st = S.Aiming;
			velocity.X = 0;
			velocity.Z = 0;
		}
	}
	public void shootAreaExited(Node body)
	{
		if (body is player)
		{
			st = S.Running;
		}
	}
	void updateTargetPosition()
	{
		if (iteration > navUpdateAfterNumberOfIterations)
		{
			navAgent.TargetPosition = targetNode.Position;
			nextPosition = navAgent.GetNextPathPosition();
			iteration = 0;
		}
		else
		{
			iteration++;
		}
	}
	void onDeath()
	{
		ragdolled = true;
		st = S.Ragdoll;
		animPlayer.QueueFree();
		health.hitAble = false;
		ragdollSimulator.PhysicalBonesStartSimulation();
		detectArea.QueueFree();
		shootArea.QueueFree();
		animPlayer.QueueFree();
		navAgent.QueueFree();
		collisionShape.Disabled = true;
		deleteTimer.Start();
	}
}
