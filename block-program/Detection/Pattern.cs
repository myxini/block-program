using Myxini.Recognition.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class Pattern
    {
        private IImage pattern;
        public IBlock Block { get; private set; }

        public Pattern(IImage pattern, IBlock block)
        {
            this.pattern = pattern;
            Block = block;
        }

        public double Match(IImage image, IPatternMatchingAlgorithm algorithm)
        {
            return algorithm.Match(pattern, image);
        }
    }
}
