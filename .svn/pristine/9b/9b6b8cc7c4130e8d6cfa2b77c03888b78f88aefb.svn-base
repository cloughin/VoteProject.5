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

namespace Jayrock.Json.Conversion.Converters
{
  #region Imports

  using System;
  using System.Collections.Generic;
  using System.Reflection;

  #endregion

  public class GenericListImporter : ImporterBase
  {
    private readonly Type _itemType;
    private readonly MethodInfo _addMethod;

    public GenericListImporter(Type type)
      : base(type) 
    {
      Type[] types = type.GetGenericArguments();
      _itemType = types[0];
      _addMethod = type.GetMethod("Add", types);
    }

    protected override object ImportFromArray(ImportContext context, JsonReader reader)
    {
      object list = Activator.CreateInstance(OutputType); 

      reader.ReadToken(JsonTokenClass.Array);

      while (reader.TokenClass != JsonTokenClass.EndArray)
        _addMethod.Invoke(list, new Object[] { JsonConvert.Import(_itemType, reader) });

      reader.Read();

      return list;
    }
  }
}
