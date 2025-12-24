using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏管理器
/// 单例模式，管理游戏状态（运行/结束）、游戏结束逻辑
/// </summary>
public class GameManager : MonoBehaviour
{
    // 单例实例
    public static GameManager Instance { get; private set; }
    // 游戏状态
    public bool IsGameOver { get; private set; } = false;
    // 游戏结束文本（拖拽赋值）
    [SerializeField] private Text gameOverText;

    private void Awake()
    {
        // 单例初始化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // 初始化游戏结束文本（隐藏）
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
            // 适配文本尺寸和位置
            gameOverText.fontSize = (int)(Screen.height * 0.05f);
            gameOverText.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    /// <summary>
    /// 游戏结束逻辑
    /// </summary>
    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Debug.Log("游戏结束");

        // 显示游戏结束文本
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        // 停止背景、地面、障碍移动
        StopAllMovingObjects();
    }

    /// <summary>
    /// 停止所有移动对象（背景、地面、障碍）
    /// </summary>
    private void StopAllMovingObjects()
    {
        // 停止背景滚动
        BackgroundScroll[] bgScrolls = FindObjectsOfType<BackgroundScroll>();
        foreach (var scroll in bgScrolls)
        {
            scroll.enabled = false;
        }

        // 停止地面生成/移动
        GroundManager[] groundManagers = FindObjectsOfType<GroundManager>();
        foreach (var manager in groundManagers)
        {
            manager.enabled = false;
        }

        // 停止障碍移动/生成
        ObstacleManager[] obstacleManagers = FindObjectsOfType<ObstacleManager>();
        foreach (var manager in obstacleManagers)
        {
            manager.enabled = false;
        }
        ObstacleMove[] obstacleMoves = FindObjectsOfType<ObstacleMove>();
        foreach (var move in obstacleMoves)
        {
            move.enabled = false;
        }

        // 禁用主角输入
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        foreach (var controller in playerControllers)
        {
            controller.enabled = false;
        }
    }
}
