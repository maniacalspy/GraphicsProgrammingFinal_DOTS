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
            float angle = rand.NextFloat(0f, Mathf.PI * 2);
            float newX = Mathf.Sin(angle) * rand.NextFloat(radius);
            float newZ = Mathf.Cos(angle) * rand.NextFloat(radius);
            translation.Value = new float3(newX, -7.0f, newZ);
            CommandBuffer.RemoveComponent<ResetCube>(index, entity);
            int BlockType = rand.NextInt(1, SpawnerSystem.SpawnOdds + 1);
            if (BlockType == 1) CommandBuffer.AddComponent(index, entity, new MoveUp(rand.NextFloat(1f, 25f)));
            else CommandBuffer.AddComponent(index, entity, new MoveRandom(rand.NextFloat(1f, MoveRandom.LifeSpanMax), SpawnerSystem.GetRandComponentfloat3()));
        }
    }


}

    
