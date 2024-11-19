using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera;  // 你希望在开始时显示的相机
    public Camera[] otherCameras; // 其他相机
    public string[] layersToDisable = { "Hidden", "UI" }; // 要禁用的层名数组

    private int[] originalCullingMasks; // 保存其他相机的初始 Culling Masks

    private void Start()
    {
        // 保存其他相机的初始 Culling Masks
        originalCullingMasks = new int[otherCameras.Length];
        for (int i = 0; i < otherCameras.Length; i++)
        {
            if (otherCameras[i] != null)
            {
                originalCullingMasks[i] = otherCameras[i].cullingMask; // 保存初始值
                // 禁用指定层
                foreach (string layer in layersToDisable)
                {
                    otherCameras[i].cullingMask &= ~(1 << LayerMask.NameToLayer(layer)); // 阻止渲染指定层
                }
            }
        }

        // 确保主相机启用
        if (mainCamera != null)
        {
            mainCamera.enabled = true; // 启用主相机
        }
    }

    private void Update()
    {
        // 检测 Enter 键是否被按下
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 恢复其他相机的 Culling Mask
            for (int i = 0; i < otherCameras.Length; i++)
            {
                if (otherCameras[i] != null)
                {
                    otherCameras[i].cullingMask = originalCullingMasks[i]; // 恢复为初始 Culling Mask
                }
            }

            // 禁用主相机
            if (mainCamera != null)
            {
                mainCamera.enabled = false; // 禁用主相机
            }
        }
    }
}