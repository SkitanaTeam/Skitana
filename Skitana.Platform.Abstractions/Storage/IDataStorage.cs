// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System.IO;

namespace Skitana.Platform.Abstractions.Storage
{
    public interface IDataStorage
    {
        Stream OpenRead(string name);
        Stream OpenWrite(string name);
    }
}
