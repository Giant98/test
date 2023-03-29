using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public partial struct ComponentEnabledTestSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        Entity e = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponent<EnemyComponent>(e);
        state.EntityManager.SetComponentEnabled<EnemyComponent>(e, false);
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
        var job = new TestJob
        {
            ECB = ecb
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }

    [WithNone(typeof(EnemyComponent))]
    [BurstCompile]
    partial struct TestJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public void Execute(Entity entity, [EntityIndexInQuery] int index)
        {
            ECB.SetComponentEnabled<EnemyComponent>(index, entity, true);
            ECB.SetComponent(index, entity, new EnemyComponent
            {
                IsAttacking = false
            });
        }
    }
}