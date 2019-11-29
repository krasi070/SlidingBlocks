using UnityEngine;

public static class ImageSlicer
{
    public static Texture2D[,] GetSlices(Texture2D image, int slicesPerLine)
    {
        int imageSize = Mathf.Min(image.width, image.height);
        int sliceSize = imageSize / slicesPerLine;

        Texture2D[,] slices = new Texture2D[slicesPerLine, slicesPerLine];
        for (int row = 0; row < slicesPerLine; row++)
        {
            for (int col = 0; col < slicesPerLine; col++)
            {
                Texture2D slice = new Texture2D(sliceSize, sliceSize);
                slice.wrapMode = TextureWrapMode.Clamp;
                slice.SetPixels(image.GetPixels(col * sliceSize, row * sliceSize, sliceSize, sliceSize));
                slice.Apply();

                slices[row, col] = slice;
            }
        }

        return slices;
    }
}