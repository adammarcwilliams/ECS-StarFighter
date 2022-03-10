using Unity.Entities;

[GenerateAuthoringComponent]
public struct AimTargetData : IComponentData
{
    public Entity target;
}