using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera;  // ��ϣ���ڿ�ʼʱ��ʾ�����
    public Camera[] otherCameras; // �������
    public string[] layersToDisable = { "Hidden", "UI" }; // Ҫ���õĲ�������

    private int[] originalCullingMasks; // ������������ĳ�ʼ Culling Masks

    private void Start()
    {
        // ������������ĳ�ʼ Culling Masks
        originalCullingMasks = new int[otherCameras.Length];
        for (int i = 0; i < otherCameras.Length; i++)
        {
            if (otherCameras[i] != null)
            {
                originalCullingMasks[i] = otherCameras[i].cullingMask; // �����ʼֵ
                // ����ָ����
                foreach (string layer in layersToDisable)
                {
                    otherCameras[i].cullingMask &= ~(1 << LayerMask.NameToLayer(layer)); // ��ֹ��Ⱦָ����
                }
            }
        }

        // ȷ�����������
        if (mainCamera != null)
        {
            mainCamera.enabled = true; // ���������
        }
    }

    private void Update()
    {
        // ��� Enter ���Ƿ񱻰���
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // �ָ���������� Culling Mask
            for (int i = 0; i < otherCameras.Length; i++)
            {
                if (otherCameras[i] != null)
                {
                    otherCameras[i].cullingMask = originalCullingMasks[i]; // �ָ�Ϊ��ʼ Culling Mask
                }
            }

            // ���������
            if (mainCamera != null)
            {
                mainCamera.enabled = false; // ���������
            }
        }
    }
}