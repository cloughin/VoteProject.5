using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace VoteDb2
{
  public abstract class DataReaderBase : IDataReader, IDisposable, IDataRecord
  {
    // A wrapper foro a DbDataReader that provides strong typing
    DbDataReader _DataReader;
    DbConnection _Connection;

    public DataReaderBase(DbDataReader dataReader, DbConnection connection) { _DataReader = dataReader; _Connection = connection; }

    public DbDataReader DataReader { get { return _DataReader; } }

    public int Depth { get { return _DataReader.Depth; } }

    public int FieldCount { get { return _DataReader.FieldCount; } }

    public bool HasRows { get { return _DataReader.HasRows; } }

    public bool IsClosed { get { return _DataReader.IsClosed; } }

    public int RecordsAffected { get { return _DataReader.RecordsAffected; } }

    public virtual int VisibleFieldCount { get { return _DataReader.VisibleFieldCount; } }

    public object this[int ordinal] { get { return _DataReader[ordinal]; } }

    public object this[string name] { get { return _DataReader[name]; } }

    public void Close() { _DataReader.Close(); }

    public void Dispose() { _DataReader.Dispose(); if (_Connection != null) _Connection.Dispose(); }

    public bool GetBoolean(int ordinal) { return _DataReader.GetBoolean(ordinal); }

    public byte GetByte(int ordinal) { return _DataReader.GetByte(ordinal); }

    public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) { return _DataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length); }

    public char GetChar(int ordinal) { return _DataReader.GetChar(ordinal); }

    public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) { return _DataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length); }

    IDataReader IDataRecord.GetData(int ordinal) { return _DataReader.GetData(ordinal); }

    public string GetDataTypeName(int ordinal) { return _DataReader.GetDataTypeName(ordinal); }

    public DateTime GetDateTime(int ordinal) { return _DataReader.GetDateTime(ordinal); }

    public decimal GetDecimal(int ordinal) { return _DataReader.GetDecimal(ordinal); }

    public double GetDouble(int ordinal) { return _DataReader.GetDouble(ordinal); }

    public Type GetFieldType(int ordinal) { return _DataReader.GetFieldType(ordinal); }

    public float GetFloat(int ordinal) { return _DataReader.GetFloat(ordinal); }

    public Guid GetGuid(int ordinal) { return _DataReader.GetGuid(ordinal); }

    public short GetInt16(int ordinal) { return _DataReader.GetInt16(ordinal); }

    public int GetInt32(int ordinal) { return _DataReader.GetInt32(ordinal); }

    public long GetInt64(int ordinal) { return _DataReader.GetInt64(ordinal); }

    public string GetName(int ordinal) { return _DataReader.GetName(ordinal); }

    public int GetOrdinal(string name) { return _DataReader.GetOrdinal(name); }

    public virtual Type GetProviderSpecificFieldType(int ordinal) { return _DataReader.GetProviderSpecificFieldType(ordinal); }

    public virtual object GetProviderSpecificValue(int ordinal) { return _DataReader.GetProviderSpecificValue(ordinal); }

    public virtual int GetProviderSpecificValues(object[] values) { return _DataReader.GetProviderSpecificValues(values); }

    public DataTable GetSchemaTable() { return _DataReader.GetSchemaTable(); }

    public string GetString(int ordinal) { return _DataReader.GetString(ordinal); }

    public object GetValue(int ordinal) { return _DataReader.GetValue(ordinal); }

    public int GetValues(object[] values) { return _DataReader.GetValues(values); }

    public bool IsDBNull(int ordinal) { return _DataReader.IsDBNull(ordinal); }

    public bool NextResult() { return _DataReader.NextResult(); }

    public bool Read() { return _DataReader.Read(); }
  }
}
