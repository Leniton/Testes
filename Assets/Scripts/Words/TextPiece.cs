using System;
using System.Collections.Generic;
using GameData.UI;
using GridSystem;
using UI.Utils;
using UI.Utils.Builder;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameData.Words
{
    public class TextPiece : MonoBehaviour, IPiece
    {
        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<ITrait> traits { get; set; } = new();
        public Action<ITile, ITile> onTileChanged { get; set; }

        protected Label label;
        
        protected virtual void Awake()
        {
            IPiece piece = this;
            piece.AddTrait(new GhostTrait());
            
            label ??= Word.GetWordLabel("__");
            UiController.instance.root.Add(label);
            UiController.instance.root
                .AlignItems(Align.Center)
                .JustifyContent(Justify.Center);
            PositionUi();
        }

        public virtual void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            transform.localPosition = newCoordinates;
            PositionUi();
        }

        public virtual void ChangeText(string text, Color? textColor = null,  Color? backgroundColor = null)
        {
            if (label == null)
            {
                label = Word.GetWordLabel(text, textColor, backgroundColor);
                return;
            }
            var colorText = textColor ?? Color.white;
            var bgColor = backgroundColor ?? new();
            label
                .BgColor(bgColor)
                .Text($"<line-height=70%>{text}")
                .Color(colorText);
        }
        
        protected virtual void PositionUi()
        {
            float tileSize = 108f;
            float x = coordinate.x * tileSize;
            float y = -(coordinate.y * tileSize);
            label?.Position(new Vector2(x, y), LengthUnit.Pixel);
        }
    }
}