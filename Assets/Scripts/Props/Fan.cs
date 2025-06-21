using System.Collections;
using UnityEngine;

/// <summary>
/// 一个可以被激活的风扇，它会获取一个特定奶酪的引用，
/// 并通过平滑地移动其位置，将其送到一个指定的目标点
/// </summary>
public class Fan : MonoBehaviour
{
    [Header("目标与设置")]
    [SerializeField] private Rigidbody2D cheeseToMove;      // 在编辑器中直接指定要移动的奶酪
    [SerializeField] private Transform targetDestination;   // 奶酪要被送到的目标位置
    [SerializeField] private float travelDuration = 2.0f;   // 整个移动过程需要的时间（秒）

    private bool isMoving = false; // 标志位，防止重复激活

    [ContextMenu("Start Moving Cheese")]
    // 这是一个公共方法，可以被其他脚本（比如一个按钮）调用来启动风扇
    public void StartMovingCheese()
    {
        // 如果没有指定奶酪或目标，或者正在移动中，则不执行任何操作
        if (cheeseToMove == null || targetDestination == null || isMoving)
        {
            return;
        }

        StartCoroutine(MoveCheeseToDestination());
    }

    // 协程，用于处理随时间变化的移动过程
    private IEnumerator MoveCheeseToDestination()
    {
        isMoving = true;
        Debug.Log($"开始移动奶酪: {cheeseToMove.name}");

        // --- 准备工作 ---
        // 1. 获取奶酪的 Rigidbody2D 组件
        Rigidbody2D cheeseRb = cheeseToMove.GetComponent<Rigidbody2D>();

        // 2. 将奶酪的物理属性设置为 Kinematic
        cheeseRb.bodyType = RigidbodyType2D.Kinematic;
        cheeseRb.velocity = Vector2.zero; // 清空速度

        // 3. 记录起始位置和计时器
        Vector2 startPosition = cheeseToMove.transform.position;
        float elapsedTime = 0f;

        // --- 移动循环 ---
        // 当计时器小于总时长时，持续执行
        while (elapsedTime < travelDuration)
        {
            // 计算当前应该在的进度 (0到1之间)
            float progress = elapsedTime / travelDuration;

            // 使用 Vector2.Lerp (线性插值) 来计算当前帧应该在的位置
            Vector2 newPosition = Vector2.Lerp(startPosition, targetDestination.position, progress);

            // 直接移动奶酪的位置
            cheeseRb.MovePosition(newPosition);

            // 更新计时器
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // --- 移动结束 ---
        // 确保奶酪最终精确地停在目标位置
        cheeseRb.MovePosition(targetDestination.position);

        // (可选) 将奶酪的物理属性恢复为 Dynamic，让它可以再次自由活动
         cheeseRb.bodyType = RigidbodyType2D.Static;

        Debug.Log("奶酪已到达目标位置！");
        isMoving = false;
    }

    // (可选) 在编辑器中绘制辅助线
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