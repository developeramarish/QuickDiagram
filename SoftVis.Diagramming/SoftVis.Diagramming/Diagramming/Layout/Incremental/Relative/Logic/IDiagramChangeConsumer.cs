﻿namespace Codartis.SoftVis.Diagramming.Layout.Incremental.Relative.Logic
{
    /// <summary>
    /// The implementer performs some action for each diagram change.
    /// </summary>
    internal interface IDiagramChangeConsumer
    {
        void OnDiagramCleared();
        void OnDiagramNodeAdded(DiagramNode diagramNode, ILayoutAction causingAction);
        void OnDiagramNodeRemoved(DiagramNode diagramNode, ILayoutAction causingAction);
        void OnDiagramConnectorAdded(DiagramConnector diagramConnector, ILayoutAction causingAction);
        void OnDiagramConnectorRemoved(DiagramConnector diagramConnector, ILayoutAction causingAction);
    }
}