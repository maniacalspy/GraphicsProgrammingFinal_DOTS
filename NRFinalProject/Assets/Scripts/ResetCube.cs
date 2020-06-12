using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[Serializable]
public struct ResetCube : IComponentData
{
}

public class ResetSystem : JobComponentSystem
{
    EntityCommandBufferSystem m_Barrier;



    protected override void OnCreate()
    {
        // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();


        ///create the job that needs to be scheduled
        var job = new ResetJob
        {
            CommandBuffer = commandBuffer
        }.Schedule(this, inputDeps);

        m_Barrier.AddJobHandleForProducer(job);

        return job;
    }

    [RequireComponentTag(typeof(ResetCube), typeof(MovingCube))]
    struct ResetJob : IJobForEachWithEntity<Translation>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public void Execute(Entity entity, int index, ref Translation translation)
        {
            float angle = RandomCache.GetAngle();
            float newX = SpawnerSystem.SpawnerPos.x + Mathf.Sin(angle) * RandomCache.GetCenterDistance();
            float newZ = SpawnerSystem.SpawnerPos.z + Mathf.Cos(angle) * RandomCache.GetCenterDistance();
            translation.Value = new float3(newX, SpawnerSystem.SpawnerPos.y, newZ);
            CommandBuffer.RemoveComponent<ResetCube>(index, entity);
            uint BlockType = RandomCache.GetCubeType();
            if (BlockType == 1) CommandBuffer.AddComponent(index, entity, new MoveUp(RandomCache.GetLifeSpan()));
            else CommandBuffer.AddComponent(index, entity, new MoveRandom(RandomCache.GetLifeSpan(), SpawnerSystem.GetRandComponentfloat3()));
        }
    }


}

    
