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
        public float maxHealth = 99f; // 最大血量
        public float currentHealth; // 当前血量
        public Image damageOverlay; // 伤害覆盖层
        public Camera diecamera;
        public Camera playerCamera1; // 玩家相机
        public Camera playerCamera2; // 玩家相机
        public TextMeshProUGUI deathMessage1; // 死亡消息文本
        public TextMeshProUGUI deathMessage2; // 死亡消息文本
        public GameObject weapon; // 武器游戏物体
        public float overlayDuration = 0.5f; // 颜色持续时间
        public float overlayFadeSpeed = 2f; // 颜色淡出速度

        private Coroutine overlayCoroutine; // 覆盖层协程
        private bool isDead = false; // 玩家是否死亡的标志位

        void Start()
        {
            currentHealth = maxHealth; // 初始化当前血量
            if (damageOverlay != null)
            {
                var color = damageOverlay.color;
                color.a = 0;
                damageOverlay.color = color; // 初始化时隐藏覆盖层
            }
            if (deathMessage1 != null)
            {
                deathMessage1.gameObject.SetActive(false); // 初始化时隐藏死亡消息
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

            // 检查玩家是否按下Enter键重启游戏
            if (isDead && Input.GetKeyDown(KeyCode.Return))
            {
                RestartGame();
            }
        }

        public void TakeDamage(float damage)
        {
            if (!isDead)
            {
                currentHealth -= damage; // 扣除伤害
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 确保血量不小于0

                ShowDamageOverlay(); // 显示伤害覆盖层

                // 检查是否死亡
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
                    StopCoroutine(overlayCoroutine); // 停止当前的覆盖层协程
                }
                overlayCoroutine = StartCoroutine(DamageOverlayCoroutine()); // 启动新的覆盖层协程
            }
        }

        private IEnumerator DamageOverlayCoroutine()
        {
            var color = damageOverlay.color;
            color.a = 0.3f;
            damageOverlay.color = color; // 设置覆盖层为红色

            yield return new WaitForSeconds(overlayDuration); // 等待指定时间

            while (damageOverlay.color.a > 0)
            {
                color.a -= Time.deltaTime * overlayFadeSpeed;
                damageOverlay.color = color; // 渐变隐藏覆盖层
                yield return null;
            }
        }

        // 玩家死亡处理
        private void Die()
        {
            Debug.Log("玩家死亡！");
            isDead = true; // 设置玩家死亡标志位
            if (deathMessage1 != null)
            {
                diecamera.gameObject.SetActive(true); // 显示死亡相机
                playerCamera1.gameObject.SetActive(false); // 禁用玩家相机
                playerCamera2.gameObject.SetActive(false); // 禁用玩家相机
                deathMessage1.gameObject.SetActive(true); // 显示死亡消息
                deathMessage2.gameObject.SetActive(true); // 显示死亡消息
            }
            if (weapon != null)
            {
                weapon.SetActive(false); // 禁用武器游戏物体
            }
        }

        // 重启游戏
        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
        }
    }
}
