using UnityEngine;
using UnityEngine.UI; // 如果需要显示文本信息
using UnityEngine.SceneManagement; // 用于重新加载场景
using TMPro;
using Ammomanager111;

namespace enemydie111
{
    public class EnemyManager : MonoBehaviour
    {
        public int enemynum = 1 ;
        public GameObject victoryPanel; // 胜利面板（UI）
        //public AudioClip victorySound; // 胜利音效
        private AudioSource audioSource;
        private bool isGameOver = false; // 游戏结束标志

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(false); // 隐藏胜利面板
            }
            else
            {
                Debug.LogError("Victory Panel 未被赋值，请在 Inspector 中赋值。");
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
                isGameOver = true; // 设置游戏结束标志
                Invoke("gameover", 5f); // 5秒后调用gameover方法
            }

            // 检测 Enter 键是否被按下并且游戏已经结束
            if (isGameOver && Input.GetKeyDown(KeyCode.Return))
            {
                RestartGame();
            }
        }

        public void gameover()
        {
            Debug.Log("所有敌人已被击败！你赢了！");

            // 显示胜利面板
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }

            // 播放胜利音效
            //if (victorySound != null && audioSource != null)
            //{
            //    audioSource.PlayOneShot(victorySound);
            //}

            // 停止游戏中的所有活动
            Time.timeScale = 0f; // 暂停游戏
        }

        private void RestartGame()
        {
            Time.timeScale = 1f; // 恢复游戏速度
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 重新加载当前场景
        }
    }
}
