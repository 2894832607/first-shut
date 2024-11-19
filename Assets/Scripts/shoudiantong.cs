using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // �ֵ�Ͳ��Դ
    public Transform playerCamera; // ���������� Transform
    private bool isOn = false;

    void Update()
    {
        // ���� F ���л��ֵ�Ͳ״̬
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn; // �л���Դ������״̬
        }

        // ���ֵ�Ͳ��λ�ú���ת�������һ��
        flashlight.transform.position = playerCamera.position; // ���ù�Դλ��
        flashlight.transform.rotation = playerCamera.rotation; // ���ù�Դ����
    }
}