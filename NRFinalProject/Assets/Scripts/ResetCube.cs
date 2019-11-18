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

    static Unity.Mathematics.Random rand = new Unity.Mathematics.Random(2515646);

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
            float radius = 10f;
            float newX = rand.NextFloat(-radius, radius);
            float AbsNewX = Mathf.Abs(newX);
            float newZ = rand.NextFloat(-radius + AbsNewX, radius - AbsNewX);
            translation.Value = new float3(newX, -7.0f, newZ);
            CommandBuffer.RemoveComponent<ResetCube>(index, entity);
            MoveUp NewMove = new MoveUp(rand.NextFloat(10f, 50f));
            CommandBuffer.AddComponent<MoveUp>(index, entity, NewMove);
        }
    }


}

    
