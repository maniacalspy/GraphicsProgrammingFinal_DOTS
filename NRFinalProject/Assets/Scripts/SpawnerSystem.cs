using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

//[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SpawnerSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem EntityCommandBufferSystem;


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


        /// <summary>
        /// this is where the actual logic of the job is held and carried out, such as the example spawning the game objects
        /// </summary>
        /// <param name="entity">the entity that needs the job done</param>
        /// <param name="index"> the index of the job itself</param>
        /// <param name="c0">the component matching the first entity type specified</param>
        /// <param name="c1">the component matching the second entity type specified</param>
        public void Execute(Entity entity, int index, ref TestEntity c0, ref LocalToWorld c1)
        {
            Debug.Log("this should output every frame");

            ///uncommenting this line means the entity that runs this job is deleted, so the above line won't happen every frame
            //CommandBuffer.DestroyEntity(index, entity);
        }

    }

}
