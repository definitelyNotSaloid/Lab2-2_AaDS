using System;
using System.Collections.Generic;
using System.Text;
using Lab21_AaDS;
using Lab22_AaDS.Collections;
using Lab1_AaDS;

namespace Lab22_AaDS.Encoder
{
    public class HuffmanEncoder
    {
        private NotADictionary<char, int> frequencies = new NotADictionary<char, int>();

        public NotABitVector Encode(
            string str, 
            out Encoding encoding)
        {
            if (str.Length == 0)
            {
                encoding = null;
                return new NotABitVector();
            }
            CountFrequencies(str);
            encoding = new Encoding(frequencies);

            NotABitVector res = new NotABitVector();
            foreach (var ch in str)
                res.Append(encoding.CodeOf(ch));

            FlushFrequencies();
            return res;
        }

        private void CountFrequencies(string str)
        {
            foreach (char ch in str)
            {
                if (!frequencies.ContainsKey(ch))
                    frequencies.Add(ch, 0);

                frequencies[ch]++;
            }
        }

       void FlushFrequencies() => frequencies = new NotADictionary<char, int>();    
    }
    
    public class Encoding
    {
        private LeafOnlyTreeNode root;

        public Encoding(NotADictionary<char, int> frequencies)
        {
            if (frequencies.Count == 0)
                throw new ArgumentException();

            var nodeList = new NotAList<Pair<int,LeafOnlyTreeNode>>();

            foreach (var keyValuePair in frequencies)
            {
                nodeList.Add(new Pair<int, LeafOnlyTreeNode>(
                    keyValuePair.Value, 
                    new LeafOnlyTreeNode(keyValuePair.Key)
                    ));
            }

            while (nodeList.Count>1)
            {
                var minWeightNode1 = nodeList[0];
                


                // double iteration is unoptimal, but i dont want to make this code too complex
                foreach (var node in nodeList)
                {
                    if (node.first < minWeightNode1.first)
                        minWeightNode1 = node;
                }
                nodeList.Remove(minWeightNode1);

                var minWeightNode2 = nodeList[0];
                foreach (var node in nodeList)
                {
                    if (node.first < minWeightNode2.first)
                        minWeightNode2 = node;
                }
                nodeList.Remove(minWeightNode2);


                var newNode = new LeafOnlyTreeNode(null);
                newNode.leftChild = minWeightNode1.second;
                newNode.rightChild = minWeightNode2.second;
                minWeightNode1.second.parent = newNode;
                minWeightNode2.second.parent = newNode;

                nodeList.InsertAt(new Pair<int, LeafOnlyTreeNode>(
                    minWeightNode1.first + minWeightNode2.first,
                    newNode),
                    0);
            }

            root = nodeList[0].second;
        }

        public NotABitVector CodeOf(char ch)
        {
            LeafOnlyTreeNode nodeItr = root;
            var res = new NotABitVector();
            res.Append(true);
           
            while (nodeItr.val != ch)
            {
                // goin deeper
                if (nodeItr.leftChild!=null)
                {
                    nodeItr = nodeItr.leftChild;
                    res.Append(false);
                }
                else if (nodeItr.rightChild!=null)
                {
                    nodeItr = nodeItr.leftChild;
                    res.Append(true);
                }
                /*
                else 
                if (nodeItr.parent.leftChild == nodeItr
                    && nodeItr.parent.rightChild != null)
                {
                    nodeItr = nodeItr.parent.rightChild;
                    res.DropLast(1);
                    res.Append(true);
                }*/

                else // TODO rewrite it to be more readable
                {
                    while(nodeItr.parent?.rightChild == nodeItr)
                    {
                        nodeItr = nodeItr.parent;
                        res.DropLast(1);
                    }

                    if (nodeItr.parent == null)
                        return null;

                    nodeItr = nodeItr.parent.rightChild;
                    res.DropLast(1);
                    res.Append(true);
                }
            }
            
            return res;
        }

        public string Decode(NotABitVector encoded)
        {
            LeafOnlyTreeNode curNode = null;

            StringBuilder sb = new StringBuilder();


            for (int i=0;i<encoded.Count;i++)
            {
                if (curNode==null)
                {
                    if (!encoded[i]) // if bit == 0
                        throw new ArgumentException();

                    else 
                        curNode = root;
                }

                else
                {
                    if (!encoded[i])
                        curNode = curNode.leftChild ?? throw new ArgumentException();
                    else
                        curNode = curNode.rightChild ?? throw new ArgumentException();
                }

                
                if (curNode.val!=null)
                {
                    sb.Append(curNode.val);
                    curNode = null;
                }
            }

            return sb.ToString();
        }
    }

    internal class LeafOnlyTreeNode
    {
        public readonly char? val;

        public LeafOnlyTreeNode parent = null;
        public LeafOnlyTreeNode leftChild= null;
        public LeafOnlyTreeNode rightChild= null;

        public LeafOnlyTreeNode(char? ch)
        {
            val = ch;
        }
    }
}
