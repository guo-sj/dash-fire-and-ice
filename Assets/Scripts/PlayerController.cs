using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 冰人主角控制器
/// 处理触屏跳跃、动画状态切换
/// </summary>
public class PlayerController : MonoBehaviour
{
    // 跳跃初速度
    [SerializeField] private float jumpSpeed = 7f;
    // 主角刚体组件
    private Rigidbody2D rb;
    // 动画控制器
    private Animator anim;
    // 是否在地面（防止二次跳跃）
    private bool isGrounded = true;
    // 输入动作
    private PlayerInputActions inputActions;

    private void Awake()
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // 初始化输入动作
        inputActions = new PlayerInputActions();
        inputActions.Gameplay.Tap.performed += OnTapPerformed;

        // 设置主角初始位置（水平固定在屏幕20%宽度处）
        float initX = Screen.width * 0.2f / Screen.dpi * 0.0254f;
        transform.position = new Vector3(initX, transform.position.y, transform.position.z);

        // 初始化刚体参数
        rb.gravityScale = 2f;
        rb.freezeRotation = true;
        // 正确代码（替换 FreezeRotationZ 为 FreezeRotation）
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        // 更新动画参数
        anim.SetBool("isRunning", isGrounded && !anim.GetBool("isDead"));
        anim.SetBool("isJumping", !isGrounded && !anim.GetBool("isDead"));
    }

    /// <summary>
    /// 触屏点击回调
    /// </summary>
    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded && !anim.GetBool("isDead"))
        {
            Jump();
        }
    }

    /// <summary>
    /// 跳跃逻辑
    /// </summary>
    private void Jump()
    {
        isGrounded = false;
        // 重置垂直速度，添加跳跃力
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 碰撞检测（落地判断）
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // 碰撞障碍触发死亡
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            anim.SetBool("isDead", true);
            GameManager.Instance.GameOver();
        }
    }
}
