using UnityEngine;

/// <summary>
/// ���Ƶ����ؿ����߼���
/// �����жϵ�ǰ�ؿ���ʤ��/ʧ����������֪ͨ GameManager
/// </summary>
public class LevelController : MonoBehaviour
{
    [Header("�ؿ�ʤ������")]
    // ʤ�����������Ƕ��ֶ����ģ��������ռ�����Ϊ��
    private int totalCheesesInLevel;
    private int cheesesCollected = 0;

    [Header("�ؿ��������")]
    [SerializeField] private LevelExitPortal exitPortal; // �ڱ༭�������ñ��ؿ��Ĵ�����

    public bool isWin { get; private set; }  //�Ƿ���Թ��ء���ͨ�����ǵ���һ��
    [SerializeField]private bool isLevelOver = false;

    private void Awake()
    {
        // ����Ƿ��ڱ༭���������˴�����
        if (exitPortal == null)
        {
            // ���û�����ã����Գ����ڳ������Զ�����
            exitPortal = FindObjectOfType<LevelExitPortal>();
            if (exitPortal == null)
            {
                Debug.LogError($"�ؿ� '{gameObject.scene.name}' ��û���ҵ������ô����� (LevelExitPortal)��");
            }
        }
    }

    private void Start()
    {
        // �ڹؿ���ʼʱ�����㱾����Ҫ�ռ�����������
        totalCheesesInLevel = 1;

        if (totalCheesesInLevel == 0)
        {
            Debug.LogWarning($"�ؿ� '{gameObject.scene.name}' ��û���ҵ� 'Cheese'���޷�ͨ���˷�ʽ��ʤ��");
        }
        else
        {
            Debug.Log($"�ؿ� '{gameObject.scene.name}' ��ʼ��Ŀ�꣺�ռ� {totalCheesesInLevel} �����ҡ�");
        }
    }

    /// <summary>
    /// ��һ�����ұ��ռ�ʱ����
    /// </summary>
    public void OnCheeseCollected()
    {
        if (isLevelOver) return;

        cheesesCollected++;
        CheckForWinCondition();
    }

    /// <summary>
    /// �����ʧ��ʱ����
    /// </summary>
    public void TriggerDefeat()
    {
        if (isLevelOver) return;
        isLevelOver = true;

        Debug.Log($"�ؿ� '{gameObject.scene.name}' ʧ�ܣ�");

        // ֪ͨ GameManager ��Ϸʧ����
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelLost();
        }
    }

    /// <summary>
    /// ��鵱ǰ�ؿ���ʤ������
    /// </summary>
    private void CheckForWinCondition()
    {
        // ����ռ������ﵽ��������ʤ��
        if (cheesesCollected >= totalCheesesInLevel)
        {
            isLevelOver = true;
            isWin = true;
            Debug.Log($"�ؿ� '{gameObject.scene.name}' ʤ����");

            // ֪ͨ GameManager ��Ϸʤ����
            //if (GameManager.Instance != null)
            //{
            //    GameManager.Instance.OnLevelWon();
            //}

            // ����ֱ��֪ͨ GameManager�����Ǽ������
            if (exitPortal != null)
            {
                Debug.Log("�����������ռ���������š�");
                exitPortal.ActivatePortal();
            }
        }
    }
}