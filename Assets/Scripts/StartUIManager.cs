using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 启动界面管理器
/// 处理开始游戏按钮点击逻辑，销毁启动UI
/// </summary>
public class StartUIManager : MonoBehaviour
{
    // 开始游戏按钮（拖拽赋值）
    [SerializeField] private Button btnStartGame;
    // 启动UI根节点（拖拽赋值）
    [SerializeField] private GameObject uiStartRoot;

    private void Awake()
    {
        // 绑定按钮点击事件
        btnStartGame.onClick.AddListener(OnStartGameClick);
        // 确保启动UI居中适配
        uiStartRoot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// 点击开始游戏按钮回调
    /// </summary>
    private void OnStartGameClick()
    {
        // 销毁启动UI
        Destroy(uiStartRoot);
        // 可在此处触发游戏场景初始化（如启用背景滚动、地面生成等）
        Debug.Log("开始游戏，启动UI已销毁");
    }
}
