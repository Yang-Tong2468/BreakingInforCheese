using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 一个水滴生成器，会以固定的时间间隔在自身位置生成水滴预制体
/// </summary>
public class WaterDropSpawner : MonoBehaviour
{
    [Header("生成设置")]
    [SerializeField] private GameObject waterDropPrefab; // 要生成的水滴预制体
    [SerializeField] private float spawnInterval = 2.0f; // 生成水滴的时间间隔（秒）
    [SerializeField] private float initialDelay = 1.0f;  // 游戏开始后，第一次生成水滴的延迟
    [SerializeField] private Transform spawnPosition; // 生成水滴的位置

    // 在游戏开始时，启动生成循环
    private void Start()
    {
        // 检查是否设置了预制体，如果没有则报错并停止
        if (waterDropPrefab == null)
        {
            Debug.LogError("水滴预制体 (WaterDropPrefab) 未设置！无法生成水滴。");
            return;
        }

        // 启动生成水滴的协程
        StartCoroutine(SpawnWaterDropsRoutine());
    }

    // 用于循环生成水滴的协程
    private IEnumerator SpawnWaterDropsRoutine()
    {
        // 1. 初始延迟
        // 等待指定的初始延迟时间
        yield return new WaitForSeconds(initialDelay);

        // 2. 无限生成循环
        while (true) // while(true) 会创建一个无限循环，协程的特性让它不会卡死游戏
        {
            // 在生成器的位置和默认旋转下，生成一个新的水滴
            Instantiate(waterDropPrefab, spawnPosition.position, Quaternion.identity);

            // 等待指定的时间间隔
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}