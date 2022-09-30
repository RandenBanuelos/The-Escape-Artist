﻿using UnityEngine;

namespace Impact.Triggers
{
    [AddComponentMenu("Impact/2D Collision Triggers/Impact Collision Trigger 2D", 0)]
    public class ImpactCollisionTrigger2D : ImpactCollisionTriggerBase<ImpactCollisionWrapper, ImpactContactPoint>
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Enabled || (!HighPriority && ImpactManagerInstance.HasReachedPhysicsInteractionsLimit()))
                return;

            ImpactManagerInstance.IncrementPhysicsInteractionsLimit();

            ImpactCollisionWrapper c = new ImpactCollisionWrapper(collision);
            processCollision(c);
        }
    }
}