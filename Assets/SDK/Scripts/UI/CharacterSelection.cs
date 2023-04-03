using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class CharacterSelection : MonoBehaviour
    {
        [Serializable]
        public class CharacterSlot
        {
            public SpriteRenderer rune;
            public Transform select;
        }

        public CharacterSlot characterSlot;
        private int indexCharacter = 0;

        public Transform playerStart;

        public GameObject canvas = null;

        public GameObject messagePage = null;
        public GameObject optionsPage = null;
        public GameObject modManager = null;
        public GameObject loadingBarScreen = null;
        public GameObject characterSelectionPage = null;
        public GameObject gameModeSelectionPage = null;
        public GameObject customisationPage = null;
        public GameObject calibrationPage = null;
        public GameObject mapSelectionPage = null;
        public GameObject deleteMessagePage = null;
        public GameObject tutorialMessagePage = null;
        public GameObject characterMovementUI;
        public GameObject optionsMenuAnchor;

        
        public TextMeshProUGUI loadingBarStage = null;
        public TextMeshProUGUI loadingBarPercentage = null;

        public Button customizeButton = null;
        public Button deleteButton = null;
        public Button characterSelectionStartButton = null;
        public GameObject disabledModManagerWarning = null;
        public UICustomisableButton modManagerButton = null;

        public TextMeshProUGUI currentPlaytime = null;
        public TextMeshProUGUI[] currentGameModeText = null;
        public TextMeshProUGUI[] currentPlayerIndexText = null;

        public string validationSound = "";
        public AudioSource uiValidatioAudioSource = null;
        public float validationVolumeDB;
        public string cancelSound = "";
        public AudioSource uiCancelAudioSource = null;
        public float cancelVolumeDB;
        public string switchSound = "";
        public AudioSource uiSwitchAudioSource = null;
        public float switchVolumeDB;
        public string lightSound = "";
        public AudioSource uiLightAudioSource = null;
        public float lightVolumeDB;

        public AnimationClip characterMalePose;
        public AnimationClip characterFemalePose;

        private AudioContainer validationSoundClipContainer = null;
        private AudioContainer cancelSoundClipContainer = null;
        private AudioContainer switchSoundClipContainer = null;
        private AudioContainer lightSoundClipContainer = null;

        private List<Texture2D> loadedTextures = null;

        [Header("GAME MODE SELECTION")]
        public UISelectionListButtonsGameMode gameModeSelectionButton = null;
        private string gameMode = "";

        [Header("CUSTOMIZATION")]
        public GameObject customizerGender = null;
        public GameObject customizer = null;

        [Header("CALIBRATION")]
        public float currentSize = 1.75f;
        public float currentFeet = 0;
        public float currentInch = 0;
        public Transform rootMarker;
        public Transform waistMarker;
        public Transform rightFootBone;
        public Transform rightFootGrip;
        public Transform leftFootBone;
        public Transform leftFootGrip;

        public Button playButton = null;
        public Button calibrateTrackerStartButton = null;

        public UISelectionListButtonsSwitchMeterImperial switchMeterImperial = null;

        [Header("MAP SELECTION")]
        public UISelectionListButtonsMapSelection mapSelectionButton = null;
        public UIMapLevelMode mapSelectionLevelMode = null;

        [Header("Character movement")]
        public Transform characterMinPosition;
        public Transform characterMaxPosition;
        public float characterTranslationSpeed = 2f;
        public float characterRotationSpeed = 100f;


    }
}