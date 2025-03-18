using Godot;
using System;

public partial class doors : AnimationPlayer
{
    private bool opened = false;
    private bool closed = false;
    public void close(Node3D body)
    {
        if (body is player && !closed)
        {
        Play("close");
        }
        
        closed = true;
    }
    public void open(Node3D body)
    {
        if(body is player && !opened)
        {
        Play("open");

        }
        opened = true;
    }
}
