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

        public void OnPointerClick()
        {
            for (int i = 0; i < pieces.Count; i++) pieces[i].onClick?.Invoke();
            onClick?.Invoke(this);
        }

        public void OnPointerEnter(PointerEnterEvent eventData)
        {
            if (worldBound.Contains(eventData.position - eventData.deltaPosition) && !left) return;
            left = false;

            for (int i = 0; i < pieces.Count; i++) pieces[i].onEnter?.Invoke();
            onEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerLeaveEvent eventData)
        {
            if (worldBound.Contains(eventData.position)) return;
            left = true;

            for (int i = 0; i < pieces.Count; i++) pieces[i].onExit?.Invoke();
            onExit?.Invoke(this);
        }
    }
}