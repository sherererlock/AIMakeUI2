using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// UI Generator 使用示例
/// 展示如何在运行时与生成的UI进行交互
/// </summary>
public class UIGeneratorExample : MonoBehaviour
{
    [Header("UI References")]
    public Button playButton;
    public Button closeButton;
    public Button[] itemButtons;
    
    [Header("UI Containers")]
    public GameObject startWindow;
    public GameObject popupDialog;
    public GameObject targetArea;
    public GameObject itemsArea;
    
    private void Start()
    {
        // 自动查找生成的UI元素
        FindGeneratedUIElements();
        
        // 设置按钮事件
        SetupButtonEvents();
        
        // 初始化UI状态
        InitializeUIState();
    }
    
    /// <summary>
    /// 自动查找由UI Generator生成的UI元素
    /// </summary>
    private void FindGeneratedUIElements()
    {
        // 查找主要容器
        startWindow = GameObject.Find("StartWindow");
        if (startWindow != null)
        {
            Debug.Log("Found StartWindow container");
            
            // 查找弹窗
            Transform popupTransform = startWindow.transform.Find("PopupDialog");
            if (popupTransform != null)
            {
                popupDialog = popupTransform.gameObject;
                Debug.Log("Found PopupDialog");
                
                // 查找按钮
                Transform playButtonTransform = FindChildRecursive(popupTransform, "PlayButton");
                if (playButtonTransform != null)
                {
                    playButton = playButtonTransform.GetComponent<Button>();
                    Debug.Log("Found PlayButton");
                }
                
                Transform closeButtonTransform = FindChildRecursive(popupTransform, "CloseButton");
                if (closeButtonTransform != null)
                {
                    closeButton = closeButtonTransform.GetComponent<Button>();
                    Debug.Log("Found CloseButton");
                }
                
                // 查找区域容器
                Transform targetTransform = FindChildRecursive(popupTransform, "TargetArea");
                if (targetTransform != null)
                {
                    targetArea = targetTransform.gameObject;
                    Debug.Log("Found TargetArea");
                }
                
                Transform itemsTransform = FindChildRecursive(popupTransform, "ItemsArea");
                if (itemsTransform != null)
                {
                    itemsArea = itemsTransform.gameObject;
                    Debug.Log("Found ItemsArea");
                    
                    // 查找道具按钮
                    FindItemButtons(itemsTransform);
                }
            }
        }
        else
        {
            Debug.LogWarning("StartWindow not found! Please generate UI first using Tools > UI Generator");
        }
    }
    
    /// <summary>
    /// 递归查找子对象
    /// </summary>
    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            
            Transform found = FindChildRecursive(child, childName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
    
    /// <summary>
    /// 查找道具按钮
    /// </summary>
    private void FindItemButtons(Transform itemsContainer)
    {
        var buttonList = new System.Collections.Generic.List<Button>();
        
        // 查找所有包含"PowerUp"的按钮
        for (int i = 0; i < itemsContainer.childCount; i++)
        {
            Transform child = itemsContainer.GetChild(i);
            if (child.name.Contains("PowerUp"))
            {
                Button button = child.GetComponent<Button>();
                if (button != null)
                {
                    buttonList.Add(button);
                    Debug.Log($"Found item button: {child.name}");
                }
            }
        }
        
        itemButtons = buttonList.ToArray();
    }
    
    /// <summary>
    /// 设置按钮事件
    /// </summary>
    private void SetupButtonEvents()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        
        if (itemButtons != null)
        {
            for (int i = 0; i < itemButtons.Length; i++)
            {
                int index = i; // 闭包变量
                itemButtons[i].onClick.AddListener(() => OnItemButtonClicked(index));
            }
        }
    }
    
    /// <summary>
    /// 初始化UI状态
    /// </summary>
    private void InitializeUIState()
    {
        // 设置初始显示状态
        if (popupDialog != null)
        {
            popupDialog.SetActive(true);
        }
        
        // 播放弹窗动画
        if (popupDialog != null)
        {
            StartCoroutine(PlayPopupAnimation());
        }
    }
    
