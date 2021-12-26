namespace AdventOfCode2021.Day20
{
    public class Image
    {
        private readonly List<bool> ImageEnhancementAlgorithm;
        private HashSet<Coords> ActivePixels;

        private int InfinitePixelValue;
        private Range FiniteRange;

        public Image(string[] input)
        {
            // The small input image you have is only a small region of the actual infinite input image; the rest of the input
            // image consists of dark pixels (.)

            // The first section is the image enhancement algorithm. It is normally given on a single line.
            ImageEnhancementAlgorithm = input[0].ToCharArray()
                .Select(elem => elem == '#')
                .ToList();

            // The second section is the input image, a two-dimensional grid of light pixels (#) and dark pixels (.).
            ActivePixels = input[1].Split("\r\n")
               .SelectMany((row, rowIndex) => row
                   .ToCharArray()
                   .Select((elem, colIndex) => Tuple.Create(elem, new Coords(rowIndex, colIndex))))
               .Where(pair => pair.Item1 == '#')
               .Select(pair => pair.Item2)
               .ToHashSet();

            // The small input image you have is only a small region of the actual infinite input image; the rest of the input
            // image consists of dark pixels (.).
            InfinitePixelValue = 0;
            FiniteRange = new(ActivePixels);
        }

        public void Enhance()
        {
            // The image enhancement algorithm describes how to enhance an image by simultaneously converting all pixels in the
            // input image into an output image. This process can then be repeated to calculate every pixel of the output image.
            HashSet<Coords> activePixelsInOutputImage = new Range(ActivePixels, delta: 1).ToEnumerable()
                .AsParallel()
                .Where(pixelCoords => IsEnhancedPixelActive(pixelCoords))
                .ToHashSet();
            ActivePixels = activePixelsInOutputImage;

            // Through advances in imaging technology, the images being operated on here are infinite in size. Every pixel of the
            // infinite output image needs to be calculated exactly based on the relevant pixels of the input image.
            InfinitePixelValue = IsEnhancedPixelActive(new Coords(int.MaxValue, int.MaxValue)) ? 1 : 0;
            FiniteRange = new(ActivePixels);
        }

        public int GetActivePixelCount() => ActivePixels.Count;

        private bool IsEnhancedPixelActive(Coords pixelToEnhance)
        {
            // Each pixel of the output image is determined by looking at a 3x3 square of pixels centered on the corresponding
            // input image pixel.
            List<Coords> squareOfPixels = NeighbourPixels.Select(coords => coords + pixelToEnhance).ToList();

            // These nine input pixels are combined into a single binary number that is used as an index in the image enhancement
            // algorithm string.
            int binaryNumber = 0;
            squareOfPixels.ForEach(pixel => binaryNumber = (binaryNumber << 1) + GetPixelValue(binaryNumber, pixel));
            return ImageEnhancementAlgorithm[binaryNumber];
        }

        private int GetPixelValue(int binaryNumber, Coords pixel) =>
            FiniteRange.IsIn(pixel) ? (ActivePixels.Contains(pixel) ? 1 : 0) : InfinitePixelValue;

        private static readonly List<Coords> NeighbourPixels = new()
        {
            new Coords(-1, -1),
            new Coords(-1, 0),
            new Coords(-1, 1),
            new Coords(0, -1),
            new Coords(0, 0),
            new Coords(0, 1),
            new Coords(1, -1),
            new Coords(1, 0),
            new Coords(1, 1)
        };
    }
}