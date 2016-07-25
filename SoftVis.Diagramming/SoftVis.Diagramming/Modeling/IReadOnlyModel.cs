﻿using System;
using System.Collections.Generic;

namespace Codartis.SoftVis.Modeling
{
    /// <summary>
    /// A read-only view of a model.
    /// A model contains entities and their relationships.
    /// </summary>
    public interface IReadOnlyModel
    {
        IEnumerable<IModelEntity> Entities { get; }
        IEnumerable<IModelRelationship> Relationships { get; }

        event EventHandler<IModelEntity> EntityAdded;
        event EventHandler<IModelRelationship> RelationshipAdded;
        event EventHandler<IModelEntity> EntityRemoved;
        event EventHandler<IModelRelationship> RelationshipRemoved;

        IEnumerable<IModelEntity> GetRelatedEntities(IModelEntity entity, RelationshipSpecification specification);
    }
}