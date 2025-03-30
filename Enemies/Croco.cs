using Godot;
using System;
using System.Runtime.InteropServices.JavaScript;

public partial class Croco : CharacterBody3D
{
    [Export] Health health;
    [Export] EnemyStats stats;
    [Export] int damage = 20;
    [Export] NavigationAgent3D navAgent;
    [Export] AnimationPlayer animPlayer;
    [Export] Area3D detectArea;
    [Export] Area3D attackArea;
    [Export] Timer attackTimer;
    [Export] Timer deleteTimer;
    [Export] PhysicalBoneSimulator3D ragdollSimulator;
    [Export] CollisionShape3D[] collisionShape;
    [Export] Node3D helperNode;
    [Export] playerResource pr;
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
        attackTimer.Timeout += attack;

        targetNode = GetTree().GetFirstNodeInGroup("Player") as Node3D;

        detectArea.BodyEntered += detectAreaEntered;
        animPlayer.AnimationFinished += animFinished;

        iteration = new Random().Next(0, navUpdateAfterNumberOfIterations);


    }
    public void attack()
    {
        if (attackArea.HasOverlappingBodies())
        {
            var bodies = attackArea.GetOverlappingBodies();
            foreach(var body in bodies)
            {
                if(body is player)
                {
                    pr.hp-=damage;
                }
            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        LookAt(nextPosition,Vector3.Up);
        var rot = Rotation; rot.X = 0; Rotation = rot;

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
        deleteTimer.Start();
        foreach(var i in collisionShape)
        {
            i.Disabled = true;
        }
    }

}
