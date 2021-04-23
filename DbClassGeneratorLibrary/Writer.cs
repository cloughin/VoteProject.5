using System.IO;
using static System.String;

namespace GenerateDbClasses
{
  static class Writer
  {
    private static TextWriter _TextWriter;
    private static bool _SpacerPending;

    public static TextWriter TextWriter
    {
      set
      {
        _TextWriter = value;
      }
    }

    public static void CloseBlock()
    {
      _SpacerPending = false;
      Write("}}");
    }

    public static void OpenBlock()
    {
      Write("{{");
    }

    public static void Write()
    {
      _TextWriter.WriteLine();
    }

    public static void Write(string format, params object[] args)
    {
      if (_SpacerPending)
      {
        Write();
        _SpacerPending = false;
      }
      var text = Format(format, args).Split('\n');
      foreach (var line in text)
      {
        DoIndent();
        _TextWriter.WriteLine(line);
      }
    }

    public static void WriteUnformatted(string line)
    {
      if (_SpacerPending)
      {
        Write();
        _SpacerPending = false;
      }
      DoIndent();
      _TextWriter.WriteLine(line);
    }

    private static void DoIndent()
    {
      for (var n = 0; n < Indent.CurrentIndent; n++)
        _TextWriter.Write(" ");
    }

    public static void Spacer()
    {
      _SpacerPending = true;
    }
  }
}
