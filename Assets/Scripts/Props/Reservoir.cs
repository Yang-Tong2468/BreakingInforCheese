using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 油层
/// </summary>
public class Reservoir : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // --- 检查进入的对象是否是我们的目标 ---

        // 1. 检查是否是 Player
        bool isPlayer = other.CompareTag("Player");

        // 2. 检查是否属于 Sticky 层
        // 你需要提前在 WallWalkerController 中设置好 stickyLayer 变量才能用
        // 或者我们在这里直接用层级名字来判断，更独立
        bool isStickyObject = other.gameObject.layer == LayerMask.NameToLayer("Sticky");

        // 如果既不是 Player 也不是 StickyObject，则忽略
        if (!isPlayer && !isStickyObject)
        {
            return;
        }

        // --- 固定对象的逻辑 ---

        // 尝试从进入的对象上获取 Rigidbody2D 组件
        Rigidbody2D rbToFreeze = other.GetComponent<Rigidbody2D>();

        // 如果对象有 Rigidbody2D 组件
        if (rbToFreeze != null)
        {
            Debug.Log($"油层捕获了: {other.name}");

            // 核心：将刚体的类型变为 Static！
            // Static 类型的刚体完全不受物理引擎影响，也不会被代码施加的力或速度移动。
            // 它的位置被彻底锁定，完美实现了“固定住”的效果。
            rbToFreeze.bodyType = RigidbodyType2D.Static;
        }
    }
}