    /// <summary>
    /// 播放弹窗出现动画
    /// </summary>
    private IEnumerator PlayPopupAnimation()
    {
        if (popupDialog == null) yield break;
        
        RectTransform rectTransform = popupDialog.GetComponent<RectTransform>();
        if (rectTransform == null) yield break;
        
        // 从小到大的缩放动画
        Vector3 originalScale = rectTransform.localScale;
        rectTransform.localScale = Vector3.zero;
        
        float duration = 0.3f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            // 使用缓动函数
            float scale = Mathf.Lerp(0f, 1f, EaseOutBack(progress));
            rectTransform.localScale = originalScale * scale;
            
            yield return null;
        }
        
        rectTransform.localScale = originalScale;
    }
    
    /// <summary>
    /// 缓动函数：回弹效果
    /// </summary>
    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
    
    #region 按钮事件处理
    
    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked!");
        
        // 播放按钮点击效果
        if (playButton != null)
        {
            StartCoroutine(ButtonClickEffect(playButton.transform));
        }
        
        // 这里可以添加游戏开始逻辑
        StartGame();
    }
    
    private void OnCloseButtonClicked()
    {
        Debug.Log("Close button clicked!");
        
        // 播放关闭动画
        if (popupDialog != null)
        {
            StartCoroutine(ClosePopupAnimation());
        }
    }
    
    private void OnItemButtonClicked(int itemIndex)
    {
        Debug.Log($"Item button {itemIndex} clicked!");
        
        if (itemButtons != null && itemIndex < itemButtons.Length)
        {
            // 播放按钮点击效果
            StartCoroutine(ButtonClickEffect(itemButtons[itemIndex].transform));
            
            // 这里可以添加道具选择逻辑
            SelectItem(itemIndex);
        }
    }
    
    #endregion
    
    #region 游戏逻辑
    
    private void StartGame()
    {
        Debug.Log("Starting game...");
        // 这里添加游戏开始的逻辑
    }
    
    private void SelectItem(int itemIndex)
    {
        Debug.Log($"Selected item: {itemIndex}");
        // 这里添加道具选择的逻辑
    }
    
    #endregion
    
    #region 动画效果
    
    private IEnumerator ButtonClickEffect(Transform buttonTransform)
    {
        if (buttonTransform == null) yield break;
        
        Vector3 originalScale = buttonTransform.localScale;
        
        // 按下效果
        float duration = 0.1f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0.95f, elapsed / duration);
            buttonTransform.localScale = originalScale * scale;
            yield return null;
        }
        
        // 弹起效果
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(0.95f, 1f, elapsed / duration);
            buttonTransform.localScale = originalScale * scale;
            yield return null;
        }
        
        buttonTransform.localScale = originalScale;
    }
    
    private IEnumerator ClosePopupAnimation()
    {
        if (popupDialog == null) yield break;
        
        RectTransform rectTransform = popupDialog.GetComponent<RectTransform>();
        if (rectTransform == null) yield break;
        
        Vector3 originalScale = rectTransform.localScale;
        
        float duration = 0.2f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            float scale = Mathf.Lerp(1f, 0f, progress);
            rectTransform.localScale = originalScale * scale;
            yield return null;
        }
        
        popupDialog.SetActive(false);
        rectTransform.localScale = originalScale;
    }
    
    #endregion
    
    #region 公共方法
    
    /// <summary>
    /// 显示弹窗
    /// </summary>
    public void ShowPopup()
    {
        if (popupDialog != null)
        {
            popupDialog.SetActive(true);
            StartCoroutine(PlayPopupAnimation());
        }
    }
    
    /// <summary>
    /// 隐藏弹窗
    /// </summary>
    public void HidePopup()
    {
        if (popupDialog != null)
        {
            StartCoroutine(ClosePopupAnimation());
        }
    }
    
    #endregion
}