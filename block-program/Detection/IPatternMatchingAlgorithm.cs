using Myxini.Recognition.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public interface IPatternMatchingAlgorithm
    {
        /// <summary>
        /// raw_blockと一番よくマッチするpatternを返す
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="raw_block"></param>
        /// <returns></returns>
        Pattern Match(IEnumerable<Pattern> patterns, IImage raw_block);

        /// <summary>
        /// imageとpatternのマッチする具合を返す
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        double Match(IImage pattern, IImage image);
    }
}
