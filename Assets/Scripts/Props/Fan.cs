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

    private bool isMoving = false; // ��־λ����ֹ�ظ�����

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

        // --- ׼������ ---
        // 1. ��ȡ���ҵ� Rigidbody2D ���
        Rigidbody2D cheeseRb = cheeseToMove.GetComponent<Rigidbody2D>();

        // 2. �����ҵ�������������Ϊ Kinematic
        cheeseRb.bodyType = RigidbodyType2D.Kinematic;
        cheeseRb.velocity = Vector2.zero; // ����ٶ�

        // 3. ��¼��ʼλ�úͼ�ʱ��
        Vector2 startPosition = cheeseToMove.transform.position;
        float elapsedTime = 0f;

        // --- �ƶ�ѭ�� ---
        // ����ʱ��С����ʱ��ʱ������ִ��
        while (elapsedTime < travelDuration)
        {
            // ���㵱ǰӦ���ڵĽ��� (0��1֮��)
            float progress = elapsedTime / travelDuration;

            // ʹ�� Vector2.Lerp (���Բ�ֵ) �����㵱ǰ֡Ӧ���ڵ�λ��
            Vector2 newPosition = Vector2.Lerp(startPosition, targetDestination.position, progress);

            // ֱ���ƶ����ҵ�λ��
            cheeseRb.MovePosition(newPosition);

            // ���¼�ʱ��
            elapsedTime += Time.deltaTime;

            // �ȴ���һ֡
            yield return null;
        }

        // --- �ƶ����� ---
        // ȷ���������վ�ȷ��ͣ��Ŀ��λ��
        cheeseRb.MovePosition(targetDestination.position);

        // (��ѡ) �����ҵ��������Իָ�Ϊ Dynamic�����������ٴ����ɻ
         cheeseRb.bodyType = RigidbodyType2D.Static;

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