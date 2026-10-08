using System;
using System.Collections.Generic;
using GridSystem;
using UI.Utils;
using UI.Utils.Builder;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameData.Words
{
    public abstract class Word
    {
        protected List<IPiece> pieces = new();
        
        public abstract IPiece GetPiece();
        
        public void ForEachPiece(Action<IPiece> action)
        {
            foreach (var piece in pieces)
                action(piece);
        }

        public static Label GetWordLabel(string word, Color? textColor = null,  Color? backgroundColor = null)
        {
            var colorText = textColor ?? Color.white;
            var bgColor = backgroundColor ?? new();
            var label = new Label(word);
            label
                .BgColor(bgColor)
                .AbsPos()
                .Font(Resources.Load<Font>("LowresPixel-Regular"))
                .FontSize(60)
                .LetterSpacing(10)
                .WrapText(WhiteSpace.Normal)
                .TextAlign(TextAnchor.MiddleCenter)
                .Color(colorText)
                .Size(100);
            return label;
        }
    }
}