﻿using System;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    public class HelpAttribute : Attribute
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string Syntax { get; set; }
        public string Description { get; set; }
        public string[] Examples { get; set; } = new string[0];
    }
}