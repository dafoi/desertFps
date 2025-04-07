using Godot;
using System;
[GlobalClass]
public partial class Trigger : Node
{
    public string msg = "";
    [Signal] public delegate void triggeredEventHandler(string msg);
    public void emit()
    {
        EmitSignal(SignalName.triggered,msg);
        GD.Print("emitted : " + msg);
    }

}
