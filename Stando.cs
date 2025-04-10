using Godot;
using System;

public partial class Stando : Node3D
{
    [Export] Node3D red;
    [Export] Node3D blue;
    [Export] AudioStreamPlayer3D asp;
    [Export] Area3D area;
    [Export] Label3D label;
    [Export] Skull.COLOR t;
    [Signal] public delegate void StandactivatedEventHandler();
    public bool activated = false;
    
    public override void _Ready()
    {
        label.Text = "Place: " + t.ToString();
        area.BodyEntered += bodyEntered;
    }
    public void bodyEntered(Node3D body)
    {
        if (activated) return;

        if(body is player p )
        {
            if (p.GetNode<PlayerItemsManager>("PlayerItemsManager").takeItem(t.ToString(), 1)){
                label.Visible = false;
                asp.Play();
                activated = true;
                if(t == Skull.COLOR.SKULLRED)
                {
                    red.Visible = true;
                }
                if (t == Skull.COLOR.SKULLBLUE)
                {
                    blue.Visible = true;
                }
                EmitSignal(SignalName.Standactivated);
            }
        }
    }
    
}
