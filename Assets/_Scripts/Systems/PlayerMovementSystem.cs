using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;


public class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        var gamepad = Gamepad.current; // Should extract all this into its own input system later
        if (gamepad == null) { return; }
        float3 direction = new float3(gamepad.leftStick.ReadValue(), 0f);

        Entities
            .WithName("PlayerMovementSystem")
            .WithoutBurst()
            .WithAll<PlayerTag>()
            .ForEach((ref Translation position, in MovementData movementData) =>
            {
                float3 pos = position.Value;
                float speed = gamepad.bButton.isPressed ? movementData.speed * 5f : movementData.speed;

                pos += direction * speed * deltaTime;

                if (math.distance(float3.zero, pos) >= movementData.clampRadius) return;

                position.Value = pos;
            })
            .Run();
    }
}
