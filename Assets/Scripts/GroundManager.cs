using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 地面管理器
/// 实现地面预制体的循环生成与回收，模拟无限地面
/// </summary>
public class GroundManager : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundPoolCount = 3;
    [SerializeField] private float scrollSpeed = 5f;

    private float groundWidth;
    private List<GameObject> activeGrounds;

    private void Awake()
    {
        // 计算地面宽度（使用相机的可视宽度）
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        groundWidth = cameraWidth;

        activeGrounds = new List<GameObject>();

        // 初始化地面位置
        for (int i = 0; i < groundPoolCount; i++)
        {
            GameObject ground = Instantiate(groundPrefab, transform);
            ground.transform.position = new Vector3(i * groundWidth, -3f, 0f);
            // 设置地面宽度
            SpriteRenderer sr = ground.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.drawMode = SpriteDrawMode.Tiled;
                sr.size = new Vector2(groundWidth, 1f);
            }
            // 设置碰撞体大小
            BoxCollider2D col = ground.GetComponent<BoxCollider2D>();
            if (col != null)
            {
                col.size = new Vector2(groundWidth, 1f);
            }
            activeGrounds.Add(ground);
        }

        Debug.Log("GroundManager: 初始化完成，地面宽度=" + groundWidth);
    }

    private void Update()
    {
        // 游戏结束时停止
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        // 移动所有地面
        foreach (GameObject ground in activeGrounds)
        {
            if (ground != null)
            {
                ground.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
            }
        }

        // 检查第一个地面是否移出屏幕左侧
        if (activeGrounds.Count > 0 && activeGrounds[0] != null)
        {
            GameObject firstGround = activeGrounds[0];
            if (firstGround.transform.position.x <= -groundWidth)
            {
                // 移动到最右侧
                float lastGroundX = activeGrounds[activeGrounds.Count - 1].transform.position.x;
                firstGround.transform.position = new Vector3(lastGroundX + groundWidth, -3f, 0f);

                // 将第一个移到列表末尾
                activeGrounds.RemoveAt(0);
                activeGrounds.Add(firstGround);
            }
        }
    }
}
