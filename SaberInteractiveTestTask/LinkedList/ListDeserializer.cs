using System;
using System.Collections.Generic;
using System.IO;

namespace LinkedList
{
	public class ListDeserializer
	{
		private FileStream fileStream;
		private ListRand listRand;

		public void Deserialize(FileStream stream, ListRand list)
        {
			fileStream = stream;
			listRand = list;

			if (!IsFileDeserializeAvalable())
				return;

			List<ListNodeWrapper> nodesWrappers = new();
			Dictionary<ListNode, int> nodesWRandomIdx = new();

			int s;
			while (!IsByteEqualTo(s = fileStream.ReadByte(), ']'))
			{
				if (IsByteEqualTo(s, '{'))
                {
					ParseNode(nodesWrappers);
					AddNodeToList(nodesWrappers, nodesWRandomIdx);
                }
			}

			foreach (var val in nodesWRandomIdx)
			{
				val.Key.Rand = nodesWrappers[val.Value].Node;
			}
		}

		private bool IsFileDeserializeAvalable()
        {
			int s = fileStream.ReadByte();
			if (s == -1 || !IsByteEqualTo(s, '['))
			{
				Console.WriteLine("Something's wrong with the file!");
				return false;
			}
			return true;
		}

		private void AddNodeToList(List<ListNodeWrapper> nodesWrappers, Dictionary<ListNode, int> nodesWRandomIdx)
        {
			var curNodeInfo = nodesWrappers[^1];

			if (curNodeInfo.Prev == "null")
            {
				listRand.Head = curNodeInfo.Node;
				return;
            }

			if (curNodeInfo.Next == "null") 
			{
				listRand.Tail = curNodeInfo.Node;
				listRand.Count = nodesWrappers.Count;
			}

			var prevNodeInfo = nodesWrappers[^2];
			prevNodeInfo.Node.Next = curNodeInfo.Node;
			curNodeInfo.Node.Prev = prevNodeInfo.Node;

			if (!int.TryParse(curNodeInfo.Rand, out int rand))
				return;
			nodesWRandomIdx[curNodeInfo.Node] = rand;
		}

		private void ParseNode(List<ListNodeWrapper> nodesWrappers)
        {
			var nodeInfo = new Dictionary<string, string>();
			bool isCurNode = false;
			do
			{
				string[] vals = GetNodeInfoParams(ref isCurNode);
				nodeInfo[vals[0]] = vals[1];
			} while (isCurNode);

			var node = new ListNode { Data = nodeInfo["Data"] };
			var nodeWrapper = new ListNodeWrapper { Prev = nodeInfo["Prev"], Next = nodeInfo["Next"], Rand = nodeInfo["Rand"], Node = node };
			nodesWrappers.Add(nodeWrapper);
        }

		private string[] GetNodeInfoParams(ref bool isCurNode)
        {
			var val = string.Empty;
			isCurNode = TryConvertUntil(ref val, ',');
			var parts = val.Split(':', 2);
			return parts;
        }

		private bool TryConvertUntil(ref string val, char stopChar, bool ignoreBracket = false)
        {
			int s;			
			while (!IsByteEqualTo(s = fileStream.ReadByte(), stopChar))
            {
				if (!ignoreBracket && IsByteEqualTo(s, '}'))
					return false;

				if (IsByteEqualTo(s, '\''))
				{
					TryConvertUntil(ref val, '\'', true);
					continue;
				}
				val += (char)s;
            }
			return true;
        }

		private bool IsByteEqualTo(int checkByte, char checkChar)
			=> (char)checkByte == checkChar;
	}
}
