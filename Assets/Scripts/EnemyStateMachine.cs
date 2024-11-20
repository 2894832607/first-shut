using UnityEngine;

// ³éÏó×´Ì¬Àà
public abstract class EnemyState
{
    public abstract void Enter(EnemyAI enemy);
    public abstract void Execute(EnemyAI enemy);
    public abstract void Exit(EnemyAI enemy);
}

// Ñ²Âß×´Ì¬
public class PatrolState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.StartPatrolling(); // ³õÊ¼»¯Ñ²Âß×´Ì¬
    }

    public override void Execute(EnemyAI enemy)
    {
        enemy.Patrol(); // Ö´ĞĞÑ²ÂßĞĞÎª
        if (enemy.IsPlayerInRange())
        {   
            enemy.ChangeState(new ChaseState()); // ÇĞ»»µ½×·×Ù×´Ì¬
          
        }
    }

    public override void Exit(EnemyAI enemy)
    {
        // ÇåÀíÑ²Âß×´Ì¬
    }
}

// ×·×Ù×´Ì¬
public class ChaseState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.StartChase();
    }

    public override void Execute(EnemyAI enemy)
    {
        enemy.ChasePlayer(); // Ö´ĞĞ×·×ÙĞĞÎª
        if (!enemy.IsPlayerInRange())
        {   
            enemy.ChangeState(new PatrolState()); // ÇĞ»»»ØÑ²Âß×´Ì¬
            enemy.animator11.SetTrigger("WALK");
            enemy.bite1 = false;
            enemy.bite2 = false;
        }
        if (enemy.IsPlayerInAttackRange())
        {
            
            enemy.ChangeState(new AttackState());
        }
    }

    public override void Exit(EnemyAI enemy)
    {
        // ÇåÀí×·×Ù×´Ì¬
    }
}

// ¹¥»÷×´Ì¬
public class AttackState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.StartAttacking(); // ³õÊ¼»¯¹¥»÷×´Ì¬
    }

    public override void Execute(EnemyAI enemy)
    {
        enemy.ChasePlayer(); // Ö´ĞĞ×·×ÙĞĞÎª
        enemy.AttackCoroutine();
        if (!enemy.IsPlayerInRange())
        {   
            enemy.StopAttacking();
            enemy.ChangeState(new PatrolState()); // ÇĞ»»»ØÑ²Âß×´Ì¬
            enemy.animator11.SetTrigger("WALK");
            enemy.bite1 = false;
            enemy.bite2 = false;
        }
    }

    public override void Exit(EnemyAI enemy)
    {
        // ÇåÀí¹¥»÷×´Ì¬
    }
}

// ËÀÍö×´Ì¬
public class DeathState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.die(); // ³õÊ¼»¯ËÀÍö×´Ì¬
    }

    public override void Execute(EnemyAI enemy)
    {
        // ËÀÍö×´Ì¬ÏÂ²»Ö´ĞĞÈÎºÎĞĞÎª
    }

    public override void Exit(EnemyAI enemy)
    {
        // ÇåÀíËÀÍö×´Ì¬
    }
}
