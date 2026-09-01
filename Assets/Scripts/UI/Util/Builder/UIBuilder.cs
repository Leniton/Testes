using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Utils.Builder
{
    public class UIBuilder<T> where T : VisualElement
    {
        public T element { get; private set; }
        private LengthUnit defaultUnit;
        
        public UIBuilder(T element, LengthUnit defaultUnit = LengthUnit.Pixel)
        {
            this.element = element;
            this.defaultUnit = defaultUnit;
            element.Grow(0);
            element.Shrink(0);
        }

        public T Build() => element;

        public UIBuilder<T> Name(string name)
        {
            element.Name(name);
            return this;
        }
        
        public UIBuilder<T> AbsPos()
        {
            element.AbsPos();
            return this;
        }
        public UIBuilder<T> Position(Vector2 position)
        {
            element.Position(position);
            return this;
        }
        public UIBuilder<T> Rotation(float angle)
        {
            element.Rotation(angle);
            return this;
        }
        
        public UIBuilder<T> Align(Align align)
        {
            element.Align(align);
            return this;
        }
        
        public UIBuilder<T> JustifyContent(Justify justify)
        {
            element.JustifyContent(justify);
            return this;
        }

        public UIBuilder<T> MinSize(Vector2 size, LengthUnit? unit = null)
        {
            element.MinSize(size, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> MinSize(float width, float? height = null, LengthUnit? unit = null)
        {
            element.MinSize(width, height, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> MaxSize(Vector2 size, LengthUnit? unit = null)
        {
            element.MaxSize(size, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> MaxSize(float width, float? height = null, LengthUnit? unit = null)
        {
            element.MaxSize(width, height, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> Size(Vector2 size, LengthUnit? unit = null)
        {
            element.Size(size, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> Size(float width, float? height = null, LengthUnit? unit = null)
        {
            element.Size(width, height, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> Width(float width, LengthUnit? unit = null)
        {
            element.Width(width, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> Height(float height, LengthUnit? unit = null)
        {
            element.Height(height, unit ?? defaultUnit);
            return this;
        }

        public UIBuilder<T> BgColor(Color color)
        {
            element.BgColor(color);
            return this;
        }
        public UIBuilder<T> Color(Color color)
        {
            element.Color(color);
            return this;
        }
        
        public UIBuilder<T> FlexDirection(FlexDirection direction)
        {
            element.FlexDirection(direction);
            return this;
        }
        public UIBuilder<T> Grow(float grow = 1)
        {
            element.Grow(grow);
            return this;
        }
        public UIBuilder<T> Shrink(float shrink = 1)
        {
            element.Shrink(shrink);
            return this;
        }

        public UIBuilder<T> Padding(float left, float? top = null, float? right = null, float? bottom = null, LengthUnit? unit = null)
        {
            element.Padding(left, top, right, bottom, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> Margin(float left, float? top = null, float? right = null, float? bottom = null, LengthUnit? unit = null)
        {
            element.Margin(left, top, right, bottom, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> BorderSize(float left, float? top = null, float? right = null, float? bottom = null)
        {
            element.BorderSize(left, top, right, bottom);
            return this;
        }
        public UIBuilder<T> BorderRadius(float left, float? top = null, float? right = null, float? bottom = null, LengthUnit? unit = null)
        {
            element.BorderRadius(left, top, right, bottom, unit ?? defaultUnit);
            return this;
        }
        public UIBuilder<T> BorderColor(Color left, Color? top = null, Color? right = null, Color? bottom = null)
        {
            element.BorderColor(left, top, right, bottom);
            return this;
        }
    }
}