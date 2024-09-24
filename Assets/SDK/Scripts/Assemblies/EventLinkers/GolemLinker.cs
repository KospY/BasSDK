using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/SkillEventLinker")]
    [AddComponentMenu("ThunderRoad/Golem Linker")]
    public class GolemLinker : EventLinker
    {
        public enum GolemEvent
        {
            OnLinkerStart = 0,
            OnGolemSpawn = 1,
            OnGolemWake = 2,
            OnGolemMeleeAttack = 3,
            OnGolemMagicAttack = 4,
            OnGolemDealDamage = 5,
            OnGolemRampage = 6,
            OnGolemCrystalBreak = 7,
            OnArenaCrystalBreak = 8,
            OnGolemStaggered = 9,
            OnGolemStunned = 10,
            OnGolemDefeated = 11,
            OnGolemKilled = 12,
        }

        [System.Serializable]
        public class GolemUnityEvent
        {
            public GolemEvent golemEvent;
            public EventTime eventTime;
            public Vector2Int crystalsLeft;
            public UnityEvent<Golem> onActivate;

            public GolemUnityEvent Copy()
            {
                return new GolemUnityEvent()
                {
                    golemEvent = this.golemEvent,
                    eventTime = this.eventTime,
                    onActivate = this.onActivate,
                };
            }
        }

        public List<GolemUnityEvent> golemEvents = new List<GolemUnityEvent>();


        public override void UnsubscribeNamedMethods()
        {
        }

        //Rampage = 0,
        //SwingRight = 1,
        //SwingLeft = 2,
        //ComboSwing = 3,
        //ComboSwingAndSlam = 4,
        //SwingBehindRight = 5,
        //SwingBehindLeft = 6,
        //SwingBehindRightTurnBack = 7,
        //SwingBehindLeftTurnBack = 8,
        //SwingLeftStep = 9,
        //SwingRightStep = 10,
        //Slam = 11,
        //Stampede = 12,
        //Breakdance = 13,
        //SlamLeftTurn90 = 14,
        //SlamRightTurn90 = 15,
        //SwingLeftTurn90 = 16,
        //SwingRightTurn90 = 17,
        //Spray = 18,
        //SprayDance = 19,
        //Throw = 20,
        //Beam = 21,
        //SelfImbue = 22,
        //RadialBurst = 23,
        //ShakeOff = 24,
        //LightShake = 25,
        public void LocalGolemMeleeAttack(int attack)
        {
        }

        public void LocalGolemMagicAttack(int attack)
        {
        }

        public void BreakLocalGolemCrystals(int number)
        {
        }

        public void BreakLocalArenaCrystals(int number)
        {
        }

        public void StaggerLocalGolem(Transform source)
        {
        }

        public void StunLocalGolem(float duration)
        {
        }

        public void DefeatLocalGolem()
        {
        }

    }
}
