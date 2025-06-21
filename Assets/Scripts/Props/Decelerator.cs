using UnityEngine;

public class Decelerator : MonoBehaviour
{
    [SerializeField] private float decelerationRate = 0.01f; // �����ʣ�ֵԽ��ͣ��Խ��
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // ʹ�� Lerp ƽ���ؽ��ٶȼ�����
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, decelerationRate * Time.fixedDeltaTime);

        // ����ٶ��㹻С������ȫֹͣ���Ƴ�����ű�������Խ�ʡ����
        if (rb.velocity.magnitude < 0.01f)
        {
            rb.velocity = Vector2.zero;
            //Destroy(this); // ��������
        }
    }
}