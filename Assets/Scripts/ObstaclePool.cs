using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 障碍对象池
/// 管理障碍预制体的复用，避免频繁创建/销毁
/// </summary>
public class ObstaclePool : MonoBehaviour
{
    // 障碍预制体（拖拽赋值）
    [SerializeField] private GameObject obstaclePrefab;
    // 对象池大小
    [SerializeField] private int poolSize = 5;
    // 障碍对象池队列
    private Queue<GameObject> obstaclePool;

    private void Awake()
    {
        // 初始化对象池
        obstaclePool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.SetActive(false);
            obstaclePool.Enqueue(obstacle);
        }
    }

    /// <summary>
    /// 从对象池获取障碍
    /// </summary>
    /// <returns>可用的障碍对象</returns>
    public GameObject GetObstacleFromPool()
    {
        if (obstaclePool.Count == 0)
        {
            // 池为空时额外创建一个
            GameObject newObstacle = Instantiate(obstaclePrefab, transform);
            newObstacle.SetActive(false);
            obstaclePool.Enqueue(newObstacle);
        }

        GameObject obstacle = obstaclePool.Dequeue();
        obstacle.SetActive(true);
        return obstacle;
    }

    /// <summary>
    /// 回收障碍到对象池
    /// </summary>
    /// <param name="obstacle">要回收的障碍</param>
    public void ReturnObstacleToPool(GameObject obstacle)
    {
        obstacle.SetActive(false);
        obstaclePool.Enqueue(obstacle);
    }
}
