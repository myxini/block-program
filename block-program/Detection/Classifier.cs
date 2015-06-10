using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    class Classifier : IClassifier
    {
        private class Pattern
        {
            private Image.IImage pattern;
            IBlock Block { get; private set; }
            
            Pattern(Image.IImage pattern, IBlock block)
            {
                this.pattern = pattern;
                Block = block;
            }

            double Match(Image.IImage image)
            {
                // ここでパターンマッチング
                // 仮に値を返す
                return (new Random()).NextDouble() * 2 - 1;
            }
        }

        private IList<Pattern> patterns = new List<Pattern>
        {
            //LED
            //Move
            //Rotate
            //MicroSwitch
            //PSD
            //Start
            //End
        };

        public IBlock clustering(Raw.IRawBlock raw_block)
        {
            // ここでパターンマッチング

            throw new NotImplementedException();
        }
    }
}
