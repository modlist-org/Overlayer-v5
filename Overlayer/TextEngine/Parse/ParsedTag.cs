namespace Overlayer.TextEngine.Parse;

public readonly struct ParsedTag(string raw, string name, string[] args, int index, int length) {
    public readonly string Raw = raw;
    public readonly string Name = name;
    public readonly string[] Args = args;
    public readonly int Index = index;
    public readonly int Length = length;
}