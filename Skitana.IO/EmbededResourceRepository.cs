// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.IO.Abstractions;
using System.IO;
using System.Reflection;

namespace Skitana.IO
{
    public sealed class EmbededResourceRepository : IFilesRepository
    {
        private readonly Assembly assembly;
        private readonly string workingPath;

        public EmbededResourceRepository(Assembly assembly, string workingPath)
        {
            this.assembly = assembly;
            this.workingPath = workingPath;
        }

        public Stream Open(string path)
        {
            path = workingPath + '.' + path.Replace("\\", "/").Replace("/", ".");
            path = path.Replace("..", ".");

            var stream = assembly.GetManifestResourceStream(path);
            if (stream == null) throw new FileNotFoundException();
            return stream;
        }
    }
}
