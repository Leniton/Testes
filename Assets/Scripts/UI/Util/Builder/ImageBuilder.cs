using AddressableAsyncInstances;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Utils.Builder
{
    public static class ImageBuilder
    {
        public static T Sprite<T>(this T image, Sprite sprite) where T : Image
        {
            image.sprite = sprite;
            return image;
        }
        public static T BgImage<T>(this T image, Sprite sprite) where T : VisualElement
        {
            image.style.backgroundImage = new (sprite);
            return image;
        }
        public static T Tint<T>(this T image, Color color) where T : Image
        {
            image.tintColor = color;
            return image;
        }
        
        public static T Slice<T>(this T image, int top = 1, int? left= null, int? bot = null, int? right = null) where T : VisualElement
        {
           image.style.unitySliceTop = top;
           image.style.unitySliceLeft = left ?? top;
           image.style.unitySliceBottom = bot ?? top;
           image.style.unitySliceRight = right ?? left ?? top;
            return image;
        }
        public static T SliceScale<T>(this T image, float scale) where T : VisualElement
        {
            image.style.unitySliceScale = scale;
            return image;
        }
        
        public static UIBuilder<T> Sprite<T>(this UIBuilder<T> builder, Sprite sprite) where T : Image
        {
            builder.element.Sprite(sprite);
            return builder;
        }

        public static T AddressableLoadSprite<T>(this T image, string path) where T : Image
        {
            AAAsset<Sprite>.LoadAsset(path, sprite => image.sprite = sprite);
            return image;
        }
        
        public static T AddressableLoadBgImage<T>(this T image, string path) where T : VisualElement
        {
            AAAsset<Sprite>.LoadAsset(path, sprite => image.BgImage(sprite));
            return image;
        }
    }
}