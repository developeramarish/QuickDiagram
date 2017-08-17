﻿using Codartis.SoftVis.Modeling;

namespace Codartis.SoftVis.TestHostApp.Modeling
{
    public class TestModelNodeStereotype : ModelNodeStereotype
    {
        public static readonly ModelNodeStereotype Interface = new TestModelNodeStereotype(nameof(Interface));

        private TestModelNodeStereotype(string name) : base(name)
        {
        }
    }
}
