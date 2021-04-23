using System;

namespace GenerateDbClasses
{
  class Indent : IDisposable
  {
    private readonly int _Spaces;

      public Indent(int spaces)
      {
        _Spaces = spaces;
        CurrentIndent += spaces;
      }

      public Indent()
        : this(2)
      {
      }

      public static int CurrentIndent { get; private set; }

    public void Dispose()
      {
        CurrentIndent -= _Spaces;
      }
  }
}
