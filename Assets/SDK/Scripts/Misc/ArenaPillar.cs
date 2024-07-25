using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Skill.SpellPower;
using UnityEngine;

namespace ThunderRoad
{
    public class ArenaPillar : MonoBehaviour
    {
        public List<Animator> rewardPillarAnimators;
        public AudioSource rewardPillarSound;
        public GameObject rewardPillarFX;
        public Transform spawnPoint;
        public Transform tpPosition;

        public float drivePositionSpring = 100;
        public float drivePositionDamper = 2;
        public float slerpPositionSpring = 200;
        public float slerpPositionDamper = 0;

        private bool _pillArctive;
        private static readonly int GoOut = Animator.StringToHash("GoOut");

        private ConfigurableJoint _linkItemConfigurableJoint = null;
        private Rigidbody _linkItemRigidbody = null;

        private Item _item = null;

        public bool IsPillarActive => _pillArctive;

        public Action pillarUpEvent;
        public Action pillarDownEvent;

        private Action<Item> _itemSpawnCallback;

    }
}