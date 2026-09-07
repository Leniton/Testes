using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Utils.Builder
{
    public static class TextBuilder
    {
        public static T TextAlign<T>(this T element, TextAnchor align) where T: TextElement
        {
            element.style.unityTextAlign = align;
            return element;
        }
        public static UIBuilder<T> TextAlign<T>(this UIBuilder<T> builder, TextAnchor align) where T: TextElement
        {
            builder.element.TextAlign(align);
            return builder;
        }

        public static T FontSize<T>(this T element, float size) where T: TextElement
        {
            element.style.fontSize = size;
            return element;
        }
        public static UIBuilder<T> FontSize<T>(this UIBuilder<T> builder, float size) where T: TextElement
        {
            builder.element.FontSize(size);
            return builder;
        }

        public static T Text<T>(this T element, string text) where T : TextElement
        {
            element.text = text;
            return element;
        }
        public static UIBuilder<T> Text<T>(this UIBuilder<T> builder, string text) where T : TextElement
        {
            builder.element.Text(text);
            return builder;
        }

        public static T TextOutline<T>(this T element, float width) where T : TextElement
        {
            element.style.unityTextOutlineWidth = width;
            return element;
        }
        
        public static T TextOutlineColor<T>(this T element, Color color) where T : TextElement
        {
            element.style.unityTextOutlineColor = color;
            return element;
        }
        
        public static T Overflow<T>(this T element, TextOverflow overflow) where T : TextElement
        {
            element.style.textOverflow = overflow;
            return element;
        }

        public static T WrapText<T>(this T element, WhiteSpace wrap) where T : TextElement
        {
            element.style.whiteSpace = wrap;
            return element;
        }
    }
}