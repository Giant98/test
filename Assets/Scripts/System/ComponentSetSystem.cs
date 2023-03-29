using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Random = Unity.Mathematics.Random;
[UpdateAfter(typeof(ComponentEnabledTestSystem))]
[BurstCompile]
public partial struct ComponentSetSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        var job = new Test2Job
        {
            ECB = ecb
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    partial struct Test2Job : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public void Execute(Entity entity, [EntityIndexInQuery] int index)
        {
            ECB.SetComponent(index, entity, new EnemyComponent
            {
                IsAttacking = false
            });
        }
    }
}