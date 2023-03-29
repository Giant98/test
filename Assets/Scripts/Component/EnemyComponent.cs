using Unity.Entities;
using Unity.Transforms;

public struct EnemyComponent : IComponentData, IEnableableComponent
{
    public bool IsAttacking;
}
