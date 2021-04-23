#region License, Terms and Conditions

//
// Jayrock - JSON and JSON-RPC for Microsoft .NET Framework and Mono
// Written by Atif Aziz (atif.aziz@skybow.com)
// Copyright (c) 2005 Atif Aziz. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 2.1 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//

#endregion

using System.Globalization;
using System.IO;
using System.Text;

namespace Jayrock
{
  #region Imports

  

  #endregion

  /// <summary>
  /// Drop-in replacement for <see cref="System.CodeDom.Compiler.IndentedTextWriter"/>
  /// that does not require a full-trust link and inheritance demand.
  /// </summary>
  public sealed class IndentedTextWriter : TextWriter
  {
    private readonly TextWriter _Writer;
    private int _Level;
    private bool _TabsPending;
    private readonly string _Tab;

    private const string DefaultTabString = "\x20\x20\x20\x20";

    public IndentedTextWriter(TextWriter writer, string tabString = DefaultTabString)
      : base(CultureInfo.InvariantCulture)
    {
      _Writer = writer;
      _Tab = tabString;
      _Level = 0;
      _TabsPending = false;
    }

    public override Encoding Encoding { get { return _Writer.Encoding; } }

    public override string NewLine { get { return _Writer.NewLine; } set { _Writer.NewLine = value; } }

    public int Indent { get { return _Level; } set { _Level = value < 0 ? 0 : value; } }

    public TextWriter InnerWriter { get { return _Writer; } }

    internal string TabString { get { return _Tab; } }

    public override void Close()
    {
      _Writer.Close();
    }

    public override void Flush()
    {
      _Writer.Flush();
    }

    //public override void Write(string s)
    //{
    //  WritePendingTabs();
    //  _Writer.Write(s);
    //}

    public override void Write(bool value)
    {
      WritePendingTabs();
      _Writer.Write(value);
    }

    public override void Write(char value)
    {
      WritePendingTabs();
      _Writer.Write(value);
    }

    public override void Write(char[] buffer)
    {
      WritePendingTabs();
      _Writer.Write(buffer);
    }

    public override void Write(char[] buffer, int index, int count)
    {
      WritePendingTabs();
      _Writer.Write(buffer, index, count);
    }

    public override void Write(double value)
    {
      WritePendingTabs();
      _Writer.Write(value);
    }

    public override void Write(float value)
    {
      WritePendingTabs();
      _Writer.Write(value);
    }

    public override void Write(int value)
    {
      WritePendingTabs();
      _Writer.Write(value);
    }

    public override void Write(long value)
    {
      WritePendingTabs();
      _Writer.Write(value);
    }

    public override void Write(object value)
    {
      WritePendingTabs();
      _Writer.Write(value);
    }

    //public override void Write(string format, object arg0)
    //{
    //  WritePendingTabs();
    //  _Writer.Write(format, arg0);
    //}

    //public override void Write(string format, object arg0, object arg1)
    //{
    //  WritePendingTabs();
    //  _Writer.Write(format, arg0, arg1);
    //}

    public override void Write(string format, params object[] arg)
    {
      WritePendingTabs();
      _Writer.Write(format, arg);
    }

    public void WriteLineNoTabs(string s)
    {
      _Writer.WriteLine(s);
    }

    //public override void WriteLine(string s)
    //{
    //  WritePendingTabs();
    //  _Writer.WriteLine(s);
    //  _TabsPending = true;
    //}

    public override void WriteLine()
    {
      WritePendingTabs();
      _Writer.WriteLine();
      _TabsPending = true;
    }

    public override void WriteLine(bool value)
    {
      WritePendingTabs();
      _Writer.WriteLine(value);
      _TabsPending = true;
    }

    public override void WriteLine(char value)
    {
      WritePendingTabs();
      _Writer.WriteLine(value);
      _TabsPending = true;
    }

    public override void WriteLine(char[] buffer)
    {
      WritePendingTabs();
      _Writer.WriteLine(buffer);
      _TabsPending = true;
    }

    public override void WriteLine(char[] buffer, int index, int count)
    {
      WritePendingTabs();
      _Writer.WriteLine(buffer, index, count);
      _TabsPending = true;
    }

    public override void WriteLine(double value)
    {
      WritePendingTabs();
      _Writer.WriteLine(value);
      _TabsPending = true;
    }

    public override void WriteLine(float value)
    {
      WritePendingTabs();
      _Writer.WriteLine(value);
      _TabsPending = true;
    }

    public override void WriteLine(int value)
    {
      WritePendingTabs();
      _Writer.WriteLine(value);
      _TabsPending = true;
    }

    public override void WriteLine(long value)
    {
      WritePendingTabs();
      _Writer.WriteLine(value);
      _TabsPending = true;
    }

    public override void WriteLine(object value)
    {
      WritePendingTabs();
      _Writer.WriteLine(value);
      _TabsPending = true;
    }

    //public override void WriteLine(string format, object arg0)
    //{
    //  WritePendingTabs();
    //  _Writer.WriteLine(format, arg0);
    //  _TabsPending = true;
    //}

    //public override void WriteLine(string format, object arg0, object arg1)
    //{
    //  WritePendingTabs();
    //  _Writer.WriteLine(format, arg0, arg1);
    //  _TabsPending = true;
    //}

    public override void WriteLine(string format, params object[] arg)
    {
      WritePendingTabs();
      _Writer.WriteLine(format, arg);
      _TabsPending = true;
    }

    private void WritePendingTabs()
    {
      if (!_TabsPending)
        return;

      _TabsPending = false;

      for (var i = 0; i < _Level; i++)
        _Writer.Write(_Tab);
    }
  }
}