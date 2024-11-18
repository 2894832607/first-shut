using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ƶ������ӽ�
public class Mousemove : MonoBehaviour
{
    public float mousespeed = 100f;
     float xRotation = 0f;
     float yRotation = 0f;
    public float topcmera = -90f;
    public float bottomcamera = 90f;  
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //��ȡ����ƶ��ľ���
        float mouseX = Input.GetAxis("Mouse X") * mousespeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mousespeed * Time.deltaTime;

        //��ת�ӽ�
        xRotation -= mouseY;
       

        //�����ӽ�
        xRotation = Mathf.Clamp(xRotation, topcmera,bottomcamera);

        yRotation += mouseX;

        //��ת�ӽ�

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
