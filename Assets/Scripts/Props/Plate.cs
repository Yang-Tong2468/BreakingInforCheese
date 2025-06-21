using UnityEngine;

/// <summary>
/// 控制盘子的行为当它碰到带有 "Cheese" 标签的物体时，
/// 会将自己和奶酪都固定住，播放特效，然后一起消失
/// </summary>
public class Plate : MonoBehaviour
{
    [Header("效果设置")]
    [SerializeField] private GameObject collectionEffectPrefab; // 收集成功时播放的特效预制体
    [SerializeField] private float disappearDelay = 0.5f;     // 固定后到消失的延迟时间

    private bool hasCollected = false; // 一个标志位，防止重复触发

    // OnCollisionEnter2D 会在两个带有 Collider2D 的刚体碰撞时被调用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"盘子碰到了: {collision.gameObject.name}");
        // 如果已经收集过了，或者撞到的不是奶酪，则直接返回
        if (hasCollected ||!(collision.gameObject.CompareTag("Cheese") || collision.gameObject.CompareTag("BubbleCheese")))
        {
            return;
        }

        // --- 成功碰到奶酪，执行收集逻辑 ---

        // 1. 设置标志位，防止在消失延迟期间再次触发碰撞逻辑
        hasCollected = true;

        Debug.Log("盘子碰到了奶酪！");

        // 2. 获取盘子和奶酪的 Rigidbody2D 组件
        Rigidbody2D plateRb = GetComponent<Rigidbody2D>();
        Rigidbody2D cheeseRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // 3. 将它们都固定住
        if (plateRb != null)
        {
            // 将盘子变为 Static，彻底锁定位置
            plateRb.bodyType = RigidbodyType2D.Static;
        }
        if (cheeseRb != null)
        {
            // 将奶酪也变为 Static
            cheeseRb.bodyType = RigidbodyType2D.Static;
        }

        // 4. 播放特效
        if (collectionEffectPrefab != null)
        {
            // 在碰撞点播放特效，看起来更自然
            Vector2 effectPosition = collision.contacts[0].point;
            Instantiate(collectionEffectPrefab, effectPosition, Quaternion.identity);
        }

        // 5. 延迟后销毁盘子和奶酪
        // 销毁盘子自身
        Destroy(gameObject, disappearDelay);
        // 销毁奶酪对象
        Destroy(collision.gameObject, disappearDelay);
    }
}