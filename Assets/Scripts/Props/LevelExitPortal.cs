// LevelExitPortal.cs

using UnityEngine;

/// <summary>
/// 关卡出口/传送门。当玩家进入时，会向 LevelController 请求通关。
/// </summary>
public class LevelExitPortal : MonoBehaviour
{
    [Header("视觉效果")]
    [SerializeField] private Color activeColor = Color.green; // 满足条件时，传送门可能变色
    [SerializeField] private GameObject activationEffectPrefab; // 激活时播放的特效
    [SerializeField] private Sprite activeSprite; // 激活时的精灵
    private SpriteRenderer spriteRenderer;
    private bool isActivated = false; // 传送门是否已激活（条件满足）

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 可以在这里设置一个“未激活”的初始颜色
        // if (spriteRenderer != null) spriteRenderer.color = Color.gray;
    }

    // 激活传送门——你可以让 LevelController 在条件满足时调用这个公共方法
    public void ActivatePortal()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log("传送门已被激活！");

        // 改变颜色以提供视觉反馈
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = activeSprite;
        }

        // 播放激活特效
        if (activationEffectPrefab != null)
        {
            Instantiate(activationEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    // 使用触发器来检测玩家进入
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入的是否是玩家，并且传送门是否已激活
        if (isActivated && other.CompareTag("Player"))
        {
            Debug.Log("玩家进入已激活的传送门，准备通关...");

            // 禁用玩家的移动，防止在场景切换时发生意外——加载新场景会重新载入脚本，不必担心
            WallWalkerController playerController = other.GetComponent<WallWalkerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            // 通知 GameManager 加载下一关
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadNextLevel();
            }
        }
    }
}