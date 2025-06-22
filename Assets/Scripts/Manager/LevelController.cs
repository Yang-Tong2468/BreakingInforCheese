using UnityEngine;

/// <summary>
/// 控制单个关卡的逻辑。
/// 负责判断当前关卡的胜利/失败条件，并通知 GameManager
/// </summary>
public class LevelController : MonoBehaviour
{
    [Header("关卡胜利条件")]
    // 胜利条件可以是多种多样的，这里以收集奶酪为例
    private int totalCheesesInLevel;
    private int cheesesCollected = 0;

    [Header("关卡组件引用")]
    [SerializeField] private LevelExitPortal exitPortal; // 在编辑器中引用本关卡的传送门

    public bool isWin { get; private set; }  //是否可以过关——通过井盖到下一关
    [SerializeField]private bool isLevelOver = false;

    private void Awake()
    {
        // 检查是否在编辑器中设置了传送门
        if (exitPortal == null)
        {
            // 如果没有设置，可以尝试在场景中自动查找
            exitPortal = FindObjectOfType<LevelExitPortal>();
            if (exitPortal == null)
            {
                Debug.LogError($"关卡 '{gameObject.scene.name}' 中没有找到或设置传送门 (LevelExitPortal)！");
            }
        }
    }

    private void Start()
    {
        // 在关卡开始时，计算本关需要收集的奶酪数量
        totalCheesesInLevel = 1;

        if (totalCheesesInLevel == 0)
        {
            Debug.LogWarning($"关卡 '{gameObject.scene.name}' 中没有找到 'Cheese'，无法通过此方式获胜。");
        }
        else
        {
            Debug.Log($"关卡 '{gameObject.scene.name}' 开始！目标：收集 {totalCheesesInLevel} 个奶酪。");
        }
    }

    /// <summary>
    /// 当一个奶酪被收集时调用
    /// </summary>
    public void OnCheeseCollected()
    {
        if (isLevelOver) return;

        cheesesCollected++;
        CheckForWinCondition();
    }

    /// <summary>
    /// 当玩家失败时调用
    /// </summary>
    public void TriggerDefeat()
    {
        if (isLevelOver) return;
        isLevelOver = true;

        Debug.Log($"关卡 '{gameObject.scene.name}' 失败！");

        // 通知 GameManager 游戏失败了
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelLost();
        }
    }

    /// <summary>
    /// 检查当前关卡的胜利条件
    /// </summary>
    private void CheckForWinCondition()
    {
        // 如果收集数量达到总数，则胜利
        if (cheesesCollected >= totalCheesesInLevel)
        {
            isLevelOver = true;
            isWin = true;
            Debug.Log($"关卡 '{gameObject.scene.name}' 胜利！");

            // 通知 GameManager 游戏胜利了
            //if (GameManager.Instance != null)
            //{
            //    GameManager.Instance.OnLevelWon();
            //}

            // 不再直接通知 GameManager，而是激活传送门
            if (exitPortal != null)
            {
                Debug.Log("所有奶酪已收集！激活传送门。");
                exitPortal.ActivatePortal();
            }
        }
    }
}