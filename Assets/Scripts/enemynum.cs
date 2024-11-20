using UnityEngine;
using UnityEngine.UI; // �����Ҫ��ʾ�ı���Ϣ
using UnityEngine.SceneManagement; // �������¼��س���
using TMPro;
using Ammomanager111;

namespace enemydie111
{
    public class EnemyManager : MonoBehaviour
    {
        public int enemynum = 1 ;
        public GameObject victoryPanel; // ʤ����壨UI��
        //public AudioClip victorySound; // ʤ����Ч
        private AudioSource audioSource;
        private bool isGameOver = false; // ��Ϸ������־

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(false); // ����ʤ�����
            }
            else
            {
                Debug.LogError("Victory Panel δ����ֵ������ Inspector �и�ֵ��");
            }
        }

        public void enemydie()
        {
            enemynum -= 1;
            Ammomanager.Instance.ammodisplay4.text = $"{enemynum}";
        }

        private void Update()
        {
            if (enemynum == 0 && !isGameOver)
            {
                isGameOver = true; // ������Ϸ������־
                Invoke("gameover", 5f); // 5������gameover����
            }

            // ��� Enter ���Ƿ񱻰��²�����Ϸ�Ѿ�����
            if (isGameOver && Input.GetKeyDown(KeyCode.Return))
            {
                RestartGame();
            }
        }

        public void gameover()
        {
            Debug.Log("���е����ѱ����ܣ���Ӯ�ˣ�");

            // ��ʾʤ�����
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }

            // ����ʤ����Ч
            //if (victorySound != null && audioSource != null)
            //{
            //    audioSource.PlayOneShot(victorySound);
            //}

            // ֹͣ��Ϸ�е����л
            Time.timeScale = 0f; // ��ͣ��Ϸ
        }

        private void RestartGame()
        {
            Time.timeScale = 1f; // �ָ���Ϸ�ٶ�
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���¼��ص�ǰ����
        }
    }
}
