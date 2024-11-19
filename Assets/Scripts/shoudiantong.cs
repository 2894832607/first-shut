using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // 手电筒光源
    public Transform playerCamera; // 玩家摄像机的 Transform
    private bool isOn = false;

    void Update()
    {
        // 按下 F 键切换手电筒状态
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn; // 切换光源的启用状态
        }

        // 让手电筒的位置和旋转与摄像机一致
        flashlight.transform.position = playerCamera.position; // 设置光源位置
        flashlight.transform.rotation = playerCamera.rotation; // 设置光源方向
    }
}