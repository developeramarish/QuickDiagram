﻿using System.Collections.Generic;

namespace Codartis.SoftVis.Diagramming.Graph.Layout.Incremental.Relative
{
    /// <summary>
    /// A read-only view of the layout vertex layers.
    /// </summary>
    internal interface IReadOnlyLayoutVertexLayers : IEnumerable<IReadOnlyLayoutVertexLayer>
    {
        int Count { get; }

        int GetLayerIndex(LayoutVertexBase vertex);
        int GetIndexInLayer(LayoutVertexBase vertex);

        bool HasLocation(LayoutVertexBase vertex);
        RelativeLocation GetLocation(LayoutVertexBase vertex);

        IReadOnlyLayoutVertexLayer GetLayer(LayoutVertexBase vertex);
        IReadOnlyLayoutVertexLayer GetLayer(int layerIndex);
    }
}