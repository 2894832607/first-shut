using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public Transform door; // Ҫ�ƶ�����
    public float moveDistance = 3f; // ���ƶ��ľ���
    public float speed = 2f; // ���ƶ����ٶ�
    public AudioClip openSound; // ������Ч
    public AudioClip closeSound; // ������Ч
    private Vector3 closedPosition; // �Źر�ʱ��λ��
    private Vector3 openPosition; // �Ŵ�ʱ��λ��
    private bool isOpen = false; // �ŵ�״̬

    private void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door Transform is not assigned in the inspector.");
            return;
        }

        closedPosition = door.position; // ��ʼ���Źر�ʱ��λ��
        openPosition = closedPosition + new Vector3(moveDistance, 0, 0); // ���������Ҵ�ʱ��λ��
    }

    // ����ҽ��봥����ʱ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �����봥�����Ķ����Ƿ������
        {
            OpenDoor();
        }
    }

    // ������뿪������ʱ
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // ����˳��������Ķ����Ƿ������
        {
            CloseDoor();
        }
    }

    // ���ŵ��߼�
    private void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true; // ����״̬
            StopAllCoroutines(); // ֹͣ�κ����ڽ��е�Э��
            if (openSound != null)
            {
                AudioSource.PlayClipAtPoint(openSound, door.position);
            }
            StartCoroutine(MoveDoor(door.position, openPosition)); // ����
        }
    }

    // �ر��ŵ��߼�
    private void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false; // ����״̬
            StopAllCoroutines(); // ֹͣ�κ����ڽ��е�Э��
            if (closeSound != null)
            {
                AudioSource.PlayClipAtPoint(closeSound, door.position);
            }
            StartCoroutine(MoveDoor(door.position, closedPosition)); // ����
        }
    }

    // ƽ���ƶ���
    private System.Collections.IEnumerator MoveDoor(Vector3 start, Vector3 target)
    {
        float journeyLength = Vector3.Distance(start, target);
        float startTime = Time.time;

        while (Vector3.Distance(door.position, target) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            door.position = Vector3.Lerp(start, target, fractionOfJourney);
            yield return null; // �ȴ���һ֡
        }

        door.position = target; // ȷ�����ƶ���Ŀ��λ��
        Debug.Log("Door moved to: " + target);
    }
}