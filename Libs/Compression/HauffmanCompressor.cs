using Libs.BinaryTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libs.Compression
{
    public class HauffmanCompressor
    {
        private BinaryTree _path;
        private HeapNode<BinaryTree>[] _charCount;

        public void Fit(IEnumerable<string> texts)
        {
            _charCount = new HeapNode<BinaryTree>[256];
            foreach (var txt in texts)
                foreach (char c in txt)
                {
                    _charCount[c] ??= new HeapNode<BinaryTree>(new BinaryTree(c), 0);
                    _charCount[c].Freq++;
                }
            //addinng character 0 as end of document
            _charCount[0] ??= new HeapNode<BinaryTree>(new BinaryTree(0), 0);

            var mapper = new BinaryHeap<BinaryTree>();
            mapper.BuildHeap(_charCount.Where(c=> c!=null));
            
            do
            {
                var a = mapper.ExtractMin();
                var b = mapper.ExtractMin();

                var heapNode = new HeapNode<BinaryTree>(
                    new BinaryTree 
                    { 
                        Element = -1, 
                        Left = a.Element, 
                        Right = b.Element
                    }, a.Freq + b.Freq);
                a.Element.Parent = heapNode.Element;
                b.Element.Parent = heapNode.Element;
                mapper.Insert(heapNode);
            } while (mapper.HeapSize > 1);
            
            _path = mapper.ExtractMin().Element;
        }

        public IEnumerable<byte[]> Encode(IEnumerable<string> texts)
        {
            foreach (var txt in texts)
            {
                var buffer = new List<byte>(txt.Length);
                var @byte = new List<bool>(8);
                foreach (bool bit in txt
                    .SelectMany(c=> GetCharPath(c)) //read all binary buffer
                    .Concat(GetCharPath(0)))// add character 0 as the end of document
                {
                    @byte.Add(bit);
                    if (@byte.Count == 8)
                    {
                        buffer.Add(EncodeBool(@byte.ToArray()));
                        @byte = new List<bool>(8);
                    }
                }
                if (@byte.Count > 0)
                {
                    //completing last byte with ceros
                    @byte.AddRange(Enumerable.Repeat(false, 8 - @byte.Count));
                    buffer.Add(EncodeBool(@byte.ToArray()));
                }

                yield return buffer.ToArray();
            }
        }
        public IEnumerable<byte[]> FitEncode(IEnumerable<string> texts)
        {
            Fit(texts);
            return Encode(texts).ToArray();
        }

        public IEnumerable<string> Decode(IEnumerable<byte[]> encodedStrings)
        {
            foreach (var encoded in encodedStrings)
                yield return Decode(encoded);
        }
        public string Decode(byte[] encodedString)
        {
            var buffer = encodedString.SelectMany(b=> DecodeBool(b)).GetEnumerator();
            var builder = new StringBuilder();

            var node = _path;
            while (buffer.MoveNext())
            {
                if (buffer.Current) node = node.Right;
                else node = node.Left;

                if (node.Left == node.Right && node.Right == null)
                {
                    if (node.Element == 0)
                        break; // end of document found
                    builder.Append((char)node.Element);
                    node = _path;
                }
            }
            return builder.ToString();
        }

        public bool[] GetCharPath(int c)
        {
            if (_charCount[c] == null)
                return null;

            var node = _charCount[c].Element;
            var path = new LinkedList<bool>();
            var p = node.Parent;
            while (p != null)
            {
                if (p.Left == node)
                    path.AddFirst(false);
                else path.AddFirst(true);
                node = p;
                p = node.Parent;
            }
            return path.ToArray();
        }

        public IEnumerable<(char c, bool[] enc)> GetEncodings() => Enumerable.Range(1, 255).Select(i => ((char)i, GetCharPath(i)));

        private byte EncodeBool(bool[] arr)
        {
            byte val = 0;
            foreach (bool b in arr)
            {
                val <<= 1;
                if (b) val |= 1;
            }
            return val;
        }

        private bool[] DecodeBool(byte b)
        {
            bool[] result = new bool[8];

            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            Array.Reverse(result);

            return result;
        }


    }
}
