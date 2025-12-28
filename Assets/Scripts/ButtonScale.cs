using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 按钮缩放反馈脚本
/// 按下时缩小，抬起/离开时恢复原尺寸
/// </summary>
public class ButtonScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // 缩放系数（按下时）
    [SerializeField] private float pressScale = 0.95f;
    // 原始尺寸
    private Vector3 originalScale;

    private void Awake()
    {
        // 初始化原始尺寸
        originalScale = transform.localScale;
        // 适配屏幕分辨率的按钮尺寸
        float btnWidth = Screen.width * 0.2f;
        float btnHeight = Screen.height * 0.1f;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(btnWidth, btnHeight);
    }

    /// <summary>
    /// 按下按钮时触发
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * pressScale;
    }

    /// <summary>
    /// 抬起按钮时触发
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    /// <summary>
    /// 鼠标/手指离开按钮时触发
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
