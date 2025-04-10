using Godot;
using System;

public partial class LostScreen : Node3D
{
    [Export] Button again;
    [Export] Button exit;
    string again_uid;
    
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        exit.Pressed += ()=> { GetTree().Quit(); };
        //Global.Instance.currentSceneLevelUIDSet += () => { again_uid = Global.Instance.currentSceneLevelUID; };
        again_uid = Global.Instance.currentSceneLevelUID;
        again.Pressed += loadAgain;
    }
    public void loadAgain()
    {
        GetTree().ChangeSceneToFile(Global.Instance.currentSceneLevelUID);
    }
}
