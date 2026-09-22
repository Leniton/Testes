using System;
using System.Collections.Generic;
using UI.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace GridSystem.VisualElements
{
    public class Tile : Button, ITile
    {
        public List<IPiece> pieces { get; set; } = new();

        public Color defaultColor { get; } = ColorExtension.GrayShade(.8f);

        public Color selectableColor { get; } = ColorExtension.GrayShade(.4f);

        public Color validColor { get; } = Color.green;

        public Color invalidColor { get; } = Color.red;
        public List<Color> colors { get; set; } = new();

        public ITile.State state { get; set; }
        public Action<ITile> onClick { get; set; }
        public Action<ITile> onEnter { get; set; }
        public Action<ITile> onExit { get; set; }
        
        private Color? selectedColor;

        private bool left = true;

        public Tile()
        {
            this.Padding(0);
            style.backgroundColor = defaultColor;
            RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            RegisterCallback<PointerLeaveEvent>(OnPointerExit);
            clicked += OnPointerClick;
        }

        public void ApplyColor(Color color)
        {
            style.backgroundColor = color;
        }

        private void OnPointerClick()
        {
            for (int i = 0; i < pieces.Count; i++) pieces[i].onClick?.Invoke();
            onClick?.Invoke(this);
        }

        private void OnPointerEnter(PointerEnterEvent eventData)
        {
            if (worldBound.Contains(eventData.position - eventData.deltaPosition) && !left) return;
            left = false;

            for (int i = 0; i < pieces.Count; i++) pieces[i].onEnter?.Invoke();
            onEnter?.Invoke(this);
        }

        private void OnPointerExit(PointerLeaveEvent eventData)
        {
            if (worldBound.Contains(eventData.position)) return;
            left = true;

            for (int i = 0; i < pieces.Count; i++) pieces[i].onExit?.Invoke();
            onExit?.Invoke(this);
        }

        public void SetColor(Color color)
        {
            colors.Clear();
            AddColor(color);
        }

        public void AddColor(Color color)
        {
            Color invertedColor = ColorExtension.InvertColor(color);
            invertedColor.a = 0;

            colors.Add(invertedColor);
            UpdateColor();
        }

        public void UpdateColor()
        {
            Color newColor = colors.Count > 0 ? Color.white : defaultColor;
            for (int i = 0; i < colors.Count; i++)
            {
                newColor -= colors[i] * (.25f + (1f / (colors.Count + 1f)));
            }

            ApplyColor(newColor);
        }

        public void RemoveColor(Color color)
        {
            Color invertedColor = ColorExtension.InvertColor(color);
            invertedColor.a = 0;

            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i] == invertedColor)
                {
                    colors.RemoveAt(i);
                    UpdateColor();
                    break;
                }
            }
        }
        
        private Color StateColor() => state switch
        {
            ITile.State.selectable => selectableColor,
            ITile.State.invalid => invalidColor,
            _ => defaultColor,
        };

        public void SetState(ITile.State newState)
        {
            RemoveColor(StateColor());
            state = newState;
            AddColor(StateColor());
        }
        
        public void Select(int filter)
        {
            ITile tile = this;
            selectedColor = IGrid.IsInFilter(tile.pieceID, filter) ? validColor : invalidColor;
            AddColor(selectedColor.Value);
        }
        public void Deselect()
        {
            if (!selectedColor.HasValue) return;
            RemoveColor(selectedColor.Value);
        }
    }
}