using UnityEngine;

/// <summary>
/// 障碍管理器
/// 控制障碍的随机生成、移动、回收
/// </summary>
public class ObstacleManager : MonoBehaviour
{
    // 障碍对象池（拖拽赋值）
    [SerializeField] private ObstaclePool obstaclePool;
    // 生成间隔（2-4秒）
    [SerializeField] private float minSpawnInterval = 2f;
    [SerializeField] private float maxSpawnInterval = 4f;
    // 障碍生成位置（主角右侧0.5~0.8屏幕宽度）
    private float spawnXMin;
    private float spawnXMax;
    // 障碍Y轴位置（地面上方）
    private float spawnY;
    // 下一次生成时间
    private float nextSpawnTime;

    private void Awake()
    {
        // 初始化生成位置（适配分辨率）
        spawnXMin = Screen.width * 0.5f / Screen.dpi * 0.0254f;
        spawnXMax = Screen.width * 0.8f / Screen.dpi * 0.0254f;
        spawnY = (Screen.height * 0.08f / Screen.dpi * 0.0254f) + (Screen.height * 0.1f / Screen.dpi * 0.0254f); // 地面高度+障碍高度/2

        // 初始化下一次生成时间
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        // 游戏结束时停止生成
        if (GameManager.Instance.IsGameOver) return;

        // 到时间生成障碍
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            // 重置下一次生成时间
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    /// <summary>
    /// 生成障碍
    /// </summary>
    private void SpawnObstacle()
    {
        GameObject obstacle = obstaclePool.GetObstacleFromPool();
        // 随机生成位置
        float randomX = Random.Range(spawnXMin, spawnXMax);
        obstacle.transform.position = new Vector3(randomX, spawnY, transform.position.z);
        // 设置障碍尺寸（适配分辨率）
        float obstacleSize = Screen.height * 0.1f / Screen.dpi * 0.0254f;
        obstacle.transform.localScale = new Vector3(obstacleSize, obstacleSize, 1f);
        // 添加移动脚本（若未添加）
        if (!obstacle.TryGetComponent(out ObstacleMove moveScript))
        {
            moveScript = obstacle.AddComponent<ObstacleMove>();
        }
        moveScript.Init(obstaclePool);
    }
}
