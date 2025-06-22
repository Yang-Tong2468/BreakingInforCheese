using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏总管理器 
/// 负责处理游戏状态、场景加载、UI显示等全局事务
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelController CurrentLevelController; // 当前关卡控制器

    [Header("UI 面板")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <-- 这是跨场景存在的关键！
            SceneManager.sceneLoaded += OnSceneLoaded; // 监听场景加载事件
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 记得在 OnDestroy 里移除事件监听，防止内存泄漏
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 场景加载后自动查找 LevelController
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        defeatPanel = GameObject.Find("DefeatPanel");
        defeatPanel.SetActive(false);
        CurrentLevelController = FindObjectOfType<LevelController>();
        if (CurrentLevelController == null)
        {
            Debug.LogWarning("未找到 LevelController！");
        }
    }

    /// <summary>
    /// 当一个关卡胜利时被调用
    /// </summary>
    public void OnLevelWon()
    {
        Debug.Log("GameManager 收到胜利信号！");
        if (victoryPanel != null)
        {
            //victoryPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 当一个关卡失败时被调用
    /// </summary>
    public void OnLevelLost()
    {
        Debug.Log("GameManager 收到失败信号！");
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
    }


    /// <summary>
    /// 进入下一关卡/场景
    /// </summary>
    public void LoadNextLevel()
    {
        // 先隐藏UI，避免在新场景闪烁
        if (victoryPanel != null) victoryPanel.SetActive(false);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("通关所有关卡！");
            // 这里可以加载主菜单或制作人员名单
            // SceneManager.LoadScene("MainMenu");
        }
    }


    /// <summary>
    /// 重新开始这一关卡
    /// </summary>
    public void RestartLevel()
    {
        ResumeGame(); // 确保游戏恢复正常速度
        if (defeatPanel != null) defeatPanel.SetActive(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f; // 设置时间缩放为0，游戏暂停
        Debug.Log("游戏已暂停");
    }

    /// <summary>
    /// 恢复游戏
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f; // 恢复时间缩放，游戏继续
        Debug.Log("游戏已恢复");
    }

    /// <summary>
    /// 离开/结束游戏
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}