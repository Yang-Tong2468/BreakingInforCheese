using UnityEngine;

public class Decelerator : MonoBehaviour
{
    [SerializeField] private float decelerationRate = 0.01f; // 减速率，值越大停得越快
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 使用 Lerp 平滑地将速度减到零
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, decelerationRate * Time.fixedDeltaTime);

        // 如果速度足够小，就完全停止并移除这个脚本组件，以节省性能
        if (rb.velocity.magnitude < 0.01f)
        {
            rb.velocity = Vector2.zero;
            //Destroy(this); // 自我销毁
        }
    }
}