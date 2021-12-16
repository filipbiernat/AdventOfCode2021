using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day16
{
    public class Day16A : IDay
    {
        private readonly Stack<bool> Data = new();
        private int VersionNumbers = 0;

        public void Run()
        {
            // The transmission was sent using the Buoyancy Interchange Transmission System (BITS), a method of packing numeric
            // expressions into a binary sequence. Your submarine's computer has saved the transmission in hexadecimal.
            string input = File.ReadAllText(@"..\..\..\Day16\Day16.txt");

            // The first step of decoding the message is to convert the hexadecimal representation into binary.
            foreach (bool bit in StringToBitArray(input))
            {
                Data.Push(bit);
            }

            // The BITS transmission contains a single packet at its outermost layer which itself contains many other packets.
            // The hexadecimal representation of this packet might encode a few extra 0 bits at the end;
            // these are not part of the transmission and should be ignored.
            // Decode the structure of your hexadecimal-encoded BITS transmission.
            ReadPacket();

            // What do you get if you add up the version numbers in all packets?
            Console.WriteLine("Solution: {0}.", VersionNumbers);
        }

        private void ReadPacket()
        {
            // Every packet begins with a standard header: the first three bits encode the packet version,
            // and the next three bits encode the packet type ID. 
            int packetVersion = ReadSegment(3);
            int packetTypeId = ReadSegment(3);
            // For now, parse the hierarchy of the packets throughout the transmission and add up all of the version numbers.
            VersionNumbers += packetVersion;

            if (packetTypeId == 4)
            {
                ReadLiteralValue();
            }
            else
            {
                ReadOperatorSubPackets();
            }
        }

        private void ReadLiteralValue()
        {
            // Packets with type ID 4 represent a literal value. Literal value packets encode a single binary number.
            // To do this, the binary number is padded with leading zeroes until its length is a multiple of four bits,
            // and then it is broken into groups of four bits.
            int literalValue = 0;
            // Each group is prefixed by a 1 bit except the last group, which is prefixed by a 0 bit.
            // These groups of five bits immediately follow the packet header.
            int dataIndicatorBit = 1;
            while (dataIndicatorBit == 1)
            {
                dataIndicatorBit = ReadSegment(1);
                literalValue = (literalValue << 4) + ReadSegment(4);
            }
        }
        private void ReadOperatorSubPackets()
        {
            // An operator packet contains one or more packets. To indicate which subsequent binary data represents its
            // sub-packets, an operator packet can use one of two modes indicated by the bit immediately after the packet header;
            // this is called the length type ID:
            int lengthTypeId = ReadSegment(1);
            // If the length type ID is 0, then the next 15 bits are a number that represents the total length in bits of the
            // sub-packets contained by this packet.
            if (lengthTypeId == 0)
            {
                int totalLengthOfSubPackets = ReadSegment(15);
                int dataCountAfterSubPackets = Data.Count - totalLengthOfSubPackets;
                // Finally, after the length type ID bit and the 15-bit or 11-bit field, the sub-packets appear.
                while (Data.Count > dataCountAfterSubPackets)
                {
                    ReadPacket();
                }
            }
            // If the length type ID is 1, then the next 11 bits are a number that represents the number of sub-packets
            // immediately contained by this packet.
            else if (lengthTypeId == 1)
            {
                int numberOfSubPackets = ReadSegment(11);
                // Finally, after the length type ID bit and the 15-bit or 11-bit field, the sub-packets appear.
                for (int subPacket = 0; subPacket < numberOfSubPackets; ++ subPacket)
                {
                    ReadPacket();
                }
            }
        }

        private int ReadSegment(int length)
        {
            int segment = 0;
            for (int bit = 0; bit < length; ++bit)
            {
                segment = (segment << 1) + (Data.Pop() ? 1 : 0);
            }
            return segment;
        }

        private static BitArray StringToBitArray(string hex)
        {
            return new BitArray(Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .Reverse()
                .ToArray());
        }
    }
}
