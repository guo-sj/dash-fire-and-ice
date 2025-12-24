using UnityEngine;

/// <summary>
/// 障碍移动脚本
/// 控制障碍向左移动，移出屏幕后回收
/// </summary>
public class ObstacleMove : MonoBehaviour
{
    // 移动速度（与背景同步5m/s）
    [SerializeField] private float moveSpeed = 5f;
    // 障碍对象池
    private ObstaclePool obstaclePool;
    // 回收阈值（屏幕左侧外）
    private float recycleX;

    private void Awake()
    {
        // 初始化回收阈值
        recycleX = -Screen.width * 0.1f / Screen.dpi * 0.0254f;
    }

    /// <summary>
    /// 初始化障碍移动参数
    /// </summary>
    /// <param name="pool">障碍对象池</param>
    public void Init(ObstaclePool pool)
    {
        obstaclePool = pool;
    }

    private void Update()
    {
        // 游戏结束时停止移动
        if (GameManager.Instance.IsGameOver) return;

        // 向左移动
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 移出屏幕左侧后回收
        if (transform.position.x <= recycleX)
        {
            obstaclePool.ReturnObstacleToPool(gameObject);
        }
    }
}
