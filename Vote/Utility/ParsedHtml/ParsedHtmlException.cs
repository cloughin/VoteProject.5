using System;
using System.Runtime.Serialization;

namespace Vote
{
  [Serializable]
  public class ParsedHtmlException : Exception
  {
    #region Protected

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    protected ParsedHtmlException(
      SerializationInfo serializationInfo, StreamingContext streamingContent)
      : base(serializationInfo, streamingContent)
    {
    }


    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Protected

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public ParsedHtmlException()
    {
    }

    public ParsedHtmlException(string message) : base(message)
    {
    }

    public ParsedHtmlException(string message, Exception innerException)
      : base(message, innerException)
    {
    }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}