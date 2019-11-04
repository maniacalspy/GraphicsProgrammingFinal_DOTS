using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class MoveSystem : ComponentSystem
{
    Unity.Mathematics.Random rand = new Unity.Mathematics.Random(2515646);


    protected override void OnUpdate()
    {
        Entities.WithAllReadOnly<MovingCube, MoveUp>().ForEach(
             (Entity id, ref Translation translation) =>
                {
                    var deltaTime = Time.deltaTime;
                    translation = new Translation()
                    {
                        Value = new float3(translation.Value.x, translation.Value.y + deltaTime * rand.NextFloat(), translation.Value.z)
                    };

                    if (translation.Value.y > 6.0f)
                        EntityManager.RemoveComponent<MoveUp>(id);
                }
             );



        Entities.WithAllReadOnly<MovingCube>().WithNone<MoveUp>().ForEach(
                (Entity id, ref Translation translation) =>
                {
                    translation = new Translation()
                    {
                        Value = new float3(translation.Value.x, -6.0f, translation.Value.z)
                    };

                    EntityManager.AddComponentData(id, new MoveUp());
                }
            );
    }

}
