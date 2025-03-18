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
    [Export] CheckForObstacles obstacleCheck;
    Vector3 velocity = Vector3.Zero;
    Vector3 nextPosition = Vector3.Zero;
    Node3D targetNode;
    bool aimingTimerStarted = false;
    int iteration = 0;


    enum S
    {
        Idle,
        Running,
        Aiming,
        Shooting,
    }
    S st = S.Idle;
    public override void _Ready()

    {
        health.hit += gotHit;
        targetNode = GetTree().GetFirstNodeInGroup("Player") as Node3D;
        detectArea.BodyEntered += detectAreaEntered;
        shootArea.BodyEntered += shootAreaEntered;
        shootArea.BodyExited += shootAreaExited;
        aimingTimer.Timeout += shoot;
        animPlayer.AnimationFinished += animFinished;
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
                //updateTargetPosition();
                applyMovement(delta);
                break;
            case S.Aiming:


                if (!aimingTimerStarted)
                {
                    aimingTimer.Start();
                    aimingTimerStarted = true;
                }
                animPlayer.Pause();


                break;
            case S.Shooting:
                LookAt(targetNode.GlobalPosition, Vector3.Up);
                Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
                break;

        }

        Velocity = velocity;
        MoveAndSlide();
    }
    void gotHit()
    {
        if (st == S.Idle)
        {
            st = S.Running;
        }
    }
    void shoot()
    {
        LookAt(targetNode.GlobalPosition, Vector3.Up);
        Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
        st = S.Shooting;
        animPlayer.Play("Shoot");
        aimingTimerStarted = false;

    }
    void animFinished(StringName name)
    {
        if (name == "Shoot")
        {
            st = S.Aiming;
        }
    }
    void spawnProjectile()
    {
        LookAt(targetNode.GlobalPosition, Vector3.Up);
        var ball = projectile.Instantiate() as BallProjectile;
        ball.LookAt(targetNode.GlobalPosition, Vector3.Up);
        ball.GlobalPosition = shootingPoint.GlobalPosition;
        ball.direction = (targetNode.GlobalPosition - shootingPoint.GlobalPosition).Normalized();
        GetParent().AddChild(ball);
        GD.Print($"target : {targetNode.GlobalPosition} ,spawn at: {ball.GlobalPosition},");

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
        navAgent.TargetPosition = targetNode.Position;
        Vector3 direction = (navAgent.GetNextPathPosition() - GlobalPosition).Normalized() * stats.speed;
        velocity.X = direction.X;
        velocity.Z = direction.Z;
        //this.LookAt(nextPosition, Vector3.Up);
    }
    public void detectAreaEntered(Node body)
    {
        if (body is player && st == S.Idle)
        {
            st = S.Running;
        }
    }
    public void shootAreaEntered(Node body)
    {
        if (body is player)
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
        if (iteration > 3)
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
}
