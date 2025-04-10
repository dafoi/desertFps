using Godot;
using System;

public partial class Gates : Node3D
{
    [Export] public bool isOneTimeOnly = false;
    [Export] public bool closed = true;
    

    [Export] Node[] triggers;
    [Export] Godot.Collections.Dictionary<string, ACTIONS> dict;
    [Export] AudioStreamPlayer3D audioPlayerOpen;
    [Export] AudioStreamPlayer3D audioPlayerClose;
    [Export] GpuParticles3D particles;
    [Export] float defaultAnimationSpeed = 1.0f;
    private bool used = false;

    private bool queueClose = false;
    private bool queueOpen = false;

    [Export] AnimationPlayer animationPlayer;
    enum ACTIONS
    {
        close,
        open
    }
    public override void _Ready()
    {

        if (!closed)
        {
            animationPlayer.SpeedScale = 200f;
            animationPlayer.Play("open");
            
        }
        
        animationPlayer.AnimationFinished += animFinished;
        

        foreach (var trigger in triggers)
        {
            if (trigger.HasNode("Trigger"))
            {
                Trigger trg = trigger.GetNode<Trigger>("Trigger");
                trg.triggered += recieve;
            }
        }
    }
    public void recieve(string msg)
    {
        act(dict[msg]);
    }

    void act(ACTIONS A)
    {
        if (animationPlayer.IsPlaying())
        {
            if (A == ACTIONS.close){ queueClose = true; }
            else { queueOpen = true; }
        
            return;
        }

        if (used && isOneTimeOnly)
        {
            return;
        }

        used = true;
        

        if (A == ACTIONS.close)
        {
            close();

        }
        else
        {
            open();
            
        }
    }
    public void animFinished(StringName name)
    {
        animationPlayer.SpeedScale = defaultAnimationSpeed;
        GD.Print("Gates anim finished : " + name);
        switch (name)
        {
            case "close":
                if (queueOpen) {
                    open();
                    queueOpen = false;
                }
                break;
            case "open":
                if (queueClose)
                {
                    close();
                    queueClose = false;
                }
                break;
        } 
    }
    public void open()
    {
        animationPlayer.Play("open");
        particles.Emitting = true;
        audioPlayerOpen.Play();
    }
    public void close()
    {
        animationPlayer.Play("close");
        particles.Emitting = true;
        audioPlayerClose.Play();
    }

}
