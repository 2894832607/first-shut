using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鼠标移动控制视角
public class mousemove : MonoBehaviour
{
    public float mousespeed = 100f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float topcamera = -90f;
    public float bottomcamera = 90f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //获取鼠标移动的距离
        float mouseX = Input.GetAxis("Mouse X") * mousespeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mousespeed * Time.deltaTime;

        //旋转视角
        xRotation += mouseY;
        yRotation += mouseX;

        //限制视角
        xRotation = Mathf.Clamp(xRotation, topcamera, bottomcamera);

        //旋转视角
        transform.Rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
