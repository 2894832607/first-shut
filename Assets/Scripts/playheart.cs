using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ammomanager111;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace playheart111
{
    public class playheart : MonoBehaviour
    {
        public float maxHealth = 99f; // ���Ѫ��
        public float currentHealth; // ��ǰѪ��
        public Image damageOverlay; // �˺����ǲ�
        public Camera diecamera;
        public Camera playerCamera1; // ������
        public Camera playerCamera2; // ������
        public TextMeshProUGUI deathMessage1; // ������Ϣ�ı�
        public TextMeshProUGUI deathMessage2; // ������Ϣ�ı�
        public GameObject weapon; // ������Ϸ����
        public float overlayDuration = 0.5f; // ��ɫ����ʱ��
        public float overlayFadeSpeed = 2f; // ��ɫ�����ٶ�

        private Coroutine overlayCoroutine; // ���ǲ�Э��
        private bool isDead = false; // ����Ƿ������ı�־λ

        void Start()
        {
            currentHealth = maxHealth; // ��ʼ����ǰѪ��
            if (damageOverlay != null)
            {
                var color = damageOverlay.color;
                color.a = 0;
                damageOverlay.color = color; // ��ʼ��ʱ���ظ��ǲ�
            }
            if (deathMessage1 != null)
            {
                deathMessage1.gameObject.SetActive(false); // ��ʼ��ʱ����������Ϣ
            }
        }

        void Update()
        {
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
            if (Ammomanager.Instance.ammodisplay1 != null)
            {
                Ammomanager.Instance.ammodisplay3.text = $"{currentHealth}";
            }

            // �������Ƿ���Enter��������Ϸ
            if (isDead && Input.GetKeyDown(KeyCode.Return))
            {
                RestartGame();
            }
        }

        public void TakeDamage(float damage)
        {
            if (!isDead)
            {
                currentHealth -= damage; // �۳��˺�
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ȷ��Ѫ����С��0

                ShowDamageOverlay(); // ��ʾ�˺����ǲ�

                // ����Ƿ�����
                if (currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        private void ShowDamageOverlay()
        {
            if (damageOverlay != null)
            {
                if (overlayCoroutine != null)
                {
                    StopCoroutine(overlayCoroutine); // ֹͣ��ǰ�ĸ��ǲ�Э��
                }
                overlayCoroutine = StartCoroutine(DamageOverlayCoroutine()); // �����µĸ��ǲ�Э��
            }
        }

        private IEnumerator DamageOverlayCoroutine()
        {
            var color = damageOverlay.color;
            color.a = 0.3f;
            damageOverlay.color = color; // ���ø��ǲ�Ϊ��ɫ

            yield return new WaitForSeconds(overlayDuration); // �ȴ�ָ��ʱ��

            while (damageOverlay.color.a > 0)
            {
                color.a -= Time.deltaTime * overlayFadeSpeed;
                damageOverlay.color = color; // �������ظ��ǲ�
                yield return null;
            }
        }

        // �����������
        private void Die()
        {
            Debug.Log("���������");
            isDead = true; // �������������־λ
            if (deathMessage1 != null)
            {
                diecamera.gameObject.SetActive(true); // ��ʾ�������
                playerCamera1.gameObject.SetActive(false); // ����������
                playerCamera2.gameObject.SetActive(false); // ����������
                deathMessage1.gameObject.SetActive(true); // ��ʾ������Ϣ
                deathMessage2.gameObject.SetActive(true); // ��ʾ������Ϣ
            }
            if (weapon != null)
            {
                weapon.SetActive(false); // ����������Ϸ����
            }
        }

        // ������Ϸ
        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���¼��ص�ǰ����
        }
    }
}
