using Godot;
using System;
using System.Runtime.InteropServices.JavaScript;

public partial class Croco : CharacterBody3D
{
    [Export] Health health;
    [Export] EnemyStats stats;
    [Export] NavigationAgent3D navAgent;
    [Export] AnimationPlayer animPlayer;
    [Export] Area3D detectArea;
    [Export] PhysicalBoneSimulator3D ragdollSimulator;

    [Export] CollisionShape3D[] collisionShape;
    public bool ragdolled = false;

    Vector3 velocity = Vector3.Zero;
    Vector3 nextPosition = Vector3.Zero;

    Node3D targetNode;

    bool aimingTimerStarted = false;

    int navUpdateAfterNumberOfIterations = 12;
    int iteration = 0;
    int navAgentUpdateOffset = 0;



    public enum S
    {
        Idle,
        Running,
        Ragdoll
    }
    public S st = S.Idle;

    public override void _Ready()

    {
        health.hit += gotHit;
        health.died += onDeath;

        targetNode = GetTree().GetFirstNodeInGroup("Player") as Node3D;

        detectArea.BodyEntered += detectAreaEntered;
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

    void animFinished(StringName name)
    {
        
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
        
    }
    public void detectAreaEntered(Node body)
    {
        if (body is player && st == S.Idle && st != S.Ragdoll)
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
        animPlayer.QueueFree();
        navAgent.QueueFree();
        foreach(var i in collisionShape)
        {
            i.Disabled = true;
        }
    }

}
