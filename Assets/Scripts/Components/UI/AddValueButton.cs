using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddValueButton : ValueChangeButton
{
    public override void ChangeValue()
    {
        inputField.Value += Value;
    }
}
