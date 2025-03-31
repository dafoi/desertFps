using Godot;
using System;
using System.Collections.Generic;
using static Godot.Image;

public partial class Decals : Node3D
{
    [Export] int max_amount = 20;
    [Export] PackedScene decal;
    [Export] float time;
    public bool decalsEnabled = true;
    public override void _Ready()
    {
        createTimer();
        Settings.Instance.SettingsChanged += update;
    }
    void update(SettingsRes res)
    {
        decalsEnabled = res.decals;
        if(decalsEnabled)
        {
            createTimer();
        }
        else
        {
            var nodes = GetChildren();
            foreach(var node in nodes) 
            {
                node.QueueFree();
            }
        }
        
        
    }
    public void onTimeOut()
    {
        checkForExcess();
        createTimer();
    }
    public void createTimer()
    {
        if (!decalsEnabled) return;
        GetTree().CreateTimer(time).Timeout += onTimeOut;

    }
    public void checkForExcess()
    {
        var children = GetChildren();
        if (children.Count > max_amount)
        {
            for (int i = 0; i < children.Count - max_amount; i++)
            {
                children[i].QueueFree();
            }
        }
    }
    public void place(Vector3 collisionPoint, Vector3 normal)
    {
        if (!decalsEnabled)
        {
            return;
        }
        var decalInstance = decal.Instantiate() as Decal;
        Quaternion quat = new Quaternion(Vector3.Up, normal);
        Basis basis = new Basis(quat);
        decalInstance.GlobalTransform = new Transform3D(basis,collisionPoint);
        AddChild(decalInstance);
    }
}
