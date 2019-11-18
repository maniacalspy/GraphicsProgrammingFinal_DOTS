using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class MoveSystem : JobComponentSystem
{

    EntityCommandBufferSystem m_Barrier;

    static Unity.Mathematics.Random rand = new Unity.Mathematics.Random(2515646);


    protected override void OnCreate()
    {
        // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    //[BurstCompile]
    [RequireComponentTag(typeof(MovingCube))]
    struct MoveJob : IJobForEachWithEntity<Translation, MoveUp>
    {
        public float DeltaTime;

        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;        

        // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeedSpawnAndRemove
        public void Execute(Entity entity, int index, ref Translation translation, ref MoveUp moving)
        {
     // Rotate something about its up vector at the speed given by RotationSpeed_SpawnAndRemove.
            translation.Value = new float3(translation.Value.x + DeltaTime * MoveSystem.rand.NextFloat(), translation.Value.y + DeltaTime * MoveSystem.rand.NextFloat(), translation.Value.z);
            moving.LifeSpan -= DeltaTime;

            if (translation.Value.y > 7.0f || moving.LifeSpan <= 0f || translation.Value.x > 20f)
            {
                CommandBuffer.RemoveComponent<MoveUp>(index, entity);
                CommandBuffer.AddComponent<ResetCube>(index, entity);
            }
        }
    }
   

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        var commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();


        ///create the job that needs to be scheduled
        var job = new MoveJob
        {
            DeltaTime = Time.deltaTime,
            CommandBuffer = commandBuffer
        }.Schedule(this, inputDeps);

        m_Barrier.AddJobHandleForProducer(job);

        return job;
    }
}
