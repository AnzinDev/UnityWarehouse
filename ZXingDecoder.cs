using ZXing;
using System.Collections.Generic;

namespace Assets
{
    public class ZXingDecoder
    {
        private BarcodeReader BarcodeReader {get; set;}
        public string resultText { get; private set; }

        public ZXingDecoder(BarcodeFormat format)
        {
            BarcodeReader = new BarcodeReader();
            List<BarcodeFormat> possibleFormats = new List<BarcodeFormat>();
            possibleFormats.Add(format);
            BarcodeReader.Options.PossibleFormats = possibleFormats;
        }

        public void DecodeImage(byte[] raw, int width, int height)
        {
            Result result = BarcodeReader.Decode(raw.OtsuThreshold(width, height), width, height, RGBLuminanceSource.BitmapFormat.Unknown);
            resultText = (result != null) ? result.Text : null;
        }
    }
}
