// Author: Justin Broughal
// Course Project: AI Dodgeball (PrisonDodgeball)
// Description: Finite State Machine / Throw Decision Logic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using GameAI;


namespace GameAIStudent
{

    public class ShotSelection
    {

        public const string StudentName = "Justin Broughal";


        public enum SelectThrowReturn
        {
            DoThrow,
            NoThrowTargettingFailed,
            NoThrowOpponentCurrentlyAccelerating,
            NoThrowOpponentWillAccelerate,
            NoThrowOpponentOccluded
        }

        public static SelectThrowReturn SelectThrow(
                // the minion doing the throwing, can also be used to query generic params true of all minions
                MinionScript thisMinion,
                // info about the target
                PrisonDodgeballManager.OpponentInfo opponent,
                // What is the navmask that defines where on the navmesh the opponent can traverse
                int opponentNavmask,
                // typically this is a value a tiny bit smaller than the radius of minion added with radius of the dodgeball
                float maxAllowedThrowErrDist,
                // Time since last frame
                float deltaT,
                // Output param: The solved projectileDir for ballistic trajectory that intercepts target                
                out Vector3 projectileDir,
                // Output param: The speed the projectile is launched at in projectileDir such that
                // there is a collision with target. projectileSpeed must be <= maxProjectileSpeed
                out float projectileSpeed,
                // Output param: The time at which the projectile and target collide
                out float interceptT,
                // Output param: where the shot is expected to hit
                out Vector3 interceptPos
            )
        {
            var Mgr = PrisonDodgeballManager.Instance;

            projectileDir = Vector3.zero;
            projectileSpeed = 0f;
            interceptT = 0f;
            interceptPos = opponent.Pos;

            Vector3 opponentVel = opponent.Vel;

            // see if throw is even possible, before deciding whether to actually do it
            if (!ThrowMethods.PredictThrow(thisMinion.HeldBallPosition, thisMinion.ThrowSpeed, Physics.gravity, opponent.Pos,
                opponentVel, opponent.Forward, maxAllowedThrowErrDist,
                out projectileDir, out projectileSpeed, out interceptT, out float altT))
            {
                return SelectThrowReturn.NoThrowTargettingFailed;
            }

            interceptPos = opponent.Pos + opponentVel * Mathf.Max(0f, interceptT);

            float speed = opponentVel.magnitude;
            bool nearlyStopped = speed < 0.1f;
            float facingVsMove = 0f;
            if(speed > 1e-4f)
                facingVsMove = Vector3.Angle(opponent.Forward, opponentVel.normalized);
            bool motionMisaligned = facingVsMove > 35f;

            if(nearlyStopped || motionMisaligned)
                return SelectThrowReturn.NoThrowOpponentCurrentlyAccelerating;

            NavMeshHit nmHit;
            if (NavMesh.Raycast(opponent.Pos, interceptPos, out nmHit, opponentNavmask))
                return SelectThrowReturn.NoThrowOpponentWillAccelerate;
            
            

            // carverMask exclusion only needed for AdvancedMinionTestThrowScenario
            int carverMask = ~(1 << Mgr.NavMeshCarverLayerIndex);
            // We don't care about minion hits from raycast. Self hits should already be avoided but will filter all minions.
            // And the whole point of the throw is to hit the opponent minion, so we don't want a raycast hit stopping us.
            int minionMask = ~(1 << Mgr.MinionTeamBLayerIndex) & ~(1 << Mgr.MinionTeamALayerIndex);
            // Ignore dodgeballs. They'll most likely be out of the way before they collide
            int ballMask = ~(1 << Mgr.BallTeamALayerIndex) & ~(1 << Mgr.BallTeamBLayerIndex);
            int mask = Physics.AllLayers & carverMask & ballMask & minionMask;

            Vector3 launchPos = thisMinion.HeldBallPosition;
            Vector3 toHit = interceptPos - launchPos;
            float dist = toHit.magnitude;
            if (dist > 1e-3f)
            {
                Vector3 dir = toHit / dist;

                if (Physics.Raycast(launchPos, dir, out RaycastHit hit, dist * .995f, mask, QueryTriggerInteraction.Ignore))
                    return SelectThrowReturn.NoThrowOpponentOccluded;
            }

            thisMinion.FaceTowardsForThrow(interceptPos);

            return SelectThrowReturn.DoThrow;
        }



    }


}