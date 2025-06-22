using System.Collections;
using UnityEngine;

/// <summary>
/// һ�����Ա�����ķ��ȣ������ȡһ���ض����ҵ����ã�
/// ��ͨ��ƽ�����ƶ���λ�ã������͵�һ��ָ����Ŀ���
/// </summary>
public class Fan : MonoBehaviour
{
    [Header("Ŀ��������")]
    [SerializeField] private Rigidbody2D cheeseToMove;      // �ڱ༭����ֱ��ָ��Ҫ�ƶ�������
    [SerializeField] private Transform targetDestination;   // ����Ҫ���͵���Ŀ��λ��
    [SerializeField] private float travelDuration = 2.0f;   // �����ƶ�������Ҫ��ʱ�䣨�룩
    [SerializeField] private GameObject wind; // ���ȵķ�Ч��
    private Animator windAnimator; //��Ķ���

    private bool isMoving = false; // ��־λ����ֹ�ظ�����

    private void Awake()
    {
        // ȷ�����ȵķ�Ч���ڿ�ʼʱ�ǹرյ�
        if (wind != null)
        {
            windAnimator = wind.GetComponent<Animator>();
        }
    }


    [ContextMenu("Start Moving Cheese")]
    // ����һ���������������Ա������ű�������һ����ť����������������
    public void StartMovingCheese()
    {
        // ���û��ָ�����һ�Ŀ�꣬���������ƶ��У���ִ���κβ���
        if (cheeseToMove == null || targetDestination == null || isMoving)
        {
            return;
        }

        StartCoroutine(MoveCheeseToDestination());
    }

    // Э�̣����ڴ�����ʱ��仯���ƶ�����
    private IEnumerator MoveCheeseToDestination()
    {
        isMoving = true;
        Debug.Log($"��ʼ�ƶ�����: {cheeseToMove.name}");

        // ������ GameObject������Ĭ�϶���
        if (wind != null)
        {
            wind.SetActive(true);
        }

        // --- ׼������ ---
        Rigidbody2D cheeseRb = cheeseToMove.GetComponent<Rigidbody2D>();
        cheeseRb.bodyType = RigidbodyType2D.Kinematic;
        cheeseRb.velocity = Vector2.zero;

        Vector2 startPosition = cheeseToMove.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < travelDuration)
        {
            float progress = elapsedTime / travelDuration;
            Vector2 newPosition = Vector2.Lerp(startPosition, targetDestination.position, progress);
            cheeseRb.MovePosition(newPosition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cheeseRb.MovePosition(targetDestination.position);
        cheeseRb.bodyType = RigidbodyType2D.Static;

        // �ƶ�������رշ�
        //if (wind != null)
        //{
        //    wind.SetActive(false);
        //}

        Debug.Log("�����ѵ���Ŀ��λ�ã�");
        isMoving = false;
    }


    // (��ѡ) �ڱ༭���л��Ƹ�����
    private void OnDrawGizmosSelected()
    {
        if (targetDestination != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetDestination.position);
            Gizmos.DrawWireSphere(targetDestination.position, 0.5f);
        }
    }
}