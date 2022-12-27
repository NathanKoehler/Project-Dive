﻿using MyBox;
using Phys;
using UnityEngine;

namespace Mechanics {
    public class Breakable : Solid {
        public override bool Collidable() {
            return true;
        }

        public override bool PlayerCollide(PlayerActor p, Vector2 direction) {
            if (p.IsDiving() && direction.y < 0) {
                gameObject.SetActive(false);
                return false;
            }
            
            return base.PlayerCollide(p, direction);
        }

        public override bool IsGround(PhysObj whosAsking) {
            PlayerActor p = whosAsking.GetComponent<PlayerActor>();
            if (p != null) {
                return !p.IsDiving();
            }
            return true;
        }
    }
}