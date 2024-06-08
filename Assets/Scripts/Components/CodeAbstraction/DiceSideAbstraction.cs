using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lenix.NumberUtilities;
using Unity.VisualScripting;
using System.Text;

public class DiceSideAbstraction : MonoAbstraction
{
    [SerializeField] private StateButton choseSides;
    [SerializeField] private StateButton left, mid, top, bot, right, rightmost;
    [Space]
    [Header("Combination buttons"),SerializeField] private StateButton right2;
    [SerializeField] private StateButton topbot, row, col, all;

    private DiceSides diceSides;

    private void Awake()
    {
        left.onStateChange.AddListener((b) => diceSides ^= DiceSides.left);
        mid.onStateChange.AddListener((b) => diceSides ^= DiceSides.mid);
        top.onStateChange.AddListener((b) => diceSides ^= DiceSides.top);
        bot.onStateChange.AddListener((b) => diceSides ^= DiceSides.bot);
        right.onStateChange.AddListener((b) => diceSides ^= DiceSides.right);
        rightmost.onStateChange.AddListener((b) => diceSides ^= DiceSides.rightmost);

        right2.onStateChange.AddListener((b)=> { if (b) diceSides |= DiceSides.right2; else diceSides &= ~DiceSides.right2; });
        topbot.onStateChange.AddListener((b)=> { if (b) diceSides |= DiceSides.topbot; else diceSides &= ~DiceSides.topbot; });
        row.onStateChange.AddListener((b)=> { if (b) diceSides |= DiceSides.row; else diceSides &= ~DiceSides.row; });
        col.onStateChange.AddListener((b)=> { if (b) diceSides |= DiceSides.col; else diceSides &= ~DiceSides.col; });
        all.onStateChange.AddListener((b)=> { if (b) diceSides |= DiceSides.all; else diceSides &= ~DiceSides.all; });

        left.onStateChange.AddListener(_ => UpdateButtons());
        mid.onStateChange.AddListener(_ => UpdateButtons());
        top.onStateChange.AddListener(_ => UpdateButtons());
        bot.onStateChange.AddListener(_ => UpdateButtons());
        right.onStateChange.AddListener(_ => UpdateButtons());
        rightmost.onStateChange.AddListener(_ => UpdateButtons());
        right2.onStateChange.AddListener(_ => UpdateButtons());
        topbot.onStateChange.AddListener(_ => UpdateButtons());
        row.onStateChange.AddListener(_ => UpdateButtons());
        col.onStateChange.AddListener(_ => UpdateButtons());
        all.onStateChange.AddListener(_ => UpdateButtons());
    }

    private void UpdateButtons()
    {
        left.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.left));
        mid.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.mid));
        top.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.top));
        bot.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.bot));
        right.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.right));
        rightmost.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.rightmost));

        right2.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.right2));
        topbot.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.topbot));
        row.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.row));
        col.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.col));
        all.SetState(NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.all));

        //basic sides
        left.GetComponent<DualState>().SetState(left.State);
        mid.GetComponent<DualState>().SetState(mid.State);
        top.GetComponent<DualState>().SetState(top.State);
        bot.GetComponent<DualState>().SetState(bot.State);
        right.GetComponent<DualState>().SetState(right.State);
        rightmost.GetComponent<DualState>().SetState(rightmost.State);

        //combination sides
        right2.GetComponent<DualState>().SetState(right2.State);
        topbot.GetComponent<DualState>().SetState(topbot.State);
        row.GetComponent<DualState>().SetState(row.State);
        col.GetComponent<DualState>().SetState(col.State);
        all.GetComponent<DualState>().SetState(all.State);
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb)
    {
        //check if is composite
        List<DiceSides> sides = new();
        if(diceSides != DiceSides.all)
        {
            if (NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.topbot))
            {
                if (NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.col))
                    sides.Add(DiceSides.col);
                else 
                    sides.Add(DiceSides.topbot);
            }
            else AddSingleSides(sides, diceSides & ~DiceSides.row);

            DiceSides ignoreSide = sides.Contains(DiceSides.col) ? DiceSides.col : DiceSides.topbot;
            if (NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.right2))
            {
                if (NumberUtil.ContainsBytes((int)diceSides, (int)DiceSides.row))
                    sides.Add(DiceSides.row);
                else
                {
                    sides.Add(DiceSides.right2);
                    if (diceSides != DiceSides.right2)
                    {
                        ignoreSide |= DiceSides.right2;
                        AddSingleSides(sides, diceSides & ~ignoreSide);
                    }
                }
            }
            else AddSingleSides(sides, diceSides & ~ignoreSide);
        }
        else sides.Add(diceSides);

        //placing logic
        sb = sb ?? new();
        if (sides.Count > 1) sb.Append("(");
        for (int i = 0; i < sides.Count; i++)
        {
            if(i > 0) sb.Append("#");
            sb.Append(sides[i]);
            for (int u = 0; u < subAbstractions.Count; u++)
            {
                sb.Append($".");
                subAbstractions[u].GetCode(sb);
            }
        }
        if (sides.Count > 1) sb.Append(")");

        return sb.ToString();
    }

    private void AddSingleSides(List<DiceSides> sides, DiceSides dice)
    {
        int[] singleSides = NumberUtil.SeparateBits((int)dice);
        for (int i = 0; i < singleSides.Length; i++)
        {
            sides.Add((DiceSides)singleSides[i]);
        }
    }
}

[System.Flags]
public enum DiceSides
{
    left = 1,
    mid = 2,
    top = 4,
    bot = 8,
    right = 16,
    rightmost = 32,
    right2 = 0b110000,
    row = 0b110011,
    topbot = 0b001100,
    col = 0b001110,
    all = 0b111111
}