using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 关卡管理器 
/// 负责处理当前关卡的胜利、失败条件，并控制UI显示和场景切换
/// </summary>
public class LevelManager : MonoBehaviour
{
    // --- 单例模式实现 ---
    public static LevelManager Instance { get; private set; }

    [Header("关卡UI面板")]
    [SerializeField] private GameObject victoryPanel;   // 胜利时显示的UI面板
    [SerializeField] private GameObject defeatPanel;    // 失败时显示的UI面板

    [Header("关卡胜利条件")]
    // 这里我们用一个简单的例子：收集到所有奶酪
    private int totalCheesesInLevel;
    private int cheesesCollected = 0;

    private bool isLevelOver = false; // 标志位，防止重复触发胜利或失败

    private void Awake()
    {
        // --- 单例模式的标准实现 ---
        if (Instance == null)
        {
            Instance = this;
            // (可选) DontDestroyOnLoad(gameObject); // 如果你希望管理器跨场景存在
        }
        else
        {
            Destroy(gameObject); // 如果已存在实例，则销毁自己
        }
    }

    private void Start()
    {
        // 游戏开始时，禁用UI面板
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);

        // 初始化胜利条件
        // 找到场景中所有带有 "Cheese" 标签的物体
        totalCheesesInLevel = GameObject.FindGameObjectsWithTag("Cheese").Length;
        Debug.Log($"本关共有 {totalCheesesInLevel} 个奶酪需要收集。");

        // 如果关卡开始时就没有奶酪，直接算胜利（或报错，取决于你的设计）
        if (totalCheesesInLevel == 0)
        {
            Debug.LogWarning("关卡中没有找到 'Cheese' 标签的物体，请检查胜利条件。");
        }
    }

    // --- 公共方法，供其他脚本调用 ---

    /// <summary>
    /// 当一个奶酪被收集时，由盘子或角色调用。
    /// </summary>
    public void OnCheeseCollected()
    {
        if (isLevelOver) return;

        cheesesCollected++;
        Debug.Log($"已收集 {cheesesCollected} / {totalCheesesInLevel} 个奶酪。");

        // 检查是否满足胜利条件
        if (cheesesCollected >= totalCheesesInLevel)
        {
            LevelWon();
        }
    }

    /// <summary>
    /// 当玩家失败时（例如被水滴碰到），由玩家或障碍物调用。
    /// </summary>
    public void LevelLost()
    {
        if (isLevelOver) return;

        isLevelOver = true;
        Debug.Log("关卡失败！");

        // 激活失败UI面板
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
        else
        {
            // 如果没有UI，则直接重启
            Debug.LogWarning("失败UI面板未设置，将在2秒后直接重启。");
            Invoke("RestartLevel", 2f);
        }
    }

    // --- 私有方法，处理具体流程 ---

    private void LevelWon()
    {
        isLevelOver = true;
        Debug.Log("恭喜！关卡胜利！");

        // 激活胜利UI面板
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            // 如果没有UI，则直接进入下一关
            Debug.LogWarning("胜利UI面板未设置，将在2秒后自动进入下一关。");
            Invoke("LoadNextLevel", 2f);
        }
    }

    // --- UI按钮会调用的公共方法 ---

    /// <summary>
    /// 加载下一个关卡。
    /// </summary>
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // 检查是否存在下一个场景
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("已经是最后一关了！返回主菜单或显示通关画面。");
            // 在这里可以加载主菜单场景，例如：SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// 重新开始当前关卡。
    /// </summary>
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// 退出游戏。
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("退出游戏...");
        Application.Quit();
    }
}