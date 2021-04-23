using System;
using System.Runtime.Serialization;
using static System.String;

namespace Vote
{
  // For internal exceptions
  //
  [Serializable]
  public class VoteException : Exception
  {
    public VoteException() {}

    public VoteException(string message) : base(message) {}

    public VoteException(string message, params object[] args)
      : base(Format(message, args)) {}

    protected VoteException(string message, Exception innerException)
      : base(message, innerException) {}

    protected VoteException(
      SerializationInfo serializationInfo, StreamingContext streamingContent)
      : base(serializationInfo, streamingContent) {}
  }

  // For routine UI exceptions that are not normally logged
  [Serializable]
  public class VoteUIException : VoteException
  {
    public VoteUIException() {}

    public VoteUIException(string message) : base(message) {}

    public VoteUIException(string message, params object[] args)
      : base(Format(message, args)) {}

    public VoteUIException(string message, Exception innerException)
      : base(message, innerException) {}

    protected VoteUIException(
      SerializationInfo serializationInfo, StreamingContext streamingContent)
      : base(serializationInfo, streamingContent) {}
  }
}