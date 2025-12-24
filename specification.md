# 「冰火冲冲冲」Demo 开发任务清单
**文档说明**：本清单为Unity 2022.3 LTS开发鸿蒙2D跑酷Demo的核心任务分解，所有任务基于Unity内置资源实现，无外部依赖，完成后可直接在Unity中运行并导出鸿蒙HAP包。
**核心原则**：任务按开发流程解耦，前序任务验收通过后再执行后续任务，所有代码需添加中文注释，确保可读性。

| Task ID | 任务名称               | 任务目标                                                                 | 具体执行步骤                                                                                                                                                                                                 | 输入资源                                  | 输出成果                                                                 | 验收标准                                                                                                                                                                                                 | 依赖 Task |
|---------|------------------------|--------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------|--------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------|
| T1      | 项目基础配置           | 完成Unity项目的鸿蒙适配、层创建、单场景初始化，为后续开发搭建基础环境       | 1. 新建Unity 2022.3 LTS 2D项目，命名为「IceFireDashDemo」；<br>2. 切换平台至OpenHarmony（File→Build Settings→OpenHarmony→Switch Platform）；<br>3. 导入Unity Input System包（Window→Package Manager→Input System）；<br>4. 创建4个层：Player、Obstacle、Ground、UI；<br>5. 新建单场景，命名为「GameScene」，保存到Scenes目录。 | Unity 2022.3 LTS、OpenHarmony SDK          | 配置完成的Unity项目工程、GameScene场景文件 | 1. 项目平台成功切换为OpenHarmony；<br>2. Input System包导入无报错；<br>3. 4个层创建完成；<br>4. GameScene场景可正常打开，控制台无报错。| 无        |
| T2      | 启动界面开发           | 实现带「开始游戏」按钮的启动UI，支持触屏点击与缩放反馈，单场景内实现界面切换 | 1. 在GameScene中创建Canvas（Render Mode设为Screen Space - Overlay），命名为「UI_Start」；<br>2. 给Canvas添加全屏Image（深绿色，模拟山水画背景，Alpha=1）；<br>3. 在Canvas下创建Button，命名为「Btn_StartGame」，按屏幕比例设置尺寸（宽=Screen.width*0.2，高=Screen.height*0.1）；<br>4. 给Button添加缩放反馈（编写`ButtonScale.cs`脚本，按下时localScale=0.95，抬起恢复1）；<br>5. 编写`StartUIManager.cs`脚本，实现点击按钮后销毁UI_Start的逻辑。 | T1的项目工程                              | UI_Start Canvas、ButtonScale.cs、StartUIManager.cs | 1. 启动界面全屏显示，按钮居中；<br>2. 点击按钮有缩放反馈；<br>3. 点击按钮后启动界面销毁，控制台无报错；<br>4. 按钮适配不同屏幕分辨率。| T1        |
| T3      | 游戏场景搭建           | 实现滚动背景与循环地面，模拟无限跑酷的视觉效果，适配鸿蒙手机分辨率         | 1. 创建滚动背景：新建空对象「BG_Scroll」，添加2个与屏幕等宽的Sprite（Unity内置Square Sprite，浅绿+深绿渐变），命名为BG1、BG2，设置Z轴=-1；编写`BackgroundScroll.cs`脚本，实现以5m/s速度向左滚动并循环拼接；<br>2. 创建地面系统：新建地面预制体「Prefab_Ground」（浅棕色Square Sprite，高度=Screen.height*0.08），添加BoxCollider2D和静态Rigidbody2D；编写`GroundManager.cs`脚本，实现3个地面段的循环生成与回收；<br>3. 给地面分配Ground层，设置碰撞矩阵仅与Player层交互。 | T2的项目工程                              | BG_Scroll对象、Prefab_Ground预制体、BackgroundScroll.cs、GroundManager.cs | 1. 背景以5m/s平稳向左滚动，无断层；<br>2. 地面无限循环生成，无空隙；<br>3. 背景和地面适配1080×2400/1200×2600分辨率；<br>4. 控制台无报错。| T2        |
| T4      | 主角对象创建           | 创建冰人主角，添加物理组件、动画控制器与内置动画片段，实现基础动画状态     | 1. 创建主角对象「Player_Ice」，添加蓝色Square Sprite（尺寸=Screen.height*0.12×Screen.height*0.12），分配Player层；<br>2. 添加Rigidbody2D（重力缩放=2，冻结X轴旋转，无水平速度）和BoxCollider2D（与Sprite尺寸匹配）；<br>3. 创建AnimatorController「Player_Ice_Animator」，添加4个动画状态：Idle、Run、Jump、Die；<br>4. 制作动画片段：<br>   - Idle：Sprite静止，时长1秒；<br>   - Run：Sprite左右X偏移±0.1+上下Y偏移±0.05，循环，时长1秒；<br>   - Jump：Sprite向上移动+旋转15°，非循环，时长0.8秒；<br>   - Die：Sprite旋转90°+向下偏移，非循环，时长0.5秒；<br>5. 设置动画过渡条件（布尔参数：isRunning、isJumping、isDead）。 | T3的项目工程                              | Player_Ice对象、Player_Ice_Animator控制器、4个动画片段 | 1. 主角物理组件配置正确，无穿透地面问题；<br>2. Animator包含4个动画状态，过渡条件设置完成；<br>3. 动画片段预览流畅，无僵硬；<br>4. 控制台无动画相关报错。| T3        |
| T5      | 主角运动与动画逻辑     | 实现主角奔跑、跳跃的交互逻辑，以及动画状态的自动切换，适配鸿蒙触屏输入     | 1. 配置Input Actions：创建「PlayerInputActions」资源，添加「Tap」动作（绑定触屏Tap事件）；<br>2. 编写`PlayerController.cs`脚本，挂载到主角对象：<br>   - 实现触屏Tap触发跳跃（落地前仅跳一次，初速度=7m/s向上）；<br>   - 实现动画参数的自动设置（isRunning=true默认，isJumping在跳跃时为true，落地后为false）；<br>   - 限制主角水平位置为Screen.width*0.2，仅垂直移动；<br>3. 添加PlayerInput组件，绑定Input Actions与脚本回调。 | T4的项目工程                              | PlayerInputActions资源、PlayerController.cs脚本 | 1. 点击屏幕任意位置，主角触发跳跃，落地前无法二次跳跃；<br>2. 跳跃高度刚好能越过障碍，下落自然；<br>3. 动画随运动状态自动切换（奔跑→跳跃→奔跑）；<br>4. 鸿蒙触屏点击响应延迟≤50ms，控制台无报错。| T4        |
| T6      | 障碍系统开发           | 实现障碍的对象池管理、随机生成与自动回收，确保性能优化                     | 1. 创建障碍预制体「Prefab_Obstacle」，添加灰色Square Sprite（尺寸=Screen.height*0.1×Screen.height*0.1），分配Obstacle层，添加BoxCollider2D；<br>2. 编写`ObstaclePool.cs`脚本，实现5个障碍的对象池初始化；<br>3. 编写`ObstacleManager.cs`脚本，实现：<br>   - 每2-4秒从对象池取一个障碍，生成在主角右侧Screen.width*0.5~0.8位置；<br>   - 障碍以5m/s向左移动，移出屏幕左侧后回收至对象池；<br>4. 给障碍预制体添加移动脚本「ObstacleMove.cs」。 | T5的项目工程                              | Prefab_Obstacle预制体、ObstaclePool.cs、ObstacleManager.cs、ObstacleMove.cs | 1. 游戏开始后每2-4秒生成一个障碍，无重复创建/销毁；<br>2. 障碍向左移动并自动回收，对象池正常工作；<br>3. 障碍尺寸确保主角一次跳跃可越过；<br>4. 运行时内存占用≤100MB，控制台无报错。| T5        |
| T7      | 碰撞检测与游戏结束     | 实现主角与障碍的碰撞检测，以及游戏结束的视觉与逻辑表现                     | 1. 编写单例`GameManager.cs`脚本，挂载到空对象「GameManager」，管理游戏状态（运行/结束）；<br>2. 在`PlayerController.cs`中添加`OnCollisionEnter2D`方法，监听与障碍的碰撞：<br>   - 碰撞后设置isDead=true，播放死亡动画；<br>   - 调用`GameManager.Instance.GameOver()`方法；<br>3. 在`GameManager`中实现GameOver逻辑：停止背景/地面/障碍移动，禁用主角输入，在Canvas中显示「游戏结束」文字（白色，字号=Screen.height*0.05，居中）。 | T6的项目工程                              | GameManager对象、GameManager.cs（单例）、游戏结束UI文本 | 1. 主角与障碍碰撞后立即播放死亡动画，停止运动；<br>2. 背景、地面、障碍停止移动；<br>3. 屏幕中央显示「游戏结束」文字，触屏输入失效；<br>4. 控制台无碰撞检测报错。| T6        |
| T8      | 鸿蒙适配与本地运行验证 | 完成项目的分辨率适配与鸿蒙打包配置，确保在Unity编辑器和鸿蒙手机中均可运行   | 1. 优化分辨率适配：所有UI/游戏对象的尺寸均通过`Screen.width/Screen.height`计算，无固定像素硬编码；<br>2. 配置Player Settings（File→Build Settings→Player Settings）：<br>   - Product Name：IceFireDashDemo；<br>   - Package Name：com.demo.icefiredash；<br>   - Minimum API Level：API 9；<br>3. 在Unity编辑器中运行场景，验证所有功能；<br>4. 导出鸿蒙HAP包（Build→Build），选择纯英文路径保存。 | T7的项目工程                              | 可运行的Unity Demo、鸿蒙HAP安装包           | 1. Unity编辑器中运行，所有功能正常（启动→奔跑→跳跃→碰撞结束）；<br>2. 导出的HAP包可在鸿蒙4.0+手机安装（开启允许未签名应用）；<br>3. 手机运行时帧率稳定60fps，无卡顿；<br>4. 所有触屏操作响应正常。| T7        |


根据这个 specification，豆包帮我们生成了这些文件：


ButtonScale.cs 按钮缩放反馈

```cs
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
```
