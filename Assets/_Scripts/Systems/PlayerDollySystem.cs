using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;


public class PlayerDollySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        var allLtw = GetComponentDataFromEntity<LocalToWorld>();

        Entities
            .WithName("PlayerDollySystem")
            .WithoutBurst()
            .WithAll<PlayerDollyData>()
            .ForEach((ref Translation position, ref Rotation rotation, in PlayerDollyData playerDollyData) =>
            {
                float3 dollyCartPosition = allLtw[playerDollyData.dollyGameObject].Position;
                quaternion dollyCartRotation = allLtw[playerDollyData.dollyGameObject].Rotation;

                position.Value = dollyCartPosition;
                rotation.Value = dollyCartRotation;
            })
            .Run();
    }
}
