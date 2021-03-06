﻿using System.Collections;
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



    public static float3 SpawnerPos;
    public static int SpawnOdds = 4;

    protected override void OnCreate()
    {
        // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    public static float3 GetRandComponentfloat3()
    {
        return RandomCache.GetDirection();
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

        /// <summary>
        /// this is where the actual logic of the job is held and carried out, such as the example spawning the game objects
        /// </summary>
        /// <param name="entity">the entity that needs the job done</param>
        /// <param name="index"> the index of the job itself</param>
        /// <param name="testEntity">the component matching the first entity type specified</param>
        /// <param name="location">the component matching the second entity type specified</param>
        public void Execute(Entity entity, int index, [ReadOnly] ref TestEntity testEntity, [ReadOnly] ref LocalToWorld location)
        {
            SpawnerPos = location.Position;
            for (var x = 0; x < testEntity.CountX; x++)
            {
                for (var y = 0; y < testEntity.CountY; y++)
                {
                    for (var z = 0; z < testEntity.CountZ; z++)
                    {
                        var instance = CommandBuffer.Instantiate(index, testEntity.PrefabObject);
                        
                       
                        // Place the instantiated in a grid with some noise
                        float angle = RandomCache.GetAngle();

                        float xpos = SpawnerPos.x + Mathf.Sin(angle) * RandomCache.GetCenterDistance();
                        float zpos = SpawnerPos.z + Mathf.Cos(angle) * RandomCache.GetCenterDistance();

                        var position = math.transform(location.Value,
                            new float3(xpos, 2 * y/testEntity.CountY, zpos));

                        CommandBuffer.SetComponent(index, instance, new Translation { Value = position });
                        //the +1 is to account for the fact that the upper limit is exclusive
                        
                        //this one doesn't use the cache to allow the number of random blocks to vary more
                        uint BlockType = RandomCache.GetCubeType();
                        if (BlockType == 1)
                        {
                            CommandBuffer.AddComponent(index, instance, new MoveUp(RandomCache.GetLifeSpan()));
                        }
                        else
                        {
                            CommandBuffer.AddComponent(index, instance, 
                                    new MoveRandom(RandomCache.GetLifeSpan(), GetRandComponentfloat3()));
                        }
                        
                        CommandBuffer.AddComponent(index, instance, new MovingCube());
                    }
                }
            }

            ///uncommenting this line means the entity that runs this job is deleted, so the above line won't happen every frame
            CommandBuffer.DestroyEntity(index, entity);
        }

    }

}
