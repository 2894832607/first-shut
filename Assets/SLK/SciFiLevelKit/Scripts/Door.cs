using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float translateValue;
    public float easeTime;
    public float waitTime;
    public float triggerDistance = 2f; // 距离玩家的触发距离
    public bool moveRight = true; // 控制门向左还是向右位移
    public float moveDistance = 1f; // 控制位移多少

    private Vector3 StartlocalPos;
    private GameObject player;
    private bool isOpen = false; // 标记门是否已经打开
    private bool isMoving = false; // 标记门是否正在移动

    private void Start()
    {
        StartlocalPos = transform.localPosition;
        gameObject.isStatic = false;
        player = GameObject.FindGameObjectWithTag("Player"); // 假设玩家有一个标签为"Player"
    }

    private void Update()
    {
        if (!isOpen && !isMoving && Vector3.Distance(transform.position, player.transform.position) < triggerDistance)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        isOpen = true; // 标记门已经打开
        isMoving = true; // 标记门正在移动
        Vector3 targetPos = StartlocalPos + new Vector3(moveRight ? moveDistance : -moveDistance, 0, 0);
        StartCoroutine(MoveDoor(StartlocalPos, targetPos, easeTime, () =>
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(WaitToClose());
        }));
    }

    private IEnumerator WaitToClose()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(MoveDoor(transform.localPosition, StartlocalPos, easeTime, () =>
        {
            GetComponent<AudioSource>().Play();
            isOpen = false; // 标记门已经关闭
            isMoving = false; // 标记门不再移动
        }));
    }

    private IEnumerator MoveDoor(Vector3 from, Vector3 to, float duration, System.Action onComplete)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = to;
        onComplete?.Invoke();
    }
}


