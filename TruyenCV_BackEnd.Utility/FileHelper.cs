﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TruyenCV_BackEnd.Utility
{
    public static class FileHelper
    {
        private enum Extensions
        {
            Unknown = 0,
            DocOrXls,
            Pdf,
            Jpg,
            Png,
            DocxOrXlsx,
            Xlsx
        }

        private static readonly Dictionary<Extensions, string> ExtensionSignature = new Dictionary<Extensions, string>
        {
            {Extensions.DocOrXls, "D0-CF-11-E0-A1-B1-1A-E1"},
            {Extensions.Pdf, "25-50-44-46"},
            {Extensions.Jpg, "FF-D8-FF-E"},
            {Extensions.Png, "89-50-4E-47-0D-0A-1A-0A"},
            {Extensions.DocxOrXlsx, "50-4B-03-04-14-00-06-00"},
            {Extensions.Xlsx, "50-4B-03-04-14-00-00-00"}
        };

        public static string GetExtension(byte[] bytes)
        {
            if (bytes.Length < 8)
                return string.Empty;
            var signatureBytes = new byte[8];
            Array.Copy(bytes, signatureBytes, signatureBytes.Length);
            string signature = BitConverter.ToString(signatureBytes);
            Extensions extension = ExtensionSignature.FirstOrDefault(pair => signature.Contains(pair.Value)).Key;
            switch (extension)
            {
                case Extensions.Unknown:
                    return string.Empty;
                case Extensions.DocOrXls:
                    if (bytes.Length < 512)
                        break;
                    signatureBytes = new byte[4];
                    Array.Copy(bytes, 512, signatureBytes, 0, signatureBytes.Length);
                    signature = BitConverter.ToString(signatureBytes);
                    if (signature == "EC-A5-C1-00")
                        return ".doc";
                    return ".xls";
                case Extensions.Pdf:
                    return ".pdf";
                case Extensions.Jpg:
                    return ".jpg";
                case Extensions.Png:
                    return ".png";
                case Extensions.DocxOrXlsx:
                    string fileBody = Encoding.UTF8.GetString(bytes);
                    if (fileBody.Contains("word"))
                        return ".docx";
                    if (fileBody.Contains("xl"))
                        return ".xlsx";
                    break;
                case Extensions.Xlsx:
                    return ".xlsx";
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return string.Empty;
        }
    }
}
