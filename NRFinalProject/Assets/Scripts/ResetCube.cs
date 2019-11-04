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
            translation.Value = new float3(translation.Value.x, -7.0f, translation.Value.z);
            CommandBuffer.RemoveComponent<ResetCube>(index, entity);
            CommandBuffer.AddComponent<MoveUp>(index, entity);
        }
    }


}

    
