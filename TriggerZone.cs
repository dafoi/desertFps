using Godot;
using System;

public partial class TriggerZone : Area3D
{
    [Export] public bool OneTimeOnly = false;
    [Export] public bool forPlayer = true;

    [Export] public string[] bodyEnteredSignals;
    [Export] public string[] bodyExitedSignals;
    [Export] public string[] areaEnteredSignals;
    [Export] public string[] areaExitedSignals;
    
    [Export] public Trigger trigger;
    public bool used = false;
    
    
    public override void _Ready()
    {
        BodyEntered += bodyEntered;
        BodyExited+= bodyExited;
        AreaEntered += areaEntered;
        AreaExited += areaExited;
    }
    public void bodyEntered(Node body)
    {
        if (used && OneTimeOnly) return;

        if (forPlayer)
        {
            if(body is player)
            {
                emit(bodyEnteredSignals);
                used = true;
            }
        }
        else
        {
            emit(bodyEnteredSignals);
            used = true;
        }
        
    }
    public void bodyExited(Node body)
    {
        if (used && OneTimeOnly) return;

        if(forPlayer)
        {
            if(body is player)
            {
              emit(bodyExitedSignals);
                used = true;

            }

        }
        else
        {
            emit(bodyExitedSignals);
            used = true;
        }
    }
    public void areaEntered(Node body)
    {

        emit(areaEnteredSignals);
    }
    public void areaExited(Node body)
    {
        emit(areaExitedSignals);
    }
    public void emit(string[] msg)
    {
        foreach(string s in msg)
        {
            trigger.msg = s;

            trigger.emit();
            GD.Print(s);
        }
    }
}
