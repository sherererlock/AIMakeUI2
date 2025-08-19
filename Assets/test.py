class Position(BaseModel):
    """元素位置和尺寸信息"""
    x: int
    y: int
    width: int
    height: int

class ElementType(str, Enum):
    """UI元素类型枚举"""
    BUTTON = "button"
    TEXT = "text"
    IMAGE = "image"
    
class UIElement(BaseModel):
    """基本UI元素数据结构"""
    name: str
    type: ElementType
    position: Position
    resource: Optional[str] = None
    children: List['UIElement'] = Field(default_factory=list)