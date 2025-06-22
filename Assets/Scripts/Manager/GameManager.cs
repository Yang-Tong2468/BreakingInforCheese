using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��Ϸ�ܹ����� 
/// ��������Ϸ״̬���������ء�UI��ʾ��ȫ������
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelController CurrentLevelController; // ��ǰ�ؿ�������

    [Header("UI ���")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <-- ���ǿ糡�����ڵĹؼ���
            SceneManager.sceneLoaded += OnSceneLoaded; // �������������¼�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �ǵ��� OnDestroy ���Ƴ��¼���������ֹ�ڴ�й©
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // �������غ��Զ����� LevelController
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        defeatPanel = GameObject.Find("DefeatPanel");
        defeatPanel.SetActive(false);
        CurrentLevelController = FindObjectOfType<LevelController>();
        if (CurrentLevelController == null)
        {
            Debug.LogWarning("δ�ҵ� LevelController��");
        }
    }

    /// <summary>
    /// ��һ���ؿ�ʤ��ʱ������
    /// </summary>
    public void OnLevelWon()
    {
        Debug.Log("GameManager �յ�ʤ���źţ�");
        if (victoryPanel != null)
        {
            //victoryPanel.SetActive(true);
        }
    }

    /// <summary>
    /// ��һ���ؿ�ʧ��ʱ������
    /// </summary>
    public void OnLevelLost()
    {
        Debug.Log("GameManager �յ�ʧ���źţ�");
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
    }


    /// <summary>
    /// ������һ�ؿ�/����
    /// </summary>
    public void LoadNextLevel()
    {
        // ������UI���������³�����˸
        if (victoryPanel != null) victoryPanel.SetActive(false);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("ͨ�����йؿ���");
            // ������Լ������˵���������Ա����
            // SceneManager.LoadScene("MainMenu");
        }
    }


    /// <summary>
    /// ���¿�ʼ��һ�ؿ�
    /// </summary>
    public void RestartLevel()
    {
        ResumeGame(); // ȷ����Ϸ�ָ������ٶ�
        if (defeatPanel != null) defeatPanel.SetActive(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// ��ͣ��Ϸ
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f; // ����ʱ������Ϊ0����Ϸ��ͣ
        Debug.Log("��Ϸ����ͣ");
    }

    /// <summary>
    /// �ָ���Ϸ
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f; // �ָ�ʱ�����ţ���Ϸ����
        Debug.Log("��Ϸ�ѻָ�");
    }

    /// <summary>
    /// �뿪/������Ϸ
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("�˳���Ϸ");
        Application.Quit();
    }
}