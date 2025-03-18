using Godot;
using System;

public partial class SceneManager : Node
{
    [Export] Godot.Collections.Dictionary<string, PackedScene> levels;
    public void loadLevel(string name)
    {
        //Delete old children
        var children = GetChildren();
        foreach(var child in children)
        {
            child.QueueFree();
        }
        //Load level

        var lvl =  levels[name].Instantiate() as Node3D;
        AddChild(lvl);
    }
}
