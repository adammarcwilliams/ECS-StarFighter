using Unity.Entities;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    public float speed;
    public float boostMul;
    public float clampRadius;
}


