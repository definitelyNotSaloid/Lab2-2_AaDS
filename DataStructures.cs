using System;
using System.Collections.Generic;
using System.Text;


namespace Lab22_AaDS.Collections
{
    public class NotABitVector 
    {
        // bits are indexed as following:
        // 31, 30, 29, ... , 1, 0, |next block start| 63, 62, .. , 33, 32, |next block|, ..
        private UInt32[] bitArray;
        public int Count { get; private set; }

        public bool this[int index]
        {
            get => (bitArray[index / 32] & (1 << index%32)) > 0;
            set => throw new NotImplementedException();
        }
        

        private void AppendReversed(uint bitSequence, int sequenceLen)       
        {

            Reserve(Count + sequenceLen);
            if (sequenceLen > 32)
                throw new ArgumentException();
            if (sequenceLen < 1)
                throw new ArgumentException();

            int leftSideLen = Math.Min(32 - Count % 32, sequenceLen);        // part that will be at the same block as last bit
            int rightSideLen = sequenceLen - leftSideLen;                    // part that will be transfered to the next block
            uint bitRange = (uint)Math.Pow(2, sequenceLen) - 1;
            bitRange <<= Count % 32;
            bitRange = ~bitRange;
            bitArray[Count / 32] &= bitRange;

            bitArray[Count / 32] |= (bitSequence << Count % 32);

            if (rightSideLen != 0)
            {
                bitArray[Count / 32 + 1] |= (bitSequence >> leftSideLen);
            }

            Count += sequenceLen;
        }

        public void Append(bool bit)
        {
            uint bitAsInt = 1;
            bitAsInt <<= Count % 32;
            Reserve(Count + 1);
            if (bit)
                bitArray[Count / 32] |= bitAsInt;
            else
                bitArray[Count / 32] &= (~bitAsInt);
            Count++;
        }

        public void Append(NotABitVector other)
        {
            this.Reserve(this.Count + other.Count);
            for (int i = 0; i < other.Count / 32; i++)
            {
                this.AppendReversed(other.bitArray[i], 32);
            }

            if (other.Count%32!=0)
            {
                this.AppendReversed(other.bitArray[other.Count / 32], other.Count % 32);
            }
        }

        public void DropLast(int bitCount)
        {
            if (bitCount > Count)
                throw new ArgumentException();

            Count -= bitCount;
        }


        public NotABitVector()
        {
            bitArray = new uint[1];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for(int i=0;i<Count;i++)
            {
                sb.Append(this[i] ? '1' : '0');
            }

            return sb.ToString();
        }

        private void Reserve(int bitCount)
        {
            if (bitCount <= bitArray.Length*32)
                return;

            int blockCount = (int)Math.Ceiling(bitCount / 32d);
            var newArr = new uint[blockCount];
            bitArray.CopyTo(newArr, 0);
            bitArray = newArr;
        }
    }


    public struct Pair<T1, T2>
    {
        public T1 first;
        public T2 second;

        public Pair(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
