using System.Collections.Generic;

namespace Myxini.Recognition
{
	public class BlockGraph
	{

		/// <summary>
		/// 新たにRootにブロックを追加します
		/// </summary>
		/// <param name="block">新たに追加するブロックです</param>
		/// <returns>追加したブロックを内包するノードを返します</returns>
		public BlockNode Add(IBlock block)
		{
			var node = new BlockNode(block);
			this.Root.Add(node);
			return node;
		}

		/// <summary>
		/// ブロックを引数に与えたノードの下に追加する
		/// </summary>
		/// <param name="node">この引数のノードの下に新たにノードを追加します</param>
		/// <param name="block">あらたに追加するブロックです</param>
		/// <returns>追加したブロックを内包するノードです</returns>
		public BlockNode Add(BlockNode node, IBlock block)
		{
			var next_node = new BlockNode(block);
			node.AddNextNode(next_node);
			next_node.AddPreviousNode(node);

			return next_node;
		}

		/// <summary>
		/// ノードを取得する
		/// </summary>
		/// <param name="index">取得したいノードのインデックス</param>
		/// <returns>取得したノード</returns>
		BlockNode GetRootNode(int index)
		{
			return this.Root[index];
		}

		/// <summary>
		/// Rootの長さを取得する
		/// </summary>
		int Count { get { return this.Root.Count; } }

		private List<BlockNode> Root { get; set; }
	}
}
