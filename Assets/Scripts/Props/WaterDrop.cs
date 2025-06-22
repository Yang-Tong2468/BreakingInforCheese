using UnityEngine;
using UnityEngine.SceneManagement; 

/// <summary>
/// ����ˮ�ε���Ϊ�������������� "Player" ��ǩ������ʱ��
/// �ᴥ����Ϸ�������߼�
/// </summary>
public class WaterDrop : MonoBehaviour
{
    [Header("Ч������")]
    [SerializeField] private GameObject splashEffectPrefab; // �������ʱ���ŵġ�ˮ������Ч

    // OnCollisionEnter2D ������������ Collider2D �ĸ�����ײʱ������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if(collision.gameObject.CompareTag("Plate"))
        // {
        //     collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //     return;
        // }
        
        // 1. ����ˮ������
        Destroy(gameObject);

        // ���ײ���������Ƿ������
        if (collision.gameObject.CompareTag("Player"))
        {
            // --- ��Ϸ�����߼� ---
            Debug.Log("ˮ����������ң���Ϸ������");

            // 1. ��ȡ��Ҷ��󣬲����ܽ������Ŀ���������ֹ������Ϸ������������ƶ�
            GameObject playerObject = collision.gameObject;
            WallWalkerController playerController = playerObject.GetComponent<WallWalkerController>();
            if (playerController != null)
            {
                playerController.enabled = false; // ������ҵĿ��ƽű�
            }
            // Ҳ����ֱ�Ӷ�����ҵĸ���
            Rigidbody2D playerRb = playerObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
                playerRb.bodyType = RigidbodyType2D.Static;
            }

            // 2. ����ײ�㲥��ˮ����Ч
            if (splashEffectPrefab != null)
            {
                Instantiate(splashEffectPrefab, collision.contacts[0].point, Quaternion.identity);
            }

            // 4. ����һ��ȫ�ֵ���Ϸ����������������ֱ���ӳٺ������ؿ�
            // Ϊ����ʾ������ֱ���ӳ�1.5������¼��ص�ǰ����
            //Invoke("RestartLevel", 1.5f);
        }
    }

    // һ���򵥵ķ������������¼��ص�ǰ��ĳ���
    private void RestartLevel()
    {
        // ��ȡ��ǰ�����������
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // ���¼��ظó���
        SceneManager.LoadScene(currentSceneIndex);
    }
}