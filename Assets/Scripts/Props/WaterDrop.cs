using UnityEngine;
using UnityEngine.SceneManagement; 

/// <summary>
/// 控制水滴的行为。当它碰到带有 "Player" 标签的物体时，
/// 会触发游戏结束的逻辑
/// </summary>
public class WaterDrop : MonoBehaviour
{
    [Header("效果设置")]
    [SerializeField] private GameObject splashEffectPrefab; // 碰到玩家时播放的“水花”特效

    // OnCollisionEnter2D 会在两个带有 Collider2D 的刚体碰撞时被调用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 销毁水滴自身
        Destroy(gameObject);

        // 检查撞到的物体是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // --- 游戏结束逻辑 ---
            Debug.Log("水滴碰到了玩家！游戏结束。");


            // 1. 获取玩家对象，并可能禁用它的控制器，防止其在游戏结束画面继续移动
            GameObject playerObject = collision.gameObject;
            WallWalkerController playerController = playerObject.GetComponent<WallWalkerController>();

            GameManager.Instance.PauseGame(); // 暂停游戏

            //通知manager失败了，弹出失败界面
            GameManager.Instance.CurrentLevelController.TriggerDefeat();


            if (playerController != null)
            {
                playerController.enabled = false; // 禁用玩家的控制脚本
            }
            // 也可以直接冻结玩家的刚体
            Rigidbody2D playerRb = playerObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
                playerRb.bodyType = RigidbodyType2D.Static;
            }

            // 2. 在碰撞点播放水花特效
            if (splashEffectPrefab != null)
            {
                Instantiate(splashEffectPrefab, collision.contacts[0].point, Quaternion.identity);
            }

            // 4. 调用一个全局的游戏结束管理器，或者直接延迟后重启关卡
            // 为了演示，我们直接延迟1.5秒后重新加载当前场景
            //Invoke("RestartLevel", 1.5f);
        }
    }

    // 一个简单的方法，用于重新加载当前活动的场景
    private void RestartLevel()
    {
        // 获取当前活动场景的索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 重新加载该场景
        SceneManager.LoadScene(currentSceneIndex);
    }
}