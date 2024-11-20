using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Rendering.LookDev;
#endif
using UnityEngine;
using CameraShake111;
//移动脚本
public class playermove : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumphight = 5.0f;
    public float gravity = -9.8f ;
    public Transform groundcheck;
    public LayerMask groundmask;
    private CharacterController controller;
    public float grounddistance = 0.4f;
    Vector3 Velocity;

    bool isgrounded;
    private Vector3 lastposition = new Vector3(0f, 0f, 0f);

    private CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        isgrounded = Physics.CheckSphere(groundcheck.position, grounddistance, groundmask);
        if (isgrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        //float currentSpeed = isRunning ? speed * 1.5f : speed;

        float currentSpeed;
        if(isRunning&&isgrounded)
        {
            currentSpeed = speed * 2f;
        }
        else
        {
            currentSpeed = speed;
        }
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (isgrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Velocity.y = Mathf.Sqrt(jumphight * -2f * gravity);
        }

        Velocity.y += gravity * Time.deltaTime;

        controller.Move(Velocity * Time.deltaTime);

        lastposition = gameObject.transform.position;

        // 调用相机晃动
        if (move.magnitude > 0)
        {
            cameraShake.Shake(isRunning);
        }
        else
        {
            cameraShake.ResetPosition();
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////移动脚本
//public class playermove : MonoBehaviour
//{

//    public float speed = 5.0f;
//    public float jumphight = 5.0f;
//    public float gravity = -9.8f * 2;
//    public Transform groundcheck;
//    public LayerMask groundmask;
//    private CharacterController controller;
//    public float grounddistance = 0.4f;
//    Vector3 Velocity;

//    bool isgrounded;
//    private Vector3 lastposition = new Vector3(0f, 0f, 0f);


//    // Start is called before the first frame update
//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        isgrounded = Physics.CheckSphere(groundcheck.position, grounddistance, groundmask);
//        if (isgrounded && Velocity.y < 0)
//        {
//            Velocity.y = -2f;
//        }

//        float x = Input.GetAxis("Horizontal");
//        float z = Input.GetAxis("Vertical");
//        Vector3 move = transform.right * x + transform.forward * z;
//        controller.Move(move * speed * Time.deltaTime);

//        if (isgrounded && Input.GetButtonDown("Jump"))
//        {
//            Velocity.y = Mathf.Sqrt(jumphight * -2f * gravity);
//        }

//        Velocity.y += gravity * Time.deltaTime;

//        controller.Move(Velocity * Time.deltaTime);

//        lastposition = gameObject.transform.position;

//    }


//}
