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
    public class Is : TextPiece
    {
        protected override void Awake()
        {
            base.Awake();
            ChangeText("is");
        }
    }
}