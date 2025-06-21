using UnityEngine;

/// <summary>
/// �������ӵ���Ϊ������������ "Cheese" ��ǩ������ʱ��
/// �Ὣ�Լ������Ҷ��̶�ס��������Ч��Ȼ��һ����ʧ
/// </summary>
public class Plate : MonoBehaviour
{
    [Header("Ч������")]
    [SerializeField] private GameObject collectionEffectPrefab; // �ռ��ɹ�ʱ���ŵ���ЧԤ����
    [SerializeField] private float disappearDelay = 0.5f;     // �̶�����ʧ���ӳ�ʱ��

    private bool hasCollected = false; // һ����־λ����ֹ�ظ�����

    // OnCollisionEnter2D ������������ Collider2D �ĸ�����ײʱ������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"����������: {collision.gameObject.name}");
        // ����Ѿ��ռ����ˣ�����ײ���Ĳ������ң���ֱ�ӷ���
        if (hasCollected ||!(collision.gameObject.CompareTag("Cheese") || collision.gameObject.CompareTag("BubbleCheese")))
        {
            return;
        }

        // --- �ɹ��������ң�ִ���ռ��߼� ---

        // 1. ���ñ�־λ����ֹ����ʧ�ӳ��ڼ��ٴδ�����ײ�߼�
        hasCollected = true;

        Debug.Log("�������������ң�");

        // 2. ��ȡ���Ӻ����ҵ� Rigidbody2D ���
        Rigidbody2D plateRb = GetComponent<Rigidbody2D>();
        Rigidbody2D cheeseRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // 3. �����Ƕ��̶�ס
        if (plateRb != null)
        {
            // �����ӱ�Ϊ Static����������λ��
            plateRb.bodyType = RigidbodyType2D.Static;
        }
        if (cheeseRb != null)
        {
            // ������Ҳ��Ϊ Static
            cheeseRb.bodyType = RigidbodyType2D.Static;
        }

        // 4. ������Ч
        if (collectionEffectPrefab != null)
        {
            // ����ײ�㲥����Ч������������Ȼ
            Vector2 effectPosition = collision.contacts[0].point;
            Instantiate(collectionEffectPrefab, effectPosition, Quaternion.identity);
        }

        // 5. �ӳٺ��������Ӻ�����
        // ������������
        Destroy(gameObject, disappearDelay);
        // �������Ҷ���
        Destroy(collision.gameObject, disappearDelay);
    }
}