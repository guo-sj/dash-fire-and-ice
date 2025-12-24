using UnityEngine;

/// <summary>
/// 背景滚动脚本
/// 实现双背景循环向左滚动，模拟无限跑酷视觉效果
/// </summary>
public class BackgroundScroll : MonoBehaviour
{
    // 滚动速度（5m/s）
    [SerializeField] private float scrollSpeed = 5f;
    // 两个背景Sprite（拖拽赋值）
    [SerializeField] private Transform bg1;
    [SerializeField] private Transform bg2;
    // 背景宽度（屏幕宽度）
    private float bgWidth;

    private void Awake()
    {
        // 初始化背景宽度（与屏幕等宽）
        bgWidth = Screen.width / Screen.dpi * 0.0254f; // 像素转米（适配不同DPI）
        // 设置初始位置
        bg1.position = new Vector3(0, bg1.position.y, bg1.position.z);
        bg2.position = new Vector3(bgWidth, bg2.position.y, bg2.position.z);
    }

    private void Update()
    {
        // 背景向左滚动
        bg1.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        bg2.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 循环拼接：当背景完全移出左侧屏幕时，重置到右侧
        if (bg1.position.x <= -bgWidth)
        {
            bg1.position = new Vector3(bg2.position.x + bgWidth, bg1.position.y, bg1.position.z);
        }
        if (bg2.position.x <= -bgWidth)
        {
            bg2.position = new Vector3(bg1.position.x + bgWidth, bg2.position.y, bg2.position.z);
        }
    }
}
