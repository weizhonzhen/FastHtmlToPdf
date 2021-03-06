﻿using FastHtmlToPdf.Core.Context;
using System;

namespace FastHtmlToPdf.Core.Model
{
    public class ImageDocument : MarshalByRefObject
    {
        //public int MarginTop { get; set; }

        //public int MarginLeft { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Quality { get; set; } = 94;

        //public string Url { get; set; }

        public FormatEnum Format { get; set; } = FormatEnum.jpg;

        public bool SmartWidth { get; set; }

        public bool Transparent { get; set; }
    }
}
