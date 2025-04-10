using Godot;
using System;
[GlobalClass]
public partial class StandoTrigger : Node
{
    [Export] Trigger trg;
    [Export] Stando[] stands;
    [Export] int needSkulls = 2;
    public int currentSkulls = 0;
    public override void _Ready()
    {
        foreach(var stando in stands)
        {
            stando.Standactivated += standActivated;
        }
    }
    public void standActivated()
    {
        currentSkulls++;
        if(currentSkulls >= needSkulls)
        {
            trg.msg = "open";
            trg.emit();
        }
    }

}
