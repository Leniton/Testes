
using MessagePack;

[MessagePackObject(true)]
public class PrimitiveOutput
{
    [Key("s")] public string text;
    [Key("i")] public int number;
    [Key("b")] public bool toggle;
    [Key("f")] public float small;
    [Key("d")] public double big;
}