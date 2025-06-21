using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// һ��ˮ�������������Թ̶���ʱ����������λ������ˮ��Ԥ����
/// </summary>
public class WaterDropSpawner : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private GameObject waterDropPrefab; // Ҫ���ɵ�ˮ��Ԥ����
    [SerializeField] private float spawnInterval = 2.0f; // ����ˮ�ε�ʱ�������룩
    [SerializeField] private float initialDelay = 1.0f;  // ��Ϸ��ʼ�󣬵�һ������ˮ�ε��ӳ�
    [SerializeField] private Transform spawnPosition; // ����ˮ�ε�λ��

    // ����Ϸ��ʼʱ����������ѭ��
    private void Start()
    {
        // ����Ƿ�������Ԥ���壬���û���򱨴�ֹͣ
        if (waterDropPrefab == null)
        {
            Debug.LogError("ˮ��Ԥ���� (WaterDropPrefab) δ���ã��޷�����ˮ�Ρ�");
            return;
        }

        // ��������ˮ�ε�Э��
        StartCoroutine(SpawnWaterDropsRoutine());
    }

    // ����ѭ������ˮ�ε�Э��
    private IEnumerator SpawnWaterDropsRoutine()
    {
        // 1. ��ʼ�ӳ�
        // �ȴ�ָ���ĳ�ʼ�ӳ�ʱ��
        yield return new WaitForSeconds(initialDelay);

        // 2. ��������ѭ��
        while (true) // while(true) �ᴴ��һ������ѭ����Э�̵������������Ῠ����Ϸ
        {
            // ����������λ�ú�Ĭ����ת�£�����һ���µ�ˮ��
            Instantiate(waterDropPrefab, spawnPosition.position, Quaternion.identity);

            // �ȴ�ָ����ʱ����
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}