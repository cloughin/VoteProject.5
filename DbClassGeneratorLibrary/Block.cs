using System;

namespace GenerateDbClasses
{
  class Block : IDisposable
  {
    private readonly Indent _Indent;
    private readonly Options _Options;

    public Block() : this(Options.None)
    {
    }

    public Block(Options options)
    {
      Writer.OpenBlock();
      _Indent = new Indent();
      _Options = options;
    }

    public void Dispose()
    {
      _Indent.Dispose();
      Writer.CloseBlock();
      if ((_Options & Options.SpacerAfter) != Options.None)
        Writer.Spacer();
    }

    [Flags]
    public enum Options
    {
      None = 0,
      SpacerAfter = 1
    }
  }
}
