using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 地面管理器
/// 实现地面预制体的循环生成与回收，模拟无限地面
/// </summary>
public class GroundManager : MonoBehaviour
{
    // 地面预制体（拖拽赋值）
    [SerializeField] private GameObject groundPrefab;
    // 地面段数量（循环池）
    [SerializeField] private int groundPoolCount = 3;
    // 地面高度（屏幕高度8%）
    private float groundHeight;
    // 地面宽度（与屏幕等宽）
    private float groundWidth;
    // 地面对象池
    private Queue<GameObject> groundPool;

    private void Awake()
    {
        // 初始化地面尺寸（适配分辨率）
        groundHeight = Screen.height * 0.08f / Screen.dpi * 0.0254f; // 像素转米
        groundWidth = Screen.width / Screen.dpi * 0.0254f;

        // 初始化对象池
        groundPool = new Queue<GameObject>();
        for (int i = 0; i < groundPoolCount; i++)
        {
            GameObject ground = Instantiate(groundPrefab, transform);
            ground.SetActive(false);
            groundPool.Enqueue(ground);
        }

        // 初始化地面位置（生成第一个地面段）
        SpawnGround(Vector3.zero);
    }

    private void Update()
    {
        // 检测地面是否移出左侧屏幕，回收并重新生成
        foreach (GameObject ground in groundPool)
        {
            if (ground.activeSelf && ground.transform.position.x <= -groundWidth)
            {
                // 回收地面
                ground.SetActive(false);
                groundPool.Enqueue(ground);
                // 生成新地面（在最右侧地面的右侧）
                Vector3 spawnPos = new Vector3(transform.position.x + groundWidth * (groundPoolCount - 1), 0, 0);
                SpawnGround(spawnPos);
            }
        }
    }

    /// <summary>
    /// 从对象池生成地面
    /// </summary>
    /// <param name="spawnPos">生成位置</param>
    private void SpawnGround(Vector3 spawnPos)
    {
        if (groundPool.Count == 0) return;

        GameObject ground = groundPool.Dequeue();
        ground.SetActive(true);
        ground.transform.position = spawnPos;
        // 设置地面尺寸（适配分辨率）
        ground.GetComponent<RectTransform>().sizeDelta = new Vector2(groundWidth, groundHeight);
        // 重置碰撞体尺寸
        BoxCollider2D collider = ground.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(groundWidth, groundHeight);
    }
}
