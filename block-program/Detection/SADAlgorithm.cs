using Myxini.Recognition.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class SADAlgorithm : IPatternMatchingAlgorithm
    {
        public Pattern Match(IEnumerable<Pattern> patterns, IImage raw_block)
        {
            // ここでパターンマッチングして最もマッチするパターンに
            // 紐付けしたIBlockを返す
            return patterns
                    .OrderBy(pattern => Math.Abs(pattern.Match(raw_block, this)))
                    .First();
        }

        public double Match(IImage pattern, IImage image)
        {
            // ここでパターンマッチング
            // とりあえず SADで
            uint distance = 0;
            for (int y = 0; y < image.Width; ++y)
            {
                for (int x = 0; x < image.Height; ++x)
                {
                    distance += (uint) Math.Pow(image.GetElement(x, y, 0) - pattern.GetElement(x, y, 0), 2);
                }
            }

            return distance;
        }
    }
}
