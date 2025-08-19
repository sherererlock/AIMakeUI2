using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace UIGenerator
{
    /// <summary>
    /// UI元素数据结构
    /// </summary>
    [Serializable]
    public class UIElementData
    {
        public string name;
        public string type;
        public PositionData position;
        public string resource;
        public List<UIElementData> children = new List<UIElementData>();
    }

    /// <summary>
    /// 位置数据结构
    /// </summary>
    [Serializable]
    public class PositionData
    {
        public float x;
        public float y;
        public float width;
        public float height;
    }

    /// <summary>
    /// UGUI界面生成器
    /// </summary>
    public class UGUIGenerator : EditorWindow
    {
        private string jsonFilePath = "Assets/Design/startwindow_ugui_config.json";
        private Canvas targetCanvas;
        private UIElementData rootData;

        [MenuItem("Tools/UGUI Generator")]
        public static void ShowWindow()
        {
            GetWindow<UGUIGenerator>("UGUI Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("UGUI界面生成器", EditorStyles.boldLabel);
            
            EditorGUILayout.Space();
            
            // JSON文件路径选择
            EditorGUILayout.LabelField("JSON配置文件路径:");
            jsonFilePath = EditorGUILayout.TextField(jsonFilePath);
            
            if (GUILayout.Button("选择JSON文件"))
            {
                string path = EditorUtility.OpenFilePanel("选择JSON配置文件", "Assets", "json");
                if (!string.IsNullOrEmpty(path))
                {
                    jsonFilePath = "Assets" + path.Substring(Application.dataPath.Length);
                }
            }
            
            EditorGUILayout.Space();
            
            // 目标Canvas选择
            EditorGUILayout.LabelField("目标Canvas:");
            targetCanvas = (Canvas)EditorGUILayout.ObjectField(targetCanvas, typeof(Canvas), true);
            
            EditorGUILayout.Space();
            
            // 解析JSON按钮
            if (GUILayout.Button("解析JSON文件"))
            {
                ParseJsonFile();
            }
            
            // 生成UI按钮
            if (GUILayout.Button("生成UGUI界面") && rootData != null && targetCanvas != null)
            {
                GenerateUI();
            }
            
            EditorGUILayout.Space();
            
            // 显示解析结果
            if (rootData != null)
            {
                EditorGUILayout.LabelField("解析结果:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"根元素: {rootData.name}");
                EditorGUILayout.LabelField($"类型: {rootData.type}");
                EditorGUILayout.LabelField($"子元素数量: {rootData.children.Count}");
            }
        }

        /// <summary>
        /// 解析JSON文件
        /// </summary>
        private void ParseJsonFile()
        {
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    EditorUtility.DisplayDialog("错误", "JSON文件不存在: " + jsonFilePath, "确定");
                    return;
                }

                string jsonContent = File.ReadAllText(jsonFilePath);
                rootData = JsonUtility.FromJson<UIElementData>(jsonContent);
                
                if (rootData != null)
                {
                    Debug.Log("JSON文件解析成功: " + rootData.name);
                }
                else
                {
                    EditorUtility.DisplayDialog("错误", "JSON文件解析失败", "确定");
                }
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("错误", "解析JSON文件时出错: " + e.Message, "确定");
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 生成UGUI界面
        /// </summary>
        private void GenerateUI()
        {
            if (rootData == null || targetCanvas == null)
            {
                EditorUtility.DisplayDialog("错误", "请先解析JSON文件并选择目标Canvas", "确定");
                return;
            }

            try
            {
                // 清理现有的UI元素（可选）
                if (EditorUtility.DisplayDialog("确认", "是否清理Canvas下的现有UI元素？", "是", "否"))
                {
                    ClearCanvas();
                }

                // 创建根UI元素
                GameObject rootObject = CreateUIElement(rootData, targetCanvas.transform);
                
                // 设置Canvas的参考分辨率
                CanvasScaler canvasScaler = targetCanvas.GetComponent<CanvasScaler>();
                if (canvasScaler != null)
                {
                    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    canvasScaler.referenceResolution = new Vector2(rootData.position.width, rootData.position.height);
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    canvasScaler.matchWidthOrHeight = 0.5f;
                }

                Debug.Log("UGUI界面生成完成: " + rootData.name);
                EditorUtility.DisplayDialog("成功", "UGUI界面生成完成!", "确定");
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("错误", "生成UI时出错: " + e.Message, "确定");
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 清理Canvas下的UI元素
        /// </summary>
        private void ClearCanvas()
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in targetCanvas.transform)
            {
                children.Add(child);
            }
            
            foreach (Transform child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        /// <summary>
        /// 创建UI元素
        /// </summary>
        private GameObject CreateUIElement(UIElementData data, Transform parent)
        {
            GameObject uiObject = new GameObject(data.name);
            uiObject.transform.SetParent(parent, false);

            // 添加RectTransform组件
            RectTransform rectTransform = uiObject.AddComponent<RectTransform>();
            SetRectTransform(rectTransform, data.position, parent as RectTransform);

            // 根据类型添加相应的组件
            switch (data.type.ToLower())
            {
                case "image":
                    CreateImageComponent(uiObject, data);
                    break;
                case "button":
                    CreateButtonComponent(uiObject, data);
                    break;
                case "text":
                    CreateTextComponent(uiObject, data);
                    break;
            }

            // 递归创建子元素
            foreach (UIElementData childData in data.children)
            {
                CreateUIElement(childData, uiObject.transform);
            }

            return uiObject;
        }

        /// <summary>
        /// 设置RectTransform
        /// </summary>
        private void SetRectTransform(RectTransform rectTransform, PositionData position, RectTransform parentRect)
        {
            // 设置锚点为左上角
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            
            // 设置大小
            rectTransform.sizeDelta = new Vector2(position.width, position.height);
            
            // 设置位置（Unity的Y轴是向上的，需要转换）
            float parentHeight = parentRect != null ? parentRect.sizeDelta.y : 1080;
            rectTransform.anchoredPosition = new Vector2(position.x, -(position.y));
        }

        /// <summary>
        /// 创建Image组件
        /// </summary>
        private void CreateImageComponent(GameObject uiObject, UIElementData data)
        {
            Image image = uiObject.AddComponent<Image>();
            
            if (!string.IsNullOrEmpty(data.resource))
            {
                Sprite sprite = LoadSprite(data.resource);
                if (sprite != null)
                {
                    image.sprite = sprite;
                    image.type = Image.Type.Sliced; // 支持九宫格拉伸
                }
            }
            else
            {
                image.color = Color.clear; // 透明背景
            }
        }

        /// <summary>
        /// 创建Button组件
        /// </summary>
        private void CreateButtonComponent(GameObject uiObject, UIElementData data)
        {
            Image image = uiObject.AddComponent<Image>();
            Button button = uiObject.AddComponent<Button>();
            
            if (!string.IsNullOrEmpty(data.resource))
            {
                Sprite sprite = LoadSprite(data.resource);
                if (sprite != null)
                {
                    image.sprite = sprite;
                    image.type = Image.Type.Sliced; // 支持九宫格拉伸
                }
            }
            
            // 设置按钮的默认颜色状态
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            button.colors = colors;
        }

        /// <summary>
        /// 创建Text组件
        /// </summary>
        private void CreateTextComponent(GameObject uiObject, UIElementData data)
        {
            Text text = uiObject.AddComponent<Text>();
            text.text = data.name; // 使用元素名称作为默认文本
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 24;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
        }

        /// <summary>
        /// 加载Sprite资源
        /// </summary>
        private Sprite LoadSprite(string resourcePath)
        {
            try
            {
                // 构建完整的资源路径
                string fullPath = "Assets/" + resourcePath;
                
                // 尝试加载资源
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(fullPath);
                
                if (sprite == null)
                {
                    Debug.LogWarning($"无法加载Sprite资源: {fullPath}");
                }
                
                return sprite;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载Sprite资源时出错: {resourcePath}, 错误: {e.Message}");
                return null;
            }
        }
    }
}