using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// StartWindow界面控制器
/// 用于控制由UGUI生成器创建的StartWindow界面的交互逻辑
/// </summary>
public class StartWindowController : MonoBehaviour
{
    [Header("UI引用")]
    public Button closeButton;
    public Button playButton;
    public Button[] itemButtons;
    public Text levelText;
    public Text playText;
    public Text[] targetNumbers;
    
    [Header("游戏数据")]
    public int currentLevel = 1;
    public int[] targetValues = { 4, 2 };
    public bool[] itemUnlocked = { true, false, false, false };
    
    private void Start()
    {
        InitializeUI();
        SetupEventListeners();
        UpdateUI();
    }
    
    /// <summary>
    /// 初始化UI引用
    /// 如果没有手动分配引用，尝试自动查找
    /// </summary>
    private void InitializeUI()
    {
        // 自动查找UI组件（如果没有手动分配）
        if (closeButton == null)
            closeButton = FindUIElement<Button>("CloseButton");
            
        if (playButton == null)
            playButton = FindUIElement<Button>("PlayButton");
            
        if (levelText == null)
            levelText = FindUIElement<Text>("LevelText");
            
        if (playText == null)
            playText = FindUIElement<Text>("PlayText");
            
        // 查找目标数字文本
        if (targetNumbers == null || targetNumbers.Length == 0)
        {
            List<Text> targetTexts = new List<Text>();
            Text targetNumber4 = FindUIElement<Text>("TargetNumber4");
            Text targetNumber2 = FindUIElement<Text>("TargetNumber2");
            
            if (targetNumber4 != null) targetTexts.Add(targetNumber4);
            if (targetNumber2 != null) targetTexts.Add(targetNumber2);
            
            targetNumbers = targetTexts.ToArray();
        }
        
        // 查找道具按钮
        if (itemButtons == null || itemButtons.Length == 0)
        {
            List<Button> buttons = new List<Button>();
            for (int i = 1; i <= 4; i++)
            {
                Button itemButton = FindUIElement<Button>($"Item{i}");
                if (itemButton != null)
                    buttons.Add(itemButton);
            }
            itemButtons = buttons.ToArray();
        }
    }
    
    /// <summary>
    /// 设置事件监听器
    /// </summary>
    private void SetupEventListeners()
    {
        // 关闭按钮事件
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        
        // 开始游戏按钮事件
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        
        // 道具按钮事件
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (itemButtons[i] != null)
            {
                int index = i; // 闭包变量
                itemButtons[i].onClick.AddListener(() => OnItemButtonClicked(index));
            }
        }
    }
    
    /// <summary>
    /// 更新UI显示
    /// </summary>
    private void UpdateUI()
    {
        // 更新关卡文本
        if (levelText != null)
        {
            levelText.text = $"第{currentLevel}关";
        }
        
        // 更新开始按钮文本
        if (playText != null)
        {
            playText.text = "开始游戏";
        }
        
        // 更新目标数字
        for (int i = 0; i < targetNumbers.Length && i < targetValues.Length; i++)
        {
            if (targetNumbers[i] != null)
            {
                targetNumbers[i].text = targetValues[i].ToString();
            }
        }
        
        // 更新道具按钮状态
        UpdateItemButtons();
    }
    
    /// <summary>
    /// 更新道具按钮状态
    /// </summary>
    private void UpdateItemButtons()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (itemButtons[i] != null)
            {
                // 设置按钮是否可交互
                bool isUnlocked = i < itemUnlocked.Length ? itemUnlocked[i] : false;
                itemButtons[i].interactable = isUnlocked;
                
                // 查找锁定图标和勾选图标
                Transform lockIcon = itemButtons[i].transform.Find("LockIcon");
                Transform checkMark = itemButtons[i].transform.Find("CheckMark");
                
                // 显示/隐藏锁定图标
                if (lockIcon != null)
                {
                    lockIcon.gameObject.SetActive(!isUnlocked);
                }
                
                // 显示/隐藏勾选图标（第一个道具默认选中）
                if (checkMark != null)
                {
                    checkMark.gameObject.SetActive(isUnlocked && i == 0);
                }
            }
        }
    }
    
    /// <summary>
    /// 关闭按钮点击事件
    /// </summary>
    private void OnCloseButtonClicked()
    {
        Debug.Log("关闭按钮被点击");
        
        // 隐藏弹窗或关闭界面
        GameObject popupDialog = FindUIElement<Transform>("PopupDialog")?.gameObject;
        if (popupDialog != null)
        {
            popupDialog.SetActive(false);
        }
        else
        {
            // 如果没有找到弹窗，则销毁整个界面
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 开始游戏按钮点击事件
    /// </summary>
    private void OnPlayButtonClicked()
    {
        Debug.Log($"开始第{currentLevel}关游戏");
        
        // 这里可以添加场景切换或游戏开始的逻辑
        // 例如：SceneManager.LoadScene("GameScene");
        
        // 临时演示：增加关卡数
        currentLevel++;
        UpdateUI();
    }
    
    /// <summary>
    /// 道具按钮点击事件
    /// </summary>
    private void OnItemButtonClicked(int itemIndex)
    {
        Debug.Log($"道具{itemIndex + 1}被点击");
        
        // 检查道具是否解锁
        if (itemIndex < itemUnlocked.Length && itemUnlocked[itemIndex])
        {
            Debug.Log($"选择了道具{itemIndex + 1}");
            
            // 更新勾选状态
            UpdateItemSelection(itemIndex);
        }
        else
        {
            Debug.Log($"道具{itemIndex + 1}尚未解锁");
            // 可以显示解锁提示
        }
    }
    
    /// <summary>
    /// 更新道具选择状态
    /// </summary>
    private void UpdateItemSelection(int selectedIndex)
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (itemButtons[i] != null)
            {
                Transform checkMark = itemButtons[i].transform.Find("CheckMark");
                if (checkMark != null)
                {
                    // 只有选中的道具显示勾选图标
                    bool isSelected = (i == selectedIndex) && (i < itemUnlocked.Length && itemUnlocked[i]);
                    checkMark.gameObject.SetActive(isSelected);
                }
            }
        }
    }
    
    /// <summary>
    /// 解锁道具
    /// </summary>
    public void UnlockItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < itemUnlocked.Length)
        {
            itemUnlocked[itemIndex] = true;
            UpdateItemButtons();
            Debug.Log($"道具{itemIndex + 1}已解锁");
        }
    }
    
    /// <summary>
    /// 通用UI元素查找方法
    /// </summary>
    private T FindUIElement<T>(string elementName) where T : Component
    {
        Transform found = FindChildRecursive(transform, elementName);
        return found != null ? found.GetComponent<T>() : null;
    }
    
    /// <summary>
    /// 递归查找子对象
    /// </summary>
    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;
                
            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }
        return null;
    }
    
    /// <summary>
    /// 公共方法：显示界面
    /// </summary>
    public void ShowWindow()
    {
        gameObject.SetActive(true);
        
        // 重新初始化UI（如果需要）
        UpdateUI();
    }
    
    /// <summary>
    /// 公共方法：隐藏界面
    /// </summary>
    public void HideWindow()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 设置关卡数据
    /// </summary>
    public void SetLevelData(int level, int[] targets, bool[] unlockedItems)
    {
        currentLevel = level;
        if (targets != null)
            targetValues = targets;
        if (unlockedItems != null)
            itemUnlocked = unlockedItems;
            
        UpdateUI();
    }
}