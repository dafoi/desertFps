using Godot;
using System;

public partial class player : CharacterBody3D
{
    [Export] public playerResource playerResource;
    float Speed = 12.0f;
    float acceleration = 0.2f;

    float mouse_sensitivity = 0.04f;
    float gravity = -20f;
    float JumpVelocity = 9f;

    [Export] Camera3D cam;
    private Vector2 inputDirection;

    [Export] HeadBob Bob;
    public bool footsteps = false;
    public bool weapon_sway = false;
    [Export] public bool can_die = true;
    Vector3 velocity;
    [Export] AnimationPlayer wep_player;
    [Export] AudioStreamPlayer jumpsound;
    [Export] GpuParticles3D footparticles;
    [Export] healthBar HPProgressbar;
    [Export] AnimationPlayer cameraAnimationPlayer;
    private float timeInAir = 0f;
    private float minTimeToLandAnimation = 0.4f;
    private bool isInAir = false;
    private float positionOfJump = 0.0f;
    
    public override void _Ready()
    {
        playerResource.hp = 100;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        cam.Current = true;
        playerResource.Player = this;
    }
    public override void _PhysicsProcess(double delta)
    {

        if(playerResource.hp <= 0 && can_die)
        {
            //var sm = GetTree().GetFirstNodeInGroup("SceneManager") as SceneManager;
            GetTree().ChangeSceneToFile("uid://dqohd0gwpmbbr");
            //sm.loadLevel("main");
        }

        HPProgressbar.targetValue = playerResource.hp;



        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
            timeInAir += (float)delta;
        }
        if(!IsOnFloor() && !isInAir)
        {
            isInAir = true;
            positionOfJump = GlobalPosition.Y;

        }
        if(IsOnFloor() && isInAir)
        {
            isInAir = false;
            if(timeInAir > minTimeToLandAnimation && GlobalPosition.Y < positionOfJump)
            {
                cameraAnimationPlayer.Play("land");
            }
            timeInAir = 0f;
            
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            jumpsound.Play();
        }
        footparticles.Emitting = false;


        Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
        Bob.input = inputDir;
        inputDirection = inputDir;
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        //IF PLAYER INPUT AIR||GROUND
        if (direction != Vector3.Zero)
        {
            if (wep_player.CurrentAnimation == "Idle") wep_player.Play("Run");
            velocity.X = Mathf.Lerp(Velocity.X, direction.X * Speed, acceleration);
            velocity.Z = Mathf.Lerp(Velocity.Z, direction.Z * Speed, acceleration);
            Bob.bob = true;
            if (IsOnFloor())
            {
                footsteps = true;
                weapon_sway = true;
            }
            footparticles.Emitting = true;

        }
        //IF NO INPUT AIR||GROUND
        else
        {
           
            //GROUND
            if (IsOnFloor())
            {
                velocity.X = Mathf.Lerp(Velocity.X, 0, acceleration);
                velocity.Z = Mathf.Lerp(Velocity.Z, 0, acceleration);

            }
            Bob.bob = false;
            footsteps = false;
            weapon_sway = false;
        }
        if (!IsOnFloor()) footsteps = false;
        if (!IsOnFloor()) weapon_sway = false;

        Velocity = velocity;
        MoveAndSlide();
        

    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mot)
        {
            this.RotateY(Mathf.DegToRad(-mot.Relative.X * mouse_sensitivity));
            cam.RotateX(Mathf.DegToRad(-mot.Relative.Y * mouse_sensitivity));
            cam.RotationDegrees = new Godot.Vector3(Mathf.Clamp(cam.RotationDegrees.X, -90, 90), cam.RotationDegrees.Y, cam.RotationDegrees.Z);

        }
    }




}
