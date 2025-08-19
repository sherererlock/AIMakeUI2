# Unity UGUI Builder 使用说明

## 概述

本项目包含一个Unity编辑器工具，用于根据JSON配置文件自动生成UGUI界面。经过修改后，UIBuilder.cs脚本现在完全兼容startwindow_ugui_config.json文件格式。

## 文件说明

### 核心文件
- `Assets/Editor/UIBuilder.cs` - 主要的UI构建脚本
- `Assets/Editor/UIBuilderTest.cs` - 测试脚本，用于验证JSON解析
- `Assets/Design/startwindow_ugui_config.json` - 完整的UI配置文件
- `Assets/Design/test_simple_config.json` - 简化的测试配置文件

### 资源文件
- `Assets/G公共组件/` - UI组件资源（按钮、图标、弹窗背景等）
- `Assets/Res/sprites/` - 额外的UI精灵资源

## 使用方法

### 1. 生成UI界面

1. 在Unity编辑器中，选择菜单 `Tools > UI Builder`
2. 在弹出的窗口中，输入配置文件路径：
   ```
   Assets/Design/startwindow_ugui_config.json
   ```
3. 点击 "Build UI" 按钮
4. 脚本将在场景中创建完整的UI层级结构

### 2. 测试JSON解析

1. 选择菜单 `Tools > Test UI Builder`
2. 查看Console窗口中的解析结果和组件信息

## JSON配置文件格式

### 基本结构
```json
{
  "ui_config": {
    "window_name": "窗口名称",
    "window_type": "popup",
    "canvas_size": {
      "width": 1920,
      "height": 1080
    },
    "components": [
      // 组件配置数组
    ]
  }
}
```

### 组件配置
```json
{
  "id": "唯一标识符",
  "component_type": "Image|Text|Button|Panel",
  "name": "GameObject名称",
  "position": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "size": {
    "width": 100,
    "height": 100
  },
  "sprite_path": "Assets/路径/图片.png",
  "image_type": "Simple|Sliced",
  "color": {
    "r": 1.0,
    "g": 1.0,
    "b": 1.0,
    "a": 1.0
  },
  "children": [
    // 子组件配置
  ]
}
```

## 支持的组件类型

### Image组件
- 支持精灵图片
- 支持九宫格拉伸（Sliced）和简单（Simple）类型
- 支持RGBA颜色设置

### Text组件
- 使用TextMeshPro组件
- 支持字体大小设置
- 支持文本对齐（MiddleCenter等）
- 支持颜色设置

### Button组件
- 自动添加Button和Image组件
- 支持精灵图片和图像类型设置
- 支持颜色过渡效果

### Panel组件
- 作为容器使用，不添加额外组件
- 支持嵌套子组件

## 注意事项

1. **图片资源**: 确保所有在JSON中引用的图片文件都存在于指定路径
2. **TextMeshPro**: 首次使用文本组件时，Unity可能会提示导入TextMeshPro资源
3. **九宫格设置**: 对于需要拉伸的图片，建议在Unity中预先设置好九宫格边界
4. **层级关系**: 组件的渲染顺序由在JSON中的定义顺序决定

## 故障排除

### 常见问题

1. **图片不显示**
   - 检查图片路径是否正确
   - 确认图片已正确导入Unity项目
   - 检查图片的Import Settings中Texture Type是否设置为Sprite

2. **JSON解析失败**
   - 使用JSON验证工具检查文件格式
   - 确认所有必需字段都已填写
   - 检查是否有语法错误（如缺少逗号、括号不匹配等）

3. **组件位置不正确**
   - 检查锚点设置
   - 确认父子关系是否正确
   - 验证坐标值是否合理

## 扩展功能

如需添加新的组件类型或属性，可以：

1. 在`ComponentConfig`类中添加新字段
2. 在`CreateUIComponent`方法的switch语句中添加新的case
3. 实现相应的组件创建和属性设置逻辑

## 版本信息

- Unity版本: 2021.3 LTS或更高
- 依赖: Newtonsoft.Json, TextMeshPro
- 最后更新: 2024年