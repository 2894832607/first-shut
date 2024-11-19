using UnityEngine;
using UnityEngine.AI;
using playheart111;
using System.Collections;
using enemydie111;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // ��Ҷ���
    private NavMeshAgent agent; // NavMesh Agent ���
    private EnemyState currentState; // ��ǰ״̬

    public GameObject[] patrolPoints; // Ѳ�ߵ�����
    private int currentPatrolIndex; // ��ǰѲ�ߵ�����

    public float detectionRange = 10f; // ��ⷶΧ
    public float attackRange = 2f; // ������Χ

    public float patrolRadius = 2f; // Ѳ�߷�Χ�뾶
    private Vector3 patrolTarget; // ��ǰѲ��Ŀ��
    public float patrolspeed = 1f;
    public float chasespeed = 6f;
    public float attackDamage = 10f; // �����˺�
    public float attackrange = 4f; // ������Χ
    public float attackSpeed = 1f; // �����ٶȣ�ÿ�빥��������
    public float attackInterval = 2f; // �������ʱ��
    private bool isAttacking = false; // �Ƿ����ڹ���
    public float enemyhealth = 100f;
    public Animator animator11;
    public float dietime = 5f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new PatrolState()); // ��ʼ״̬ΪѲ��
        animator11 = GetComponent<Animator>();
        SetRandomInitialAnimation();


    }
   

    void Update()
    {
        currentState.Execute(this); // ÿ֡���õ�ǰ״̬�� Execute ����

        IsPlayerInRange();

        IsPlayerInAttackRange();
    }

    public void SetRandomInitialAnimation()
    {
        int randomAnimation = Random.Range(0, 4); // ���ѡ�� 0, 1, �� 2
        switch (randomAnimation)
        {
            case 0:
                animator11.SetTrigger("BITE1"); // ���� BITE1 ����
                patrolspeed = 0f;
                break;
            case 1:
                animator11.SetTrigger("BITE2"); // ���� BITE2 ����
                patrolspeed = 0f;
                break;
            case 2:
                animator11.SetTrigger("CRAWL"); // ���� CRAWL ����
                patrolspeed = 0.5f;
                break;
            case 3:
               animator11.SetTrigger("WALK"); // ���� CRAWL ����
               break;
        }
    }
        public void ChangeState(EnemyState newState)
    {
        currentState?.Exit(this); // �˳���ǰ״̬
        currentState = newState; // �л�����״̬
        currentState.Enter(this); // ������״̬
    }

    public bool IsPlayerInRange()
    {

       
        return Vector3.Distance(transform.position, player.position) <= detectionRange; // �������Ƿ�����ⷶΧ��
    }
    public bool IsPlayerInAttackRange()
    {
        
        return Vector3.Distance(transform.position, player.position) <= attackrange; // �������Ƿ�����ⷶΧ��
    }

    public void StartPatrolling()
    {
        
        agent.speed = patrolspeed;
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("û������Ѳ�ߵ㣡");
            return;
        }
        currentPatrolIndex = 0; // �ӵ�һ��Ѳ�ߵ㿪ʼ
        SetRandomPatrolTarget(); // ���ó�ʼĿ��
    }

    public void Patrol()
    {

     

        // ����Ƿ񵽴ﵱǰѲ��Ŀ��
        if (Vector3.Distance(transform.position, patrolTarget) <1f)
        {
            SetRandomPatrolTarget(); // ����Ŀ��������µ����Ŀ��

        }

        // ʹ�� NavMeshAgent �ƶ���Ŀ��λ��
        agent.SetDestination(patrolTarget);
    }

    private void SetRandomPatrolTarget()
    {
        // ��ȡ��ǰѲ�ߵ�λ��
        Vector3 center = patrolPoints[currentPatrolIndex].transform.position;

        // ����һ������Ƕ�
        float randomAngle = Random.Range(0f, 360f);

        // �����µ�Ŀ��λ��
        float xOffset = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * patrolRadius;
        float zOffset = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * patrolRadius;

        patrolTarget = new Vector3(center.x + xOffset, center.y, center.z + zOffset);

        // ���� NavMeshAgent ��Ŀ��
        agent.SetDestination(patrolTarget);
    }

    public void StartChase()
    {

        animator11.SetTrigger("RUN");
        agent.speed = chasespeed; // ����׷���ٶ�
    }

    public void ChasePlayer()
    {
        if (IsPlayerInRange())
        {
            agent.SetDestination(player.position); // ����Ŀ��Ϊ��ҵ�λ��
        }
    }

    public void StartAttacking()
    {
        Debug.Log("׼��������ң�");
        if (!isAttacking)
        {
            StartCoroutine(AttackCoroutine()); // ��ʼ����Э��
        }
    }

    public IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while (isAttacking) // ����������ѭ��
        {
            AttackPlayer(); // ִ�й���

            // �ȴ��������
            yield return new WaitForSeconds(attackInterval); // ʹ�ù����������
        }
    }

    public void AttackPlayer()
    {
        // �������Ƿ��ڹ�����Χ��
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // ���������һ�� playheart �ű�
            playheart playerHealth = player.GetComponent<playheart>();
            if (playerHealth != null)
            {
                animator11.SetTrigger("ATTACK");
                playerHealth.TakeDamage(attackDamage); // ���������˺�
                Debug.Log("������ң�����˺�: " + attackDamage);
            }
        }
        else
        {
            Debug.Log("��Ҳ��ڹ�����Χ�ڣ�ֹͣ������");
            StopAttacking(); // �����Ҳ��ڷ�Χ�ڣ�ֹͣ����
            animator11.SetTrigger("STOPATTACK");
        }
    }

    public void StopAttacking()
    {
        //isAttacking = false; // ֹͣ����
        //StopCoroutine(AttackCoroutine()); // ֹͣЭ��
    }

    public void die()
    {
        Debug.Log("����������");
        agent.isStopped = true; // ֹͣ����
        StopAllCoroutines(); // ֹͣ����Э��
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.enemydie();
        }

        // ���õ��˵���ײ����������
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
        Destroy(gameObject); // ֱ�����ٵ��˶���
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
        ChangeColor(Color.red*0.4f); // ���
        yield return new WaitForSeconds(0.8f); // �ȴ�0.1��
        ChangeColor(Color.white); // �ָ�ԭ������ɫ
    }

    void OnCollisionEnter(Collision collision)
    {
        // �����ײ�����Ƿ����ӵ�
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            TakeDamage(bullet.damage); // ��Ѫ
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
