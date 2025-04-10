using Godot;
using System;

public partial class Global : Node
{
    public string currentSceneLevelUID;
    private static Global instance;
    public static Global Instance => instance;
    public override void _EnterTree()
    {
        if (instance != null)
        {
            this.QueueFree(); // The Singletone is already loaded, kill this instance
        }
        instance = this;

    }
    public void setCurrentSceneLevel(string uid)
    {
        currentSceneLevelUID = uid;
    }
}
