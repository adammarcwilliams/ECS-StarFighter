using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;


[UpdateAfter(typeof(AimTargetMovementSystem))]
public class PlayerRotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        var allLtw = GetComponentDataFromEntity<LocalToWorld>();

        var gamepad = Gamepad.current; // Should extract all this into its own input system later
        if (gamepad == null) { return; }
        float horizontal = gamepad.leftStick.ReadValue().x;

        Entities
            .WithName("PlayerRotationSystem")
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((ref Rotation rotation, in Translation position, in AimTargetData aimTargetData) =>
            {
                float3 targetPos = allLtw[aimTargetData.target].Position;
                quaternion rot = RotateTowards(rotation.Value, quaternion.LookRotationSafe(targetPos, math.up()), Deg2Rad * 10000 * deltaTime);

                rotation.Value = rot;
            })
            .Run();
    }

    private const float Deg2Rad = math.PI * 2F / 360F;

    private const float Rad2Deg = 1F / Deg2Rad;

    //quaternion RotateTowards(quaternion from, quaternion to, float maxRadianDelta)
    //{
    //    float dot = math.min(math.abs(math.dot(from, to)), 1.0f);
    //    float angle = (dot > 1.0f - math.EPSILON) ? 0.0f : math.acos(dot) * 2.0F;
    //    if (angle == 0.0f) return to;
    //    return math.slerp(from, to, math.min(1.0f, maxRadianDelta / angle));
    //}

    private quaternion RotateTowards(quaternion from, quaternion to, float maxDegreesDelta)
    {
        float angle = Angle(from, to);
        if (angle == 0.0f) return to;
        return math.slerp(from, to, math.min(1.0f, maxDegreesDelta / angle));
    }

    private float Angle(quaternion a, quaternion b)
    {
        float dot = math.min(math.abs(math.dot(a, b)), 1.0f);
        return IsEqualUsingDot(dot) ? 0.0f : math.acos(dot) * 2.0F * Rad2Deg;

    }

    private bool IsEqualUsingDot(float dot)
    {
        // Returns false in the presence of NaN values.
        return dot > 1.0f - math.EPSILON;
    }

}