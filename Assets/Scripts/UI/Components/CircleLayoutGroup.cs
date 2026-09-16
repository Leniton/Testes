using System.Collections.Generic;
using UI.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Components
{
    [UxmlElement]
    public partial class CircleLayoutGroup : VisualElement
    {
        private float offset;
        private Length _radius;
        private int endOffset = 0;

        private Rotation rotation = Rotation.Clockwise;
        private float startPadding = 0;
        private float endPadding = 0;

        private bool rotateElements;
        private Rotation elementRotation = Rotation.CounterClockwise;
        private float rotationOffset;

        private float spacing => radius.value;

        #region UXML Declarations
        [Header("Main")]
        [UxmlAttribute, Range(0,1f)] public float Offset
        {
            get => offset;
            set
            {
                offset = value;
                OrganizeElements();
            }
        }
        
        [UxmlAttribute] public Length radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OrganizeElements();
            }
        }

        [UxmlAttribute] public int EndOffset
        {
            get => endOffset;
            set
            {
                endOffset = value;
                OrganizeElements();
            }
        }

        [UxmlAttribute] public Rotation Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                OrganizeElements();
            }
        }

        [UxmlAttribute] public float StartPadding
        {
            get => startPadding;
            set
            {
                startPadding = value;
                OrganizeElements();
            }
        }

        [UxmlAttribute] public float EndPadding
        {
            get => endPadding;
            set
            {
                endPadding = value;
                OrganizeElements();
            }
        }

        [Header("Rotate Elements")]
        [UxmlAttribute] public bool RotateElements
        {
            get => rotateElements;
            set
            {
                rotateElements = value;
                OrganizeElements();
            }
        }

        [UxmlAttribute] public Rotation ElementRotation
        {
            get => elementRotation;
            set
            {
                elementRotation = value;
                OrganizeElements();
            }
        }
        
        [Range(0, 360)]
        [UxmlAttribute] public float RotationOffset
        {
            get => rotationOffset;
            set
            {
                rotationOffset = value;
                OrganizeElements();
            }
        }

        [UxmlCreateInstanceMethod]
        public static CircleLayoutGroup Create()
        {
            CircleLayoutGroup element = new();
            return element;
        }
        #endregion

        public CircleLayoutGroup()
        {
            RegisterCallback<GeometryChangedEvent>(evt => OrganizeElements());
        }

        private void OrganizeElements()
        {
            int extraElements = endOffset;
            float progression = Mathf.Clamp01((1 - endPadding) - startPadding) / (childCount + extraElements);
            float currentP = offset + startPadding;
            int order = (int)rotation;
            int rotationOrder = (int)elementRotation;
            float margin = 0;
            for (int i = 0; i < childCount; i++)
            {
                if (ReferenceEquals(this[i], null)) continue;
                var element = this[i];
                margin = Mathf.Max(margin, element.resolvedStyle.width);
                margin = Mathf.Max(margin, element.resolvedStyle.height);
                element.AbsPos();
                float proportion = currentP % 1;
                float x = Mathf.Sin(2 * Mathf.PI * proportion * order);
                float y = Mathf.Cos(2 * Mathf.PI * proportion * order);
                Vector2 pos = Vector2.one * spacing;
                pos.x += x * spacing;
                pos.y += y * spacing;
                element.Position(pos, LengthUnit.Pixel);

                var angle = rotateElements ? ((360 * rotationOrder) * proportion) + rotationOffset : 0;
                element.Rotation(angle);

                currentP += progression;
            }
            float size = radius.value * 2;
            size += margin;
            var diameter = new StyleLength(new Length(size, radius.unit));
            style.minWidth = diameter;
            style.minHeight = diameter;
        }

        public CircleLayoutGroup SetRadius(float radius, LengthUnit unit = LengthUnit.Pixel)
        {
            _radius = new Length(radius, unit);
            return this;
        }
    }

    public enum Rotation
    {
        Clockwise = 1,
        CounterClockwise = -1
    }
}