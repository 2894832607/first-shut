using UnityEngine;

// ����״̬��
public abstract class EnemyState
{
    public abstract void Enter(EnemyAI enemy);
    public abstract void Execute(EnemyAI enemy);
    public abstract void Exit(EnemyAI enemy);
}

// Ѳ��״̬
public class PatrolState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.StartPatrolling(); // ��ʼ��Ѳ��״̬
    }

    public override void Execute(EnemyAI enemy)
    {
        enemy.Patrol(); // ִ��Ѳ����Ϊ
        if (enemy.IsPlayerInRange())
        {   
            enemy.ChangeState(new ChaseState()); // �л���׷��״̬
          
        }
    }

    public override void Exit(EnemyAI enemy)
    {
        // ����Ѳ��״̬
    }
}

// ׷��״̬
public class ChaseState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.StartChase();
    }

    public override void Execute(EnemyAI enemy)
    {
        enemy.ChasePlayer(); // ִ��׷����Ϊ
        if (!enemy.IsPlayerInRange())
        {   
            enemy.ChangeState(new PatrolState()); // �л���Ѳ��״̬
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
        // ����׷��״̬
    }
}

// ����״̬
public class AttackState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.StartAttacking(); // ��ʼ������״̬
    }

    public override void Execute(EnemyAI enemy)
    {
        enemy.ChasePlayer(); // ִ��׷����Ϊ
        enemy.AttackCoroutine();
        if (!enemy.IsPlayerInRange())
        {   
            enemy.StopAttacking();
            enemy.ChangeState(new PatrolState()); // �л���Ѳ��״̬
            enemy.animator11.SetTrigger("WALK");
            enemy.bite1 = false;
            enemy.bite2 = false;
        }
    }

    public override void Exit(EnemyAI enemy)
    {
        // ������״̬
    }
}

// ����״̬
public class DeathState : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        enemy.die(); // ��ʼ������״̬
    }

    public override void Execute(EnemyAI enemy)
    {
        // ����״̬�²�ִ���κ���Ϊ
    }

    public override void Exit(EnemyAI enemy)
    {
        // ��������״̬
    }
}
