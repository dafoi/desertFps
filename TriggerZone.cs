using Godot;
using System;

public partial class TriggerZone : Area3D
{
    [Export] public bool forPlayer = true;
    [Export] Godot.Collections.Dictionary<ACTIONS,string> dict;
    public Trigger trigger;
    enum ACTIONS
    {
        bodyEntered,
        BodyExited,
        AreaEntered,
        AreaExited

    }
    
    public override void _Ready()
    {
        BodyEntered += bodyEntered;
        BodyExited+= bodyExited;
        AreaEntered += areaEntered;
        AreaExited += areaExited;
        trigger = GetNode<Trigger>("Trigger");
    }
    public void bodyEntered(Node body)
    {
        if (forPlayer)
        {
            if(body is player)
            {
                emit(dict[ACTIONS.bodyEntered]);
            }
        }
        else
        {
            emit(dict[ACTIONS.bodyEntered]);
        }
        
    }
    public void bodyExited(Node body)
    {
        if(forPlayer)
        {
            if(body is player)
            {
              emit(dict[ACTIONS.BodyExited]);

            }

        }
        else
        {
            emit(dict[ACTIONS.BodyExited]);
        }
    }
    public void areaEntered(Node body)
    {
        emit(dict[ACTIONS.AreaEntered]);
    }
    public void areaExited(Node body)
    {
        emit(dict[ACTIONS.AreaExited]);
    }
    public void emit(string msg)
    {
        trigger.msg = msg;
        trigger.emit();
    }
}
