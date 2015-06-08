﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    class CommandList : IEnumerable<Command>
    {
        private List<Command> _commandList = new List<Command>();

        public CommandList()
        {
        }

        /// <summary>
        /// IEnumeratorを返す。
        /// 
        /// IEnumerableの実装
        /// </summary>
        /// <returns>IEnumerator<Command></returns>
        public IEnumerator<Command> GetEnumerator()
        {
            return this._commandList.GetEnumerator();
        }

        /// <summary>
        /// IEnumeratorを返すIEnumerableの実装
        /// </summary>
        /// <returns>IEnumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
