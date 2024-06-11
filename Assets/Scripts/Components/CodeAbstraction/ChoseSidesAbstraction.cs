using System.Text;
using UnityEngine;

public class ChoseSidesAbstraction : MonoAbstraction
{
    [SerializeField] private StateButton button;
    [SerializeField] private PickSideWindow[] options;

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb)
    {
        sb ??= new StringBuilder();
        if (!button.State) return sb.ToString();
        sb.Append("sd.");
        for (int i = 0; i < 6; i++)
        {
            SideData side = options[i].sideData;
            sb.Append(side.Id);
            if (side.pips >= 0) sb.Append($"-{side.pips}");
            if (i < 5) sb.Append(':');
        }
        Debug.Log(sb.ToString());
        return sb.ToString();
    }
}
