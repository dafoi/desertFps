using Godot;
using System;

public partial class Ragdoll : Skeleton3D
{
    
    [Export] Skeleton3D targetSkeleton;
    [Export] PhysicalBoneSimulator3D boneSimulator;
    [Export] float LinearSpringStiffness = 100f;
    [Export] float LinearSpringDamping = 10f;
    [Export] float AngularSpringStiffness = 50;
    [Export] float AngularSpringDamping = 20;
    Godot.Collections.Array<PhysicalBone3D> physicsBones;

    public override void _Ready()
    {
        boneSimulator.PhysicalBonesStartSimulation();
        var children = boneSimulator.GetChildren();
        foreach(var child in children)
        {
            if(child is PhysicalBone3D)
            {
                physicsBones.Add((PhysicalBone3D)child);
            }
        }

    }
    public Vector3 hookesLaw(Vector3 displacement, Vector3 current_velocity, float stiffness, float damping) {
        return (stiffness * displacement) - (damping * current_velocity);
    }
    public override void _PhysicsProcess(double delta)
    {
        foreach(var bone in physicsBones)
        {
            Transform3D targetTransform = targetSkeleton.GlobalTransform * targetSkeleton.GetBoneGlobalPose(bone.GetBoneId());
            Transform3D currentTransform = GlobalTransform * GetBoneGlobalPose(bone.GetBoneId());
            Vector3 positionDifference = targetTransform.Origin - currentTransform.Origin;
            Vector3 force = hookesLaw(positionDifference, bone.LinearVelocity, LinearSpringStiffness, LinearSpringDamping);
            bone.LinearVelocity += force * (float)delta;

            Basis rotationDifference = (targetTransform.Basis * currentTransform.Basis.Inverse());
            Vector3 torque = hookesLaw(rotationDifference.GetEuler(), bone.AngularVelocity, AngularSpringStiffness, AngularSpringDamping);
            bone.AngularVelocity += torque * (float)delta;
        }
    }
}
