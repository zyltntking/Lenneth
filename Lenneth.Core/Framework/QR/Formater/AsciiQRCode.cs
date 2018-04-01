using System.Collections.Generic;
using System.Text;

namespace Lenneth.Core.Framework.QR.Formater
{
    using Base;

    public class AsciiQrCode : AbstractQrCode
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public AsciiQrCode() { }

        public AsciiQrCode(QrCodeData data) : base(data)
        {
        }

        /// <summary>
        /// Returns a strings that contains the resulting QR code as ASCII chars.
        /// </summary>
        /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
        /// <returns></returns>
        public string GetGraphic(int repeatPerModule) => string.Join("\n", GetLineByLineGraphic(repeatPerModule));

        /// <summary>
        /// Returns a strings that contains the resulting QR code as ASCII chars.
        /// </summary>
        /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
        /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
        /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
        /// <param name="endOfLine">End of line separator. (Default: \n)</param>
        /// <returns></returns>
        public string GetGraphic(int repeatPerModule, string darkColorString, string whiteSpaceString, string endOfLine = "\n") => string.Join(endOfLine, GetLineByLineGraphic(repeatPerModule, darkColorString, whiteSpaceString));

        /// <summary>
        /// Returns an array of strings that contains each line of the resulting QR code as ASCII chars.
        /// </summary>
        /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
        /// <returns></returns>
        public string[] GetLineByLineGraphic(int repeatPerModule) => GetLineByLineGraphic(repeatPerModule, "██", "  ");

        /// <summary>
        /// Returns an array of strings that contains each line of the resulting QR code as ASCII chars.
        /// </summary>
        /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
        /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
        /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
        /// <returns></returns>
        public string[] GetLineByLineGraphic(int repeatPerModule, string darkColorString, string whiteSpaceString)
        {
            var qrCode = new List<string>();
            //We need to adjust the repeatPerModule based on number of characters in darkColorString
            //(we assume whiteSpaceString has the same number of characters)
            //to keep the QR code as square as possible.
            var adjustmentValueForNumberOfCharacters = darkColorString.Length / 2 != 1 ? darkColorString.Length / 2 : 0;
            var verticalNumberOfRepeats = repeatPerModule + adjustmentValueForNumberOfCharacters;
            var sideLength = QrCodeData.ModuleMatrix.Count * verticalNumberOfRepeats;
            for (var y = 0; y < sideLength; y++)
            {
                var emptyLine = true;
                var lineBuilder = new StringBuilder();

                foreach (var item in QrCodeData.ModuleMatrix)
                {
                    var module = item[(y + verticalNumberOfRepeats) / verticalNumberOfRepeats - 1];

                    for (var i = 0; i < repeatPerModule; i++)
                    {
                        lineBuilder.Append(module ? darkColorString : whiteSpaceString);
                    }
                    if (module)
                    {
                        emptyLine = false;
                    }
                }
                if (!emptyLine)
                {
                    qrCode.Add(lineBuilder.ToString());
                }
            }
            return qrCode.ToArray();
        }
    }
}