using UnityEngine;
using UnityEngine.AI;
using playheart111;
using System.Collections;
using enemydie111;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // 玩家对象
    private NavMeshAgent agent; // NavMesh Agent 组件
    private EnemyState currentState; // 当前状态

    public GameObject[] patrolPoints; // 巡逻点数组
    private int currentPatrolIndex; // 当前巡逻点索引

    public float detectionRange = 10f; // 侦测范围
    public float attackRange = 2f; // 攻击范围

    public float patrolRadius = 2f; // 巡逻范围半径
    private Vector3 patrolTarget; // 当前巡逻目标
    public float patrolspeed = 1f;
    public float chasespeed = 6f;
    public float attackDamage = 10f; // 攻击伤害
    public float attackrange = 4f; // 攻击范围
    public float attackSpeed = 1f; // 攻击速度（每秒攻击次数）
    public float attackInterval = 2f; // 攻击间隔时间
    private bool isAttacking = false; // 是否正在攻击
    public float enemyhealth = 100f;
    public Animator animator11;
    public float dietime = 5f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new PatrolState()); // 初始状态为巡逻
        animator11 = GetComponent<Animator>();
        SetRandomInitialAnimation();


    }
   

    void Update()
    {
        currentState.Execute(this); // 每帧调用当前状态的 Execute 方法

        IsPlayerInRange();

        IsPlayerInAttackRange();
    }

    public void SetRandomInitialAnimation()
    {
        int randomAnimation = Random.Range(0, 4); // 随机选择 0, 1, 或 2
        switch (randomAnimation)
        {
            case 0:
                animator11.SetTrigger("BITE1"); // 触发 BITE1 动画
                patrolspeed = 0f;
                break;
            case 1:
                animator11.SetTrigger("BITE2"); // 触发 BITE2 动画
                patrolspeed = 0f;
                break;
            case 2:
                animator11.SetTrigger("CRAWL"); // 触发 CRAWL 动画
                patrolspeed = 0.5f;
                break;
            case 3:
               animator11.SetTrigger("WALK"); // 触发 CRAWL 动画
               break;
        }
    }
        public void ChangeState(EnemyState newState)
    {
        currentState?.Exit(this); // 退出当前状态
        currentState = newState; // 切换到新状态
        currentState.Enter(this); // 进入新状态
    }

    public bool IsPlayerInRange()
    {

       
        return Vector3.Distance(transform.position, player.position) <= detectionRange; // 检查玩家是否在侦测范围内
    }
    public bool IsPlayerInAttackRange()
    {
        
        return Vector3.Distance(transform.position, player.position) <= attackrange; // 检查玩家是否在侦测范围内
    }

    public void StartPatrolling()
    {
        
        agent.speed = patrolspeed;
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("没有设置巡逻点！");
            return;
        }
        currentPatrolIndex = 0; // 从第一个巡逻点开始
        SetRandomPatrolTarget(); // 设置初始目标
    }

    public void Patrol()
    {

     

        // 检查是否到达当前巡逻目标
        if (Vector3.Distance(transform.position, patrolTarget) <1f)
        {
            SetRandomPatrolTarget(); // 到达目标后生成新的随机目标

        }

        // 使用 NavMeshAgent 移动到目标位置
        agent.SetDestination(patrolTarget);
    }

    private void SetRandomPatrolTarget()
    {
        // 获取当前巡逻点位置
        Vector3 center = patrolPoints[currentPatrolIndex].transform.position;

        // 生成一个随机角度
        float randomAngle = Random.Range(0f, 360f);

        // 计算新的目标位置
        float xOffset = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * patrolRadius;
        float zOffset = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * patrolRadius;

        patrolTarget = new Vector3(center.x + xOffset, center.y, center.z + zOffset);

        // 设置 NavMeshAgent 的目标
        agent.SetDestination(patrolTarget);
    }

    public void StartChase()
    {

        animator11.SetTrigger("RUN");
        agent.speed = chasespeed; // 设置追逐速度
    }

    public void ChasePlayer()
    {
        if (IsPlayerInRange())
        {
            agent.SetDestination(player.position); // 设置目标为玩家的位置
        }
    }

    public void StartAttacking()
    {
        Debug.Log("准备攻击玩家！");
        if (!isAttacking)
        {
            StartCoroutine(AttackCoroutine()); // 开始攻击协程
        }
    }

    public IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while (isAttacking) // 持续攻击的循环
        {
            AttackPlayer(); // 执行攻击

            // 等待攻击间隔
            yield return new WaitForSeconds(attackInterval); // 使用攻击间隔变量
        }
    }

    public void AttackPlayer()
    {
        // 检查玩家是否在攻击范围内
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // 假设玩家有一个 playheart 脚本
            playheart playerHealth = player.GetComponent<playheart>();
            if (playerHealth != null)
            {
                animator11.SetTrigger("ATTACK");
                playerHealth.TakeDamage(attackDamage); // 对玩家造成伤害
                Debug.Log("攻击玩家！造成伤害: " + attackDamage);
            }
        }
        else
        {
            Debug.Log("玩家不在攻击范围内，停止攻击。");
            StopAttacking(); // 如果玩家不在范围内，停止攻击
            animator11.SetTrigger("STOPATTACK");
        }
    }

    public void StopAttacking()
    {
        //isAttacking = false; // 停止攻击
        //StopCoroutine(AttackCoroutine()); // 停止协程
    }

    public void die()
    {
        Debug.Log("敌人死亡！");
        agent.isStopped = true; // 停止导航
        StopAllCoroutines(); // 停止所有协程
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.enemydie();
        }

        // 禁用敌人的碰撞体和其他组件
        Collider[] colliders = GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        animator11.SetTrigger("DIE");
        Invoke("destroy1", dietime);
    }
    public void destroy1()
    {
        Destroy(gameObject); // 直接销毁敌人对象
    }
    public void TakeDamage(float damage)
    {
        enemyhealth -= damage;
        if (enemyhealth <= 0)
        {
            ChangeState(new DeathState());
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        ChangeColor(Color.red*0.4f); // 变红
        yield return new WaitForSeconds(0.8f); // 等待0.1秒
        ChangeColor(Color.white); // 恢复原来的颜色
    }

    void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞到的是否是子弹
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            TakeDamage(bullet.damage); // 掉血
        }
    }

    private void ChangeColor(Color color)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                material.color = color;
            }
        }
    }
}
