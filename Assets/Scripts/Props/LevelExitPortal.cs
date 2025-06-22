// LevelExitPortal.cs

using UnityEngine;

/// <summary>
/// �ؿ�����/�����š�����ҽ���ʱ������ LevelController ����ͨ�ء�
/// </summary>
public class LevelExitPortal : MonoBehaviour
{
    [Header("�Ӿ�Ч��")]
    [SerializeField] private Color activeColor = Color.green; // ��������ʱ�������ſ��ܱ�ɫ
    [SerializeField] private GameObject activationEffectPrefab; // ����ʱ���ŵ���Ч
    [SerializeField] private Sprite activeSprite; // ����ʱ�ľ���
    private SpriteRenderer spriteRenderer;
    private bool isActivated = false; // �������Ƿ��Ѽ���������㣩

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // ��������������һ����δ����ĳ�ʼ��ɫ
        // if (spriteRenderer != null) spriteRenderer.color = Color.gray;
    }

    // ������š���������� LevelController ����������ʱ���������������
    public void ActivatePortal()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log("�������ѱ����");

        // �ı���ɫ���ṩ�Ӿ�����
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = activeSprite;
        }

        // ���ż�����Ч
        if (activationEffectPrefab != null)
        {
            Instantiate(activationEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    // ʹ�ô������������ҽ���
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��������Ƿ�����ң����Ҵ������Ƿ��Ѽ���
        if (isActivated && other.CompareTag("Player"))
        {
            Debug.Log("��ҽ����Ѽ���Ĵ����ţ�׼��ͨ��...");

            // ������ҵ��ƶ�����ֹ�ڳ����л�ʱ�������⡪�������³�������������ű������ص���
            WallWalkerController playerController = other.GetComponent<WallWalkerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            // ֪ͨ GameManager ������һ��
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadNextLevel();
            }
        }
    }
}