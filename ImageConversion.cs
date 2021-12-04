using System;

namespace Assets
{
    public static class ImageConversion
    {
        public static unsafe byte[] OtsuThreshold(this byte[] raw, int _width, int _height)
        {
            const int depth = 256;
            int width = _width;
            int height = _height;
            int size = height * width;
            byte[] binarized = new byte[size];
            fixed(byte* img = raw)
            {
                try
                {
                    byte* curr = img;
                    for (int j = 0; j < height; j++)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            binarized[width * j + i] = (byte)((*(curr++) + *(curr++) + *(curr++)) / 3);
                        }
                    }

                    int* hist = stackalloc int[depth];
                    long fullIntensity = 0;

                    for (int i = 0; i < size; ++i)
                    {
                        ++hist[binarized[i]];
                        fullIntensity += binarized[i];
                    }

                    int threshold = 0;
                    float maxSigma = 0;
                    int fcpc = 0;
                    int fcis = 0;

                    for (int t = 0; t < depth - 1; t++)
                    {
                        fcpc += hist[t];
                        fcis += t * hist[t];

                        float fcp = fcpc / (float)size;
                        float scp = 1.0f - fcp;
                        float fcm = fcis / (float)fcpc;
                        float scm = (fullIntensity - fcis) / (float)(size - fcpc);
                        float delta = fcm - scm;
                        float sigma = fcp * scp * delta * delta;
                        if (sigma > maxSigma)
                        {
                            maxSigma = sigma;
                            threshold = t;
                        }
                    }

                    for (int i = 0; i < size; i++)
                    {
                        binarized[i] = (binarized[i] >= threshold) ? (byte)255 : (byte)0;
                    }
                }
                catch (Exception)
                {
                    
                }
                return binarized;
            }
        }
    }
}
