using UnityEngine;

public static class ImageSlicer
{
    /// <summary>
    /// Slices an image into squares. If the source image is not 1:1, it will cut out the excess parts.
    /// </summary>
    /// <param name="image">The source image.</param>
    /// <param name="slicesPerLine">The number of rows and columns the image is to be sliced to.</param>
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