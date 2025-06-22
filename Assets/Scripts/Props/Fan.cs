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
    [SerializeField] private GameObject wind; // 风扇的风效果
    private Animator windAnimator; //风的动画

    private bool isMoving = false; // 标志位，防止重复激活

    private void Awake()
    {
        // 确保风扇的风效果在开始时是关闭的
        if (wind != null)
        {
            windAnimator = wind.GetComponent<Animator>();
        }
    }


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

        // 激活风的 GameObject，播放默认动画
        if (wind != null)
        {
            wind.SetActive(true);
        }

        // --- 准备工作 ---
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

        // 移动结束后关闭风
        //if (wind != null)
        //{
        //    wind.SetActive(false);
        //}

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