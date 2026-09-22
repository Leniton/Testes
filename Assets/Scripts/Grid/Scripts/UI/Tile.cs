using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GridSystem.UI
{
    public class Tile : MonoBehaviour, ITile, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public List<IPiece> pieces { get; set; } = new();

        public int pieceID
        {
            get
            {
                if (pieces.Count <= 0) return (int)PieceType.none;
                int value = (int)PieceType.generic;
                for (int i = 0; i < pieces.Count; i++)
                {
                    value |= pieces[i].id;
                }

                return value;
            }
        }

        private Image img;
        public ITile.State state { get; set; }

        [SerializeField] private Color DefaultColor, SelectableColor, ValidColor, InvalidColor;
        public Color defaultColor => DefaultColor;

        public Color selectableColor => SelectableColor;

        public Color validColor => ValidColor;

        public Color invalidColor => InvalidColor;
        public List<Color> colors { get; set; } = new();

        //callbacks
        public Action<ITile> onClick { get; set; }
        public Action<ITile> onEnter { get; set; }
        public Action<ITile> onExit { get; set; }

        private GraphicRaycaster raycaster;
        private bool left = true;

        private void Awake()
        {
            img = GetComponent<Image>();
            img.color = DefaultColor;
            raycaster = GetComponentInParent<GraphicRaycaster>();
        }

        public void ApplyColor(Color color)
        {
            img.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            for (int i = 0; i < pieces.Count; i++) pieces[i].onClick?.Invoke();
            onClick?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Contains(eventData.position - eventData.delta) && !left) return;
            left = false;

            for (int i = 0; i < pieces.Count; i++) pieces[i].onEnter?.Invoke();
            onEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Contains(eventData.position)) return;
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
            Color newColor = Color.white;
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

        private bool Contains(Vector2 mousePosition)
        {
            PointerEventData eventData = new(EventSystem.current) { position = mousePosition };
            List<RaycastResult> results = new();
            raycaster.Raycast(eventData, results);

            for (int i = 0; i < results.Count; i++)
                if (results[i].gameObject == img.gameObject)
                    return true;

            return false;
        }
    
        public void SetState(ITile.State newState) { }
        public void Select(int filter) { }
        public void Deselect() { }
    }
}