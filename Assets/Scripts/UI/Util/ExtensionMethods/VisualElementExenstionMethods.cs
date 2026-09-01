using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Utils
{
    public static class VisualElementExtensionMethods
    {
        public static T Name<T>(this T element, string name) where T : VisualElement
        {
            element.name = name;
            return element;
        }

        public static T AddElement<T>(this T element, VisualElement child) where T : VisualElement
        {
            element.Add(child);
            return element;
        }
        
        public static T AbsPos<T>(this T element) where T : VisualElement
        {
            element.style.position = UnityEngine.UIElements.Position.Absolute;
            return element;
        }
        
        public static T RelPos<T>(this T element) where T : VisualElement
        {
            element.style.position = UnityEngine.UIElements.Position.Relative;
            return element;
        }

        public static T Display<T>(this T element, DisplayStyle display) where T : VisualElement
        {
            element.style.display = display;
            return element;
        }

        public static T Opacity<T>(this T element, float opacity) where T : VisualElement
        {
            element.style.opacity = opacity;
            return element;
        }
        
        public static T PickingMode<T>(this T element, PickingMode pickingMode) where T : VisualElement
        {
            element.pickingMode = pickingMode;
            return element;
        }

        public static T Position<T>(this T element, Vector2 position) where T : VisualElement
        {
            element.transform.position = position;
            return element;
        }
        
        public static T Position<T>(this T element, int x, int? y = null) where T : VisualElement
        {
            element.transform.position = new(x, y ?? x);
            return element;
        }

        public static T Rotation<T>(this T element, float angle) where T : VisualElement
        {
            element.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            return element;
        }

        public static T Scale<T>(this T element, Vector3 scale) where T : VisualElement
        {
            element.transform.scale = scale;
            return element;
        }

        public static T FlipHorizontal<T>(this T element) where T : VisualElement
        {
            element.transform.scale = new(-element.transform.scale.x, element.transform.scale.y);
            return element;
        }

        public static T FlipVertical<T>(this T element) where T : VisualElement
        {
            element.transform.scale = new(element.transform.scale.x, -element.transform.scale.y);
            return element;
        }

        public static T Align<T>(this T element, Align align) where T : VisualElement
        {
            element.style.alignSelf = align;
            return element;
        }

        public static T AlignItems<T>(this T element, Align align) where T : VisualElement
        {
            element.style.alignItems = align;
            return element;
        }

        public static T AlignContent<T>(this T element, Align align) where T : VisualElement
        {
            element.style.alignContent = align;
            return element;
        }

        public static T JustifyContent<T>(this T element, Justify justify) where T : VisualElement
        {
            element.style.justifyContent = justify;
            return element;
        }

        public static T LayoutOffset<T>(this T element, float x, float? y = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            var xValue = new Length(x, unit);
            var yValue = new Length(y ?? x, unit);
                element.style.top = yValue;
                element.style.left = xValue;
            if (element.parent == null)
            {
                element.style.top = yValue;
                element.style.left = xValue;
            }
            else
            {
                var direction = element.parent.style.flexDirection.value;
                var align = element.style.alignSelf.value;
                if (direction == UnityEngine.UIElements.FlexDirection.Row || 
                    direction == UnityEngine.UIElements.FlexDirection.RowReverse)
                {
                    if (direction == UnityEngine.UIElements.FlexDirection.Row)
                        element.style.left = xValue;
                    else
                        element.style.right = xValue;
                    
                    if (align != UnityEngine.UIElements.Align.FlexEnd)
                        element.style.top = yValue;
                    else
                        element.style.bottom = yValue;
                }
                else
                {
                    if (direction == UnityEngine.UIElements.FlexDirection.Column)
                        element.style.top = yValue;
                    else
                        element.style.bottom = yValue;
                    
                    if (align != UnityEngine.UIElements.Align.FlexEnd)
                        element.style.left = xValue;
                    else
                        element.style.right = xValue;
                }
            }
            return element;
        }

        public static T Offset<T>(this T element, float x, float? y = null, LengthUnit unit = LengthUnit.Pixel)
            where T : VisualElement
        {
            element.style.translate = new StyleTranslate(
                new Translate(
                    new(x, unit), 
                    new(y ?? x, unit)
                    ));
            return element;
        }
        
        public static T Pivot<T>(this T element, float x, float? y = null, LengthUnit unit = LengthUnit.Pixel)
            where T : VisualElement
        {
            element.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(
                new Length(x, unit),
                new Length(y ?? x, unit)));
            return element;
        }
        
        public static T MinSize<T>(this T element, Vector2 size, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.minWidth = new Length(size.x, unit);
            element.style.minHeight = new Length(size.y, unit);
            return element;
        }

        public static T MinSize<T>(this T element, float width, float? height = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.minWidth = new Length(width, unit);
            element.style.minHeight = new Length(height ?? width, unit);
            return element;
        }

        public static T MaxSize<T>(this T element, Vector2 size, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.maxWidth = new Length(size.x, unit);
            element.style.maxHeight = new Length(size.y, unit);
            return element;
        }

        public static T MaxSize<T>(this T element, float width, float? height = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.maxWidth = new Length(width, unit);
            element.style.maxHeight = new Length(height ?? width, unit);
            return element;
        }

        public static T Size<T>(this T element, Vector2 size, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.width = new Length(size.x, unit);
            element.style.height = new Length(size.y, unit);
            return element;
        }

        public static T Size<T>(this T element, float width, float? height = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.width = new Length(width, unit);
            element.style.height = new Length(height ?? width, unit);
            return element;
        }

        public static T Width<T>(this T element, float width, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.width = new Length(width, unit);
            return element;
        }

        public static T Height<T>(this T element, float height, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.height = new Length(height, unit);
            return element;
        }

        public static T BgColor<T>(this T element, Color color) where T : VisualElement
        {
            element.style.backgroundColor = color;
            return element;
        }

        public static T Color<T>(this T element, Color color) where T : VisualElement
        {
            element.style.color = color;
            return element;
        }
        
        public static T BgTint<T>(this T element, Color color) where T : VisualElement
        {
            element.style.unityBackgroundImageTintColor = color;
            return element;
        }

        public static T FlexDirection<T>(this T element, FlexDirection direction) where T : VisualElement
        {
            element.style.flexDirection = direction;
            return element;
        }

        public static T Wrap<T>(this T element, Wrap wrap) where T : VisualElement
        {
            element.style.flexWrap = wrap;
            return element;
        }
        
        public static T Grow<T>(this T element, float grow = 1) where T : VisualElement
        {
            element.style.flexGrow = grow;
            return element;
        }

        public static T Shrink<T>(this T element, float shrink = 1) where T : VisualElement
        {
            element.style.flexShrink = shrink;
            return element;
        }

        public static T Padding<T>(this T element, float left, float? top = null, float? right = null, float? bottom = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.paddingLeft = new Length(left, unit);
            element.style.paddingRight = new Length(right ?? left, unit);
            element.style.paddingTop = new Length(top ?? left, unit);
            element.style.paddingBottom = new Length(bottom ?? top ?? left, unit);
            return element;
        }

        public static T Margin<T>(this T element, float left, float? top = null, float? right = null, float? bottom = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.marginLeft = new Length(left, unit);
            element.style.marginRight = new Length(right ?? left, unit);
            element.style.marginTop = new Length(top ?? left, unit);
            element.style.marginBottom = new Length(bottom ?? top ?? left, unit);
            return element;
        }

        public static T BorderSize<T>(this T element, float left, float? top = null, float? right = null, float? bottom = null) where T : VisualElement
        {
            element.style.borderLeftWidth = left;
            element.style.borderRightWidth = right ?? left;
            element.style.borderTopWidth = top ?? left;
            element.style.borderBottomWidth = bottom ?? top ?? left;
            return element;
        }

        public static T BorderRadius<T>(this T element, float left, float? top = null, float? right = null, float? bottom = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.borderTopLeftRadius = new Length(left, unit);
            element.style.borderTopRightRadius = new Length(right ?? left, unit);
            element.style.borderBottomLeftRadius = new Length(top ?? left, unit);
            element.style.borderBottomRightRadius = new Length(bottom ?? top ?? left, unit);
            return element;
        }

        public static T BorderColor<T>(this T element, Color left, Color? top = null, Color? right = null, Color? bottom = null) where T : VisualElement
        {
            element.style.borderLeftColor = left;
            element.style.borderRightColor = right ?? left;
            element.style.borderTopColor = top ?? left;
            element.style.borderBottomColor = bottom ?? top ?? left;
            return element;
        }

        public static T KeepSquare<T>(this T element) where T : VisualElement
        {
            element.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                var minSize = Mathf.Min(evt.newRect.width, evt.newRect.height);
                element.style.maxHeight = minSize;
                element.style.maxWidth = minSize;
            });
            return element;
        }

        public static T RepeatBG<T>(this T element, Repeat horizontal, Repeat? vertical = null) where T : VisualElement
        {
            element.style.backgroundRepeat = new BackgroundRepeat(horizontal, vertical ?? horizontal);
            return element;
        }
        
        public static T BgSize<T>(this T element, float width, float? height = null, LengthUnit unit = LengthUnit.Pixel) where T : VisualElement
        {
            element.style.backgroundSize = new BackgroundSize(new Length(width, unit), new Length(height ?? width, unit));
            return element;
        }
        
        public static T BgSize<T>(this T element, BackgroundSizeType sizeType) where T : VisualElement
        {
            element.style.backgroundSize = new BackgroundSize(sizeType);
            return element;
        }
    }

}