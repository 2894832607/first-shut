using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraShake111
{
    public class CameraShake : MonoBehaviour
    {
        public float walkShakeAmount = 0.1f; // ��·ʱ�Ļζ�����
        public float runShakeAmount = 0.2f;  // �ܲ�ʱ�Ļζ�����
        public float walkShakeSpeed = 1.0f;  // ��·ʱ�Ļζ��ٶ�
        public float runShakeSpeed = 2.0f;   // �ܲ�ʱ�Ļζ��ٶ�

        private Vector3 originalPos;
        private float shakeTimer;

        void Start()
        {
            originalPos = transform.localPosition;
        }

        public void Shake(bool isRunning)
        {
            float shakeAmount = isRunning ? runShakeAmount : walkShakeAmount;
            float shakeSpeed = isRunning ? runShakeSpeed : walkShakeSpeed;

            shakeTimer += Time.deltaTime * shakeSpeed;
            float xShake = Mathf.Sin(shakeTimer) * shakeAmount;
            float yShake = Mathf.Cos(shakeTimer * 2) * shakeAmount;

            Vector3 shakePos = originalPos + new Vector3(xShake, yShake, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, shakePos, Time.deltaTime * shakeSpeed);
        }

        public void ResetPosition()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * walkShakeSpeed);
            shakeTimer = 0; // ���ü�ʱ��
        }
    }
}
    