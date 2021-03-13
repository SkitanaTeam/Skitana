// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.IO.Abstractions;
using System.Collections.Generic;
using System.IO;

namespace Skitana.IO
{
    public sealed class AggregatedFilesRepository : IFilesRepository
    {
        public IFilesRepository DefaultSource { get; set; }

        private readonly Dictionary<string, IFilesRepository> fileSources = new Dictionary<string, IFilesRepository>();

        public Stream Open(string path)
        {
            var parts = path.Split(':');

            if (parts.Length == 1)
            {
                return DefaultSource.Open(path);
            }

            if (!fileSources.TryGetValue(parts[0], out var source)) throw new FileNotFoundException();

            parts[0] = "";
            path = string.Join(":", parts).Trim(':');
            return source.Open(path.Trim('\\', '/'));
        }

        public void RegisterSource(string name, IFilesRepository source)
        {
            fileSources.Add(name, source);
        }
    }
}
