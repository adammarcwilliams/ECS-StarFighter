using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;


public class ShipLeanSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        

        var gamepad = Gamepad.current; // Should extract all this into its own input system later
        if (gamepad == null) { return; }
        float horizontal = gamepad.leftStick.ReadValue().x;

        Entities
            .WithName("ShipLeanSystem")
            .WithoutBurst()
            .ForEach((ref Rotation rotation, in ShipData shipData) =>
            {
                // Using burst unfriendly Quarternion and MathF helpers here. Only running on a single entity so not a huge deal but figure a way to refactor later
                Quaternion targetRotation = rotation.Value;
                float3 targetEulerAngles = targetRotation.eulerAngles;
                float3 lerped = new float3(targetEulerAngles.x, targetEulerAngles.y, Mathf.LerpAngle(targetEulerAngles.z, -horizontal * shipData.leanLimit, 0.1f));
                Quaternion test = Quaternion.Euler(lerped);
                rotation.Value = test;
            })
            .Schedule();
    }
}