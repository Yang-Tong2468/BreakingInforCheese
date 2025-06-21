using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TutorialSceneManager : MonoBehaviour
{
    public Button restoreButton;
    public Button skipButton;
    public Button backButton;
    public List<string> sceneNames; // 教学场景名列表，按顺序填写

    private int currentSceneIndex = 0;

    // 以玩家为例，记录初始状态
    public GameObject player;
    private Vector3 playerInitialPosition;
    private Quaternion playerInitialRotation;

    void Start()
    {
        // 自动识别当前场景索引
        currentSceneIndex = sceneNames.IndexOf(SceneManager.GetActiveScene().name);

        // 按钮绑定
        if (restoreButton != null)
            restoreButton.onClick.AddListener(RestorePlayerState);
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipTutorial);
        if (backButton != null)
            backButton.onClick.AddListener(BackToPrevious);

        // 控制按钮显示
        if (currentSceneIndex == 0 && backButton != null)
            backButton.gameObject.SetActive(false); // 第一个场景无back
        // if (currentSceneIndex == sceneNames.Count - 1 && skipButton != null)
        //     skipButton.gameObject.SetActive(false); // 最后一个场景无skip

        // 保存初始状态
        if (player != null)
        {
            playerInitialPosition = player.transform.position;
            playerInitialRotation = player.transform.rotation;
        }
    }

    // 恢复玩家操作至初始设置
    public void RestorePlayerState()
    {
        if (player != null)
        {
            player.transform.position = playerInitialPosition;
            player.transform.rotation = playerInitialRotation;
        }
        // 如有其他对象，也可在此处恢复
        Debug.Log("玩家操作已恢复至初始设置");
    }

    // 跳过当前教学场景
    public void SkipTutorial()
    {
        if (currentSceneIndex < sceneNames.Count - 1)
        {
            SceneManager.LoadScene(sceneNames[currentSceneIndex + 1]);
        }
    }

    // 返回上一个教学场景
    public void BackToPrevious()
    {
        if (currentSceneIndex > 0)
        {
            SceneManager.LoadScene(sceneNames[currentSceneIndex - 1]);
        }
    }
}