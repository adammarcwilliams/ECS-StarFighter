using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class AimTargetMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        var gamepad = Gamepad.current; // Should extract all this into its own input system later
        if (gamepad == null) { return; }
        float3 direction = new float3(gamepad.leftStick.ReadValue(), 2f);

        Entities
            .WithName("AimTargetMovementSystem")
            .WithoutBurst()
            .WithAll<AimTargetTag>()
            .ForEach((ref Translation position) =>
            {
                position.Value = direction;
            })
            .Run();
    }
}
