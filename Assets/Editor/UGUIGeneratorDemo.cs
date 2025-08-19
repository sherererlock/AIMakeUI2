using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

namespace UIGenerator
{
    /// <summary>
    /// UGUI生成器演示工具
    /// 提供快速创建测试环境的功能
    /// </summary>
    public class UGUIGeneratorDemo : EditorWindow
    {
        [MenuItem("Tools/UGUI Generator Demo")]
        public static void ShowWindow()
        {
            GetWindow<UGUIGeneratorDemo>("UGUI Generator Demo");
        }

        private void OnGUI()
        {
            GUILayout.Label("UGUI生成器演示工具", EditorStyles.boldLabel);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.HelpBox("这个工具可以帮助您快速设置测试环境", MessageType.Info);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("创建测试Canvas"))
            {
                CreateTestCanvas();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("打开UGUI生成器"))
            {
                UGUIGenerator.ShowWindow();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("检查资源文件"))
            {
                CheckResourceFiles();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("创建EventSystem"))
            {
                CreateEventSystem();
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.HelpBox("使用步骤：\n1. 点击'创建测试Canvas'\n2. 点击'创建EventSystem'\n3. 点击'打开UGUI生成器'\n4. 在生成器中选择Canvas并生成UI", MessageType.Info);
        }

        /// <summary>
        /// 创建测试用的Canvas
        /// </summary>
        private void CreateTestCanvas()
        {
            // 检查是否已存在Canvas
            Canvas existingCanvas = FindObjectOfType<Canvas>();
            if (existingCanvas != null)
            {
                if (EditorUtility.DisplayDialog("Canvas已存在", "场景中已存在Canvas，是否继续创建新的？", "是", "否"))
                {
                    // 继续创建
                }
                else
                {
                    Selection.activeGameObject = existingCanvas.gameObject;
                    return;
                }
            }

            // 创建Canvas GameObject
            GameObject canvasGO = new GameObject("TestCanvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            // 添加CanvasScaler组件
            CanvasScaler canvasScaler = canvasGO.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;

            // 添加GraphicRaycaster组件
            canvasGO.AddComponent<GraphicRaycaster>();

            // 选中创建的Canvas
            Selection.activeGameObject = canvasGO;
            
            Debug.Log("测试Canvas创建完成: " + canvasGO.name);
            EditorUtility.DisplayDialog("成功", "测试Canvas创建完成！\n\n配置信息：\n- 渲染模式: Screen Space - Overlay\n- 参考分辨率: 1920x1080\n- 缩放模式: Scale With Screen Size", "确定");
        }

        /// <summary>
        /// 检查资源文件是否存在
        /// </summary>
        private void CheckResourceFiles()
        {
            string[] resourcePaths = {
                "Assets/G公共组件/T弹窗/bg_tanchuang1.png",
                "Assets/G公共组件/T弹窗/bg_diwen.png",
                "Assets/G公共组件/T弹窗/bg_mubiaodi.png",
                "Assets/G公共组件/A按钮/btn_close.png",
                "Assets/G公共组件/A按钮/btn_lv.png",
                "Assets/G公共组件/T图标/icon_zuanshi.png",
                "Assets/G公共组件/T图标/icon_gou.png",
                "Assets/Res/sprites/icon_tigouyuan.png",
                "Assets/Res/sprites/icon_orange_yuan.png",
                "Assets/Res/sprites/bg_lock.png",
                "Assets/Res/sprites/icon_lock.png"
            };

            System.Text.StringBuilder report = new System.Text.StringBuilder();
            report.AppendLine("资源文件检查报告：\n");
            
            int existingCount = 0;
            int totalCount = resourcePaths.Length;

            foreach (string path in resourcePaths)
            {
                bool exists = System.IO.File.Exists(path);
                if (exists)
                {
                    existingCount++;
                    report.AppendLine($"✓ {path}");
                }
                else
                {
                    report.AppendLine($"✗ {path} (缺失)");
                }
            }

            report.AppendLine($"\n总计: {existingCount}/{totalCount} 个文件存在");
            
            if (existingCount == totalCount)
            {
                report.AppendLine("\n所有资源文件都存在，可以正常生成UI！");
            }
            else
            {
                report.AppendLine("\n部分资源文件缺失，生成的UI可能显示不完整。");
                report.AppendLine("请确保所有图片资源都已导入到正确的路径。");
            }

            Debug.Log(report.ToString());
            EditorUtility.DisplayDialog("资源检查完成", report.ToString(), "确定");
        }

        /// <summary>
        /// 创建EventSystem（用于UI交互）
        /// </summary>
        private void CreateEventSystem()
        {
            // 检查是否已存在EventSystem
            EventSystem existingEventSystem = FindObjectOfType<EventSystem>();
            if (existingEventSystem != null)
            {
                Debug.Log("EventSystem已存在: " + existingEventSystem.name);
                EditorUtility.DisplayDialog("EventSystem已存在", "场景中已存在EventSystem，无需重复创建。", "确定");
                Selection.activeGameObject = existingEventSystem.gameObject;
                return;
            }

            // 创建EventSystem GameObject
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
            eventSystemGO.AddComponent<StandaloneInputModule>();

            // 选中创建的EventSystem
            Selection.activeGameObject = eventSystemGO;
            
            Debug.Log("EventSystem创建完成: " + eventSystemGO.name);
            EditorUtility.DisplayDialog("成功", "EventSystem创建完成！\n\n这是UI交互所必需的组件。", "确定");
        }
    }

    /// <summary>
    /// 快速菜单项
    /// </summary>
    public class UGUIGeneratorQuickMenu
    {
        [MenuItem("GameObject/UI/Generate UGUI from JSON", false, 2000)]
        public static void GenerateUGUIFromJSON()
        {
            // 检查选中的对象是否是Canvas
            GameObject selected = Selection.activeGameObject;
            if (selected != null && selected.GetComponent<Canvas>() != null)
            {
                // 打开UGUI生成器并自动设置Canvas
                UGUIGenerator window = EditorWindow.GetWindow<UGUIGenerator>("UGUI Generator");
                // 这里可以通过反射或公共方法设置Canvas引用
                window.Show();
                
                EditorUtility.DisplayDialog("提示", "UGUI生成器已打开！\n\n请在生成器窗口中：\n1. 设置JSON文件路径\n2. 选择当前Canvas作为目标\n3. 解析并生成UI", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "请先选中一个Canvas对象，然后再使用此功能。", "确定");
            }
        }

        [MenuItem("GameObject/UI/Generate UGUI from JSON", true)]
        public static bool ValidateGenerateUGUIFromJSON()
        {
            // 只有选中Canvas时才显示菜单项
            GameObject selected = Selection.activeGameObject;
            return selected != null && selected.GetComponent<Canvas>() != null;
        }
    }
}