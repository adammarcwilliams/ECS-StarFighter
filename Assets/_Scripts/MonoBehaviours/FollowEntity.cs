using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float3 offset;

    private EntityManager manager;

    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        if (entityToFollow == null) return;

        //Translation entPos = manager.GetComponentData<Translation>(entityToFollow);
        LocalToWorld entLTW = manager.GetComponentData<LocalToWorld>(entityToFollow);
        transform.position = entLTW.Position + offset;
        transform.rotation = entLTW.Rotation;
    }
}

