using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day16
{
    public class Day16B : IDay
    {
        private readonly Stack<bool> Data = new();

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
            long output = ReadPacket();

            // What do you get if you evaluate the expression represented by your hexadecimal-encoded BITS transmission?
            Console.WriteLine("Solution: {0}.", output);
        }

        private long ReadPacket()
        {
            // Every packet begins with a standard header: the first three bits encode the packet version,
            // and the next three bits encode the packet type ID. 
            long packetVersion = ReadSegment(3);
            long packetTypeId = ReadSegment(3);

            // Now that you have the structure of your transmission decoded, you can calculate the value of the expression it
            // represents.
            if (packetTypeId == 4)
            {
                return ReadLiteralValue();
            }
            else
            {
                List<long> subPackets = ReadOperatorSubPackets();
                return EvaluateOperation(packetTypeId, subPackets);
            }
        }

        private long ReadLiteralValue()
        {
            // Packets with type ID 4 represent a literal value. Literal value packets encode a single binary number.
            // To do this, the binary number is padded with leading zeroes until its length is a multiple of four bits,
            // and then it is broken into groups of four bits.
            long literalValue = 0;
            // Each group is prefixed by a 1 bit except the last group, which is prefixed by a 0 bit.
            // These groups of five bits immediately follow the packet header.
            long dataIndicatorBit = 1;
            while (dataIndicatorBit == 1)
            {
                dataIndicatorBit = ReadSegment(1);
                literalValue = (literalValue << 4) + ReadSegment(4);
            }
            return literalValue;
        }

        private List<long> ReadOperatorSubPackets()
        {
            // An operator packet contains one or more packets.
            List<long> subPackets = new();
            // To indicate which subsequent binary data represents its sub-packets, an operator packet can use one of two modes
            // indicated by the bit immediately after the packet header; this is called the length type ID:
            long lengthTypeId = ReadSegment(1);
            // If the length type ID is 0, then the next 15 bits are a number that represents the total length in bits of the
            // sub-packets contained by this packet.
            if (lengthTypeId == 0)
            {
                long totalLengthOfSubPackets = ReadSegment(15);
                long dataCountAfterSubPackets = Data.Count - totalLengthOfSubPackets;
                // Finally, after the length type ID bit and the 15-bit or 11-bit field, the sub-packets appear.
                while (Data.Count > dataCountAfterSubPackets)
                {
                    subPackets.Add(ReadPacket());
                }
            }
            // If the length type ID is 1, then the next 11 bits are a number that represents the number of sub-packets
            // immediately contained by this packet.
            else if (lengthTypeId == 1)
            {
                long numberOfSubPackets = ReadSegment(11);
                // Finally, after the length type ID bit and the 15-bit or 11-bit field, the sub-packets appear.
                for (long subPacket = 0; subPacket < numberOfSubPackets; ++ subPacket)
                {
                    subPackets.Add(ReadPacket());
                }
            }
            return subPackets;
        }

        private long ReadSegment(long length)
        {
            long segment = 0;
            for (long bit = 0; bit < length; ++bit)
            {
                segment = (segment << 1) + (Data.Pop() ? 1 : 0);
            }
            return segment;
        }

        private static long EvaluateOperation(long packetTypeId, List<long> subPackets)
        {
            if (packetTypeId == 0)
            {
                // Packets with type ID 0 are sum packets - their value is the sum of the values of their sub-packets.
                // If they only have a single sub-packet, their value is the value of the sub-packet.
                return subPackets.Sum();
            }
            else if (packetTypeId == 1)
            {
                // Packets with type ID 1 are product packets - their value is the result of multiplying together the values of
                // their sub-packets. If they only have a single sub-packet, their value is the value of the sub-packet.
                return subPackets.Aggregate((lhs, rhs) => lhs * rhs);
            }
            else if (packetTypeId == 2)
            {
                // Packets with type ID 2 are minimum packets - their value is the minimum of the values of their sub-packets.
                return subPackets.Min();
            }
            else if (packetTypeId == 3)
            {
                // Packets with type ID 3 are maximum packets - their value is the maximum of the values of their sub-packets.
                return subPackets.Max();
            }
            else if (packetTypeId == 5)
            {
                // Packets with type ID 5 are greater than packets - their value is 1 if the value of the first sub-packet is
                // greater than the value of the second sub-packet; otherwise, their value is 0. These packets always have
                // exactly two sub-packets.
                return (subPackets.ElementAt(0) > subPackets.ElementAt(1)) ? 1 : 0;
            }
            else if (packetTypeId == 6)
            {
                // Packets with type ID 6 are less than packets - their value is 1 if the value of the first sub-packet is
                // less than the value of the second sub-packet; otherwise, their value is 0. These packets always have
                // exactly two sub-packets.
                return (subPackets.ElementAt(0) < subPackets.ElementAt(1)) ? 1 : 0;
            }
            else
            {
                // Packets with type ID 7 are equal to packets - their value is 1 if the value of the first sub-packet is
                // equal to the value of the second sub-packet; otherwise, their value is 0. These packets always have
                // exactly two sub-packets.
                return (subPackets.ElementAt(0) == subPackets.ElementAt(1)) ? 1 : 0;
            }
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
