using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �ؿ������� 
/// ������ǰ�ؿ���ʤ����ʧ��������������UI��ʾ�ͳ����л�
/// </summary>
public class LevelManager : MonoBehaviour
{
    // --- ����ģʽʵ�� ---
    public static LevelManager Instance { get; private set; }

    [Header("�ؿ�UI���")]
    [SerializeField] private GameObject victoryPanel;   // ʤ��ʱ��ʾ��UI���
    [SerializeField] private GameObject defeatPanel;    // ʧ��ʱ��ʾ��UI���

    [Header("�ؿ�ʤ������")]
    // ����������һ���򵥵����ӣ��ռ�����������
    private int totalCheesesInLevel;
    private int cheesesCollected = 0;

    private bool isLevelOver = false; // ��־λ����ֹ�ظ�����ʤ����ʧ��

    private void Awake()
    {
        // --- ����ģʽ�ı�׼ʵ�� ---
        if (Instance == null)
        {
            Instance = this;
            // (��ѡ) DontDestroyOnLoad(gameObject); // �����ϣ���������糡������
        }
        else
        {
            Destroy(gameObject); // ����Ѵ���ʵ�����������Լ�
        }
    }

    private void Start()
    {
        // ��Ϸ��ʼʱ������UI���
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);

        // ��ʼ��ʤ������
        // �ҵ����������д��� "Cheese" ��ǩ������
        totalCheesesInLevel = GameObject.FindGameObjectsWithTag("Cheese").Length;
        Debug.Log($"���ع��� {totalCheesesInLevel} ��������Ҫ�ռ���");

        // ����ؿ���ʼʱ��û�����ң�ֱ����ʤ�����򱨴�ȡ���������ƣ�
        if (totalCheesesInLevel == 0)
        {
            Debug.LogWarning("�ؿ���û���ҵ� 'Cheese' ��ǩ�����壬����ʤ��������");
        }
    }

    // --- �����������������ű����� ---

    /// <summary>
    /// ��һ�����ұ��ռ�ʱ�������ӻ��ɫ���á�
    /// </summary>
    public void OnCheeseCollected()
    {
        if (isLevelOver) return;

        cheesesCollected++;
        Debug.Log($"���ռ� {cheesesCollected} / {totalCheesesInLevel} �����ҡ�");

        // ����Ƿ�����ʤ������
        if (cheesesCollected >= totalCheesesInLevel)
        {
            LevelWon();
        }
    }

    /// <summary>
    /// �����ʧ��ʱ�����类ˮ��������������һ��ϰ�����á�
    /// </summary>
    public void LevelLost()
    {
        if (isLevelOver) return;

        isLevelOver = true;
        Debug.Log("�ؿ�ʧ�ܣ�");

        // ����ʧ��UI���
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
        else
        {
            // ���û��UI����ֱ������
            Debug.LogWarning("ʧ��UI���δ���ã�����2���ֱ��������");
            Invoke("RestartLevel", 2f);
        }
    }

    // --- ˽�з���������������� ---

    private void LevelWon()
    {
        isLevelOver = true;
        Debug.Log("��ϲ���ؿ�ʤ����");

        // ����ʤ��UI���
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            // ���û��UI����ֱ�ӽ�����һ��
            Debug.LogWarning("ʤ��UI���δ���ã�����2����Զ�������һ�ء�");
            Invoke("LoadNextLevel", 2f);
        }
    }

    // --- UI��ť����õĹ������� ---

    /// <summary>
    /// ������һ���ؿ���
    /// </summary>
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // ����Ƿ������һ������
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�Ѿ������һ���ˣ��������˵�����ʾͨ�ػ��档");
            // ��������Լ������˵����������磺SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// ���¿�ʼ��ǰ�ؿ���
    /// </summary>
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// �˳���Ϸ��
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("�˳���Ϸ...");
        Application.Quit();
    }
}