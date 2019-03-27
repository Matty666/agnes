using System;

namespace agnes.Utils
{
    public static class ReflectiveMatrix
    {
        public static void Iterate(Action<int, int> sameIndexAction, Action<int, int> differentIndexAction, int max)
        {
            var xIndex = 0;
            var yIndex = 0;

            while (yIndex < max)
            {
                if (xIndex == yIndex)
                {
                    sameIndexAction?.Invoke(xIndex, yIndex);
                    xIndex = 0;
                    yIndex += 1;
                    continue;
                }

                differentIndexAction(xIndex, yIndex);
                xIndex += 1;
            }
        }
    }
}
