﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pilot : MonoBehaviour
{
    public abstract void MakeActions();

    protected static void FireWeapon(Ship ship, int num)
    {
        ship.GetArsenal()?.Fire(num);
    }

    protected static void Brake(Ship ship)
    {

    }

    protected void Rotate(Entity entity, float val)
    {
        entity.GetMovementStats().GetRotationStatGroup().Tick(val); 
    }

    protected void Move(Entity entity, float val)
    {
        entity.GetMovementStats().GetVelocityStatGroup().Tick(val);
    }

    protected void Teleport(Entity entity, Vector2 target)
    {
        entity.transform.localPosition += new Vector3(target.x, target.y);
    }

    /*
    protected ActionAttack attack = new ActionAttack();
    protected IActionMove move;
    protected ActionRotate rotate;

    public virtual void TakeActions()
    {
        var ship = GetComponentInParent<Ship>();
        if (!attack.IsComplete())
        {
            attack.Act(ship);
        }

        move?.Act(ship);
        rotate?.Act(ship);
        
        move = null;
        rotate = null;
    }

    public interface IAction
    {
        string GetName();
        void Act(Ship ship);
    }
    
    public class ActionAttack : IAction
    {
        private bool complete = false;
        public bool IsComplete() { return complete; }

        public void Act(Ship ship)
        {
            complete = true;
        }

        public string GetName()
        {
            return "None";
        }
    }

    public interface IActionMove : IAction { }

    public class ActionMove : IActionMove
    {
        private float val;
        public ActionMove(float val) { this.val = val;  }
        public void Act(Ship ship)
        {
            ship.GetMovementStats().GetVelocityStatGroup().Tick(val);
        }

        public string GetName()
        {
            return string.Format("Move {0}", val);
        }
    }

    public class ActionTeleport : IActionMove
    {
        private Vector2 target;

        public void Act(Ship ship)
        {
            ship.transform.localPosition += new Vector3(target.x, target.y);
        }

        public string GetName()
        {
            return string.Format("Teleport {0}", target);
        }
    }

    public class ActionRotate : IAction
    {
        private float val;
        public ActionRotate(float val) { this.val = val; }
        public void Act(Ship ship)
        {
            ship.GetMovementStats().GetRotationStatGroup().Tick(val);
        }

        public string GetName()
        {
            return string.Format("Rotate {0}", val);
        }
    }
    */
}
