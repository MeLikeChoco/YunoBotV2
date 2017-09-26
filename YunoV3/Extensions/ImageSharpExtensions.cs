using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Extensions
{
    public static class ImageSharpExtensions
    {

        public static void Resize(this Image<Rgba32> image, int width, int height)
            => image.Mutate(processor => processor.Resize(width, height));
        
    }
}
