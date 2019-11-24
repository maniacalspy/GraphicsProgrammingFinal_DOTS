using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SpawnerSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem EntityCommandBufferSystem;

    static Unity.Mathematics.Random rand = new Unity.Mathematics.Random(2515646);

    public static int SpawnOdds = 4;

    protected override void OnCreate()
    {
        // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// This is where you would create the jobs themselves and schedule them. The job then takes commands and adds them to the command buffer, which will be executed on various threads
    /// </summary>
    /// <param name="inputDeps"></param>
    /// <returns></returns>
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        ///create the job that needs to be scheduled
        var job = new SpawnerJob
        {
            CommandBuffer = EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);

        EntityCommandBufferSystem.AddJobHandleForProducer(job);

        return job;
    }


    /// <summary>
    /// this is the actual job itself, the example struct has a command buffer that is the place the job's commands are issued to, such as creating the entities to spawn in the example scene.
    /// it implements the IJobForEach interface to specify that this job is for each object with the entity components specified, so if multiple objects have the same entities the job will be done on all of them
    /// </summary>
    public struct SpawnerJob : IJobForEachWithEntity<TestEntity, LocalToWorld>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;

        int UpCount;
        int RandCount;
        /// <summary>
        /// this is where the actual logic of the job is held and carried out, such as the example spawning the game objects
        /// </summary>
        /// <param name="entity">the entity that needs the job done</param>
        /// <param name="index"> the index of the job itself</param>
        /// <param name="testEntity">the component matching the first entity type specified</param>
        /// <param name="location">the component matching the second entity type specified</param>
        public void Execute(Entity entity, int index, [ReadOnly] ref TestEntity testEntity, [ReadOnly] ref LocalToWorld location)
        {
            UpCount = 0;
            RandCount = 0;
            Debug.Log("this should output once");

            for (var x = 0; x < testEntity.CountX; x++)
            {
                for (var y = 0; y < testEntity.CountY; y++)
                {
                    for (var z = 0; z < testEntity.CountZ; z++)
                    {
                        var instance = CommandBuffer.Instantiate(index, testEntity.PrefabObject);
                        
                       
                        // Place the instantiated in a grid with some noise
                        float radius = 10f;
                        float xpos = rand.NextFloat(-radius, radius);
                        float AbsXPos = Mathf.Abs(xpos);
                        float zpos = rand.NextFloat(-radius + AbsXPos, radius - AbsXPos);
                        var position = math.transform(location.Value,
                            new float3(xpos, -7 + y * 1.3F, zpos));

                        CommandBuffer.SetComponent(index, instance, new Translation { Value = position });

                        int BlockType = rand.NextInt(1, SpawnOdds);
                        if (BlockType == 1)
                        {
                            CommandBuffer.AddComponent(index, instance, new MoveUp(rand.NextFloat(1f, 25f)));
                            UpCount++;
                        }
                        else
                        {
                            CommandBuffer.AddComponent(index, instance, new MoveRandom(rand.NextFloat(1f, MoveRandom.LifeSpanMax), rand.NextFloat3(MoveRandom.DirectionMin, MoveRandom.DirectionMax)));
                            RandCount++;
                        }
                        
                        CommandBuffer.AddComponent(index, instance, new MovingCube());
                    }
                }
            }

            ///uncommenting this line means the entity that runs this job is deleted, so the above line won't happen every frame
            Debug.Log($"Up Blocks: {UpCount} RandBlocks: {RandCount}");
            CommandBuffer.DestroyEntity(index, entity);
        }

    }

}
