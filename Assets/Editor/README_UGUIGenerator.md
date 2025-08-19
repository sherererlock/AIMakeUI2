# UGUI生成器使用说明

## 功能介绍

UGUI生成器是一个Unity编辑器工具，可以根据JSON配置文件自动生成UGUI界面。它支持解析包含UI元素层次结构、位置信息和资源引用的JSON文件，并在Unity中创建对应的UI界面。

## 使用步骤

### 1. 打开UGUI生成器窗口
- 在Unity编辑器菜单栏中选择 `Tools > UGUI Generator`
- 这将打开UGUI生成器窗口

### 2. 准备Canvas
- 在场景中创建一个Canvas对象
- 建议设置Canvas的Render Mode为Screen Space - Overlay
- 添加CanvasScaler组件以支持多分辨率适配

### 3. 配置JSON文件路径
- 在UGUI生成器窗口中，指定JSON配置文件的路径
- 默认路径为：`Assets/Design/startwindow_ugui_config.json`
- 可以点击"选择JSON文件"按钮来浏览选择文件

### 4. 选择目标Canvas
- 在"目标Canvas"字段中，拖拽或选择场景中的Canvas对象

### 5. 解析JSON文件
- 点击"解析JSON文件"按钮
- 系统会解析JSON文件并显示解析结果

### 6. 生成UGUI界面
- 点击"生成UGUI界面"按钮
- 系统会询问是否清理Canvas下的现有UI元素
- 确认后开始生成UI界面

## JSON配置文件格式

### 基本结构
```json
{
  "name": "元素名称",
  "type": "元素类型",
  "position": {
    "x": 0,
    "y": 0,
    "width": 100,
    "height": 100
  },
  "resource": "资源路径",
  "children": []
}
```

### 支持的元素类型
- **image**: 图片元素，对应Unity的Image组件
- **button**: 按钮元素，对应Unity的Button组件
- **text**: 文本元素，对应Unity的Text组件

### 位置信息
- **x, y**: 元素的左上角坐标（相对于父元素）
- **width, height**: 元素的宽度和高度
- 坐标系统：左上角为原点，X轴向右，Y轴向下

### 资源路径
- 相对于Assets文件夹的路径
- 例如：`"G公共组件/A按钮/btn_close.png"`
- 如果resource为null，则创建透明背景的元素

## 特性说明

### 九宫格拉伸支持
- 所有Image和Button组件默认设置为Sliced类型
- 支持九宫格拉伸，小图可以拉伸成各种尺寸而不变形

### 自动分辨率适配
- 生成UI时会自动设置CanvasScaler的参考分辨率
- 参考分辨率取自JSON根元素的尺寸
- 默认使用MatchWidthOrHeight模式，匹配值为0.5

### 层次结构
- 支持无限层级的UI元素嵌套
- 子元素的位置相对于父元素计算
- 保持JSON中定义的父子关系

## 注意事项

1. **资源路径**: 确保JSON中引用的所有图片资源都存在于指定路径
2. **Canvas设置**: 建议使用Screen Space - Overlay模式的Canvas
3. **备份**: 生成UI前建议备份现有场景
4. **资源导入**: 确保所有图片资源已正确导入Unity项目
5. **Sprite设置**: 图片资源需要设置为Sprite类型才能被正确加载

## 故障排除

### 常见问题

**Q: JSON解析失败**
A: 检查JSON文件格式是否正确，确保所有括号和引号匹配

**Q: 图片资源加载失败**
A: 检查资源路径是否正确，确保图片已导入为Sprite类型

**Q: UI元素位置不正确**
A: 检查Canvas的设置，确保使用了正确的锚点和缩放模式

**Q: 按钮无法点击**
A: 确保Canvas上有GraphicRaycaster组件，场景中有EventSystem对象

## 扩展功能

如需添加更多UI组件类型支持，可以在`CreateUIElement`方法中添加新的case分支，并实现对应的组件创建逻辑。

## 技术支持

如遇到问题，请检查Unity Console窗口中的错误信息，这将有助于定位问题所在。