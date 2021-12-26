using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day20
{
    public class Day20A : IDay
    {
        public void Run()
        {
            // When you get back the image from the scanners, it seems to just be random noise. Perhaps you can combine an image
            // enhancement algorithm and the input image (your puzzle input) to clean it up a little.
            string[] input = File.ReadAllText(@"..\..\..\Day20\Day20.txt").Split("\r\n\r\n");

            // Start with the original input image and apply the image enhancement algorithm twice.
            Image image = new(input);
            for (int step = 0; step < 2; ++step)
            {
                image.Enhance();
            }

            // How many pixels are lit in the resulting image?
            int output = image.GetActivePixelCount();
            Console.WriteLine("Solution: {0}", output);
        }
    }
}
