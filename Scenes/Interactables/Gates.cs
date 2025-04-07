using Godot;
using System;

public partial class Gates : Node3D
{
    [Export] public bool isOneTimeOnly = false;
    [Export] public bool closed = true;
    

    [Export] Node[] triggers;
    [Export] Godot.Collections.Dictionary<string, ACTIONS> dict;

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
        GD.Print("Gates: act() action : " + A);
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
        animationPlayer.SpeedScale = 1f;

        if (A == ACTIONS.close)
        {
            GD.Print("should close;");
            animationPlayer.Play("close");
        }
        else
        {
            animationPlayer.Play("open");
            GD.Print("should open;");
        }
    }
    public void animFinished(StringName name)
    {
        GD.Print("Gates anim finished : " + name);
        switch (name)
        {
            case "close":
                if (queueOpen) {
                    animationPlayer.Play("open");
                    queueOpen = false;
                }
                break;
            case "open":
                if (queueClose)
                {
                    animationPlayer.Play("close");
                    queueClose = false;
                }
                break;
        } 
    }

}
