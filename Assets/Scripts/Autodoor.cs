using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public Transform door; // 要移动的门
    public float moveDistance = 3f; // 门移动的距离
    public float speed = 2f; // 门移动的速度
    public AudioClip openSound; // 开门音效
    public AudioClip closeSound; // 关门音效
    private Vector3 closedPosition; // 门关闭时的位置
    private Vector3 openPosition; // 门打开时的位置
    private bool isOpen = false; // 门的状态

    private void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door Transform is not assigned in the inspector.");
            return;
        }

        closedPosition = door.position; // 初始化门关闭时的位置
        openPosition = closedPosition + new Vector3(moveDistance, 0, 0); // 计算门向右打开时的位置
    }

    // 当玩家进入触发器时
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 检查进入触发器的对象是否是玩家
        {
            OpenDoor();
        }
    }

    // 当玩家离开触发器时
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 检查退出触发器的对象是否是玩家
        {
            CloseDoor();
        }
    }

    // 打开门的逻辑
    private void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true; // 更新状态
            StopAllCoroutines(); // 停止任何正在进行的协程
            if (openSound != null)
            {
                AudioSource.PlayClipAtPoint(openSound, door.position);
            }
            StartCoroutine(MoveDoor(door.position, openPosition)); // 开门
        }
    }

    // 关闭门的逻辑
    private void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false; // 更新状态
            StopAllCoroutines(); // 停止任何正在进行的协程
            if (closeSound != null)
            {
                AudioSource.PlayClipAtPoint(closeSound, door.position);
            }
            StartCoroutine(MoveDoor(door.position, closedPosition)); // 关门
        }
    }

    // 平滑移动门
    private System.Collections.IEnumerator MoveDoor(Vector3 start, Vector3 target)
    {
        float journeyLength = Vector3.Distance(start, target);
        float startTime = Time.time;

        while (Vector3.Distance(door.position, target) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            door.position = Vector3.Lerp(start, target, fractionOfJourney);
            yield return null; // 等待下一帧
        }

        door.position = target; // 确保门移动到目标位置
        Debug.Log("Door moved to: " + target);
    }
}