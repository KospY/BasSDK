using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIAttributeGroupDisplay : MonoBehaviour
    {
        [SerializeField] private RawImage backgroundImage;
        [SerializeField] private UIText title;
        [SerializeField] private UIText description;
        [SerializeField] private HorizontalOrVerticalLayoutGroup iconLayout;
        [SerializeField] private List<RawImage> iconPool;
        [SerializeField] private GameObject separator;

        private List<(Texture iconCache, Color iconColor)> icons;
        public float iconAspectRatio;
        private List<(Texture iconCache, Color iconColor)> bgIcons;

        private int iconProgession;

        private bool iconSet = true;

        private void Awake()
        {
            ClearIcons();
            backgroundImage.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
            description.gameObject.SetActive(false);
            separator.SetActive(false);
            iconLayout.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ClearIcons();
            iconPool.Clear();
            iconPool = null;
        }

        private void OnEnable()
        {
            if (!iconSet)
            {
                StartCoroutine(SetIconCoroutine());
            }
        }

        public UIAttributeGroupDisplay WithBackground(Texture backgroundImage, Color color, float aspectRatio = 1.0f)
        {
            this.backgroundImage.texture = backgroundImage;
            this.backgroundImage.color = color;
            if (aspectRatio != 1.0f
                && this.backgroundImage.TryGetComponent<AspectRatioFitter>(out AspectRatioFitter aspectRatioFitter))
            {
                aspectRatioFitter.aspectRatio = aspectRatio;
            }

            this.backgroundImage.gameObject.SetActive(true);
            return this;
        }

        /// <summary>
        /// Set the attribute display title.
        /// </summary>
        public UIAttributeGroupDisplay WithTitle(string textId, string groupId = TextData.DEFAULT_TEXT_GROUP)
        {
            title.textGroupId = groupId;
            title.text = "{" + textId + "}";
            title.gameObject.SetActive(true);
            return this;
        }

        /// <summary>
        /// Set the attribute description.
        /// </summary>
        public UIAttributeGroupDisplay WithDescription(string textId, string groupId = TextData.DEFAULT_TEXT_GROUP)
        {
            description.textGroupId = groupId;
            description.text = "{" + textId + "}";
            description.gameObject.SetActive(true);
            return this;
        }

        /// <summary>
        /// spawn thhe number of icons
        /// </summary>
        public UIAttributeGroupDisplay WithIcons(Texture iconTexture, Color iconColor, int count = 1, float iconAspectRatio  = 1.0f)
        {
            if (iconTexture == null) return this;
            
            List<(Texture iconCache, Color iconColor)> tempList = new List<(Texture iconCache, Color iconColor)>();
            for (int i = 0; i < count; i++)
            {
                tempList.Add((iconTexture, iconColor));
            }

            return WithIcons(tempList, iconAspectRatio);
        }

        /// <summary>
        /// spawn thhe list of icons
        /// </summary>
        public UIAttributeGroupDisplay WithIcons(List<(Texture iconCache, Color iconColor)> icons, float iconAspectRatio  = 1.0f)
        {
            if (icons == null)
            { return this; }

            iconLayout.gameObject.SetActive(true);
            this.icons = icons;
            this.iconAspectRatio = iconAspectRatio;
            this.bgIcons = null;
            iconProgession = this.icons.Count;

            SetIconCount();
            return this;
        }

        /// <summary>
        /// Spawns Icons from the list (0 to iconProgression) and then from bgIcons list (iconProgression to size of bgIcons list)
        /// icons and bgIcons must be the same size.
        /// </summary>
        public UIAttributeGroupDisplay WithProgressionIcons(List<(Texture iconCache, Color iconColor)> icons, List<(Texture iconCache, Color iconColor)> bgIcons, int iconProgession, float iconAspectRatio = 1.0f)
        {
            if (icons == null || bgIcons == null)
            { return this; }

            iconLayout.gameObject.SetActive(true);
            this.icons = icons;
            this.iconAspectRatio = iconAspectRatio;
            this.bgIcons = bgIcons;
            this.iconProgession = iconProgession;

            SetIconCount();
            return this;
        }

        /// <summary>
        /// Spawn same Icons and same background Icons according to the number of icon and the progression.
        /// </summary>
        /// <param name="icon">the icon texture</param>
        /// <param name="color">the icon color</param>
        /// <param name="bgIcon"> the icon background (to see that icons are missing / progression)</param>
        /// <param name="bgColor"> the icon bakcground color</param>
        /// <param name="count"> max icons (number of background to show = count - progression)</param>
        /// <param name="progression"> icon  progression (number of icons to show)</param>
        /// <param name="aspectRatio">the aspect ratio of icons (they must ahve the same)</param>
        /// <returns></returns>
        public UIAttributeGroupDisplay WithProgressionIcons(Texture icon, Color color, Texture bgIcon, Color bgColor, int count = 1, int progression = 1, float iconAspectRatio = 1.0f)
        {
            if (icon == null || bgIcon == null)
            { return this; }

            List<(Texture iconCache, Color iconColor)> tempListIcons = new List<(Texture iconCache, Color iconColor)>();
            List<(Texture iconCache, Color iconColor)> tempListBgIcons = new List<(Texture iconCache, Color iconColor)>();
            for (int i = 0; i < count; i++)
            {
                tempListIcons.Add((icon, color));
                tempListBgIcons.Add((bgIcon, bgColor));
            }

            return WithProgressionIcons(tempListIcons, tempListBgIcons, progression, iconAspectRatio);
        }

        /// <summary>
        /// Set active the separator.
        /// </summary>
        public UIAttributeGroupDisplay WithSeparator()
        {
            separator.SetActive(true);
            return this;
        }

        /// <summary>
        /// Set the amount of icons to display.
        /// </summary>
        private void SetIconCount()
        {
            iconSet = false;

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(SetIconCoroutine());
            }
        }

        private System.Collections.IEnumerator SetIconCoroutine()
        {
            yield return null;

            RectTransform layoutTransform = iconLayout.transform as RectTransform;
            float iconHeight = layoutTransform.sizeDelta.y - (iconLayout.padding.top + iconLayout.padding.bottom);
            int totalIconCount = bgIcons.IsNullOrEmpty() ? icons.Count : bgIcons.Count;
            float iconWidth = (layoutTransform.sizeDelta.x - (iconLayout.padding.left + iconLayout.padding.right + (iconLayout.spacing * (totalIconCount - 1)))) / (float)totalIconCount;

            ClearIcons();

            // Check there is a template to create Icons
            if (iconPool == null || iconPool.Count == 0)
            {
                Debug.LogError("No Icon model present in the icon pool");
                iconSet = true;

                yield break;
            }


            // Create icons
            AspectRatioFitter.AspectMode aspectMode;
            float scale = 1.0f;

            if (iconHeight > iconWidth)
            {
                aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                iconLayout.childControlHeight = false;
                iconLayout.childControlWidth = true;
                iconLayout.childForceExpandHeight = false;
                iconLayout.childForceExpandWidth = true;
                float height = iconWidth / iconAspectRatio;
                if (height > iconHeight)
                {
                    scale = iconHeight / height;
                }
            }
            else
            {
                aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                iconLayout.childControlHeight = true;
                iconLayout.childControlWidth = false;
                iconLayout.childForceExpandHeight = true;
                iconLayout.childForceExpandWidth = false;
                float width = iconHeight * iconAspectRatio;
                if (width > iconWidth)
                {
                    scale = iconWidth / width;
                }
            }

            int poolCount = iconPool.Count;
            for (int index = 0; index < iconProgession; index++)
            {
                RawImage tempImage = index < poolCount ? iconPool[index]
                                                            : CreateIcon();
                tempImage.texture = icons[index].iconCache;
                tempImage.color = icons[index].iconColor;

                if (tempImage.TryGetComponent(out AspectRatioFitter ratioFitter))
                {
                    ratioFitter.aspectRatio = iconAspectRatio;
                    ratioFitter.aspectMode = aspectMode;
                    tempImage.transform.localScale = Vector3.one * scale;
                }

                tempImage.gameObject.SetActive(true);
            }

            // if icon progression is less than total icons then create icons background
            if (iconProgession < totalIconCount)
            {
                float scaleBgProgression = 1.0f;

                if (iconHeight > iconWidth)
                {
                    float height = iconWidth / iconAspectRatio;
                    if (height > iconHeight)
                    {
                        scaleBgProgression = iconHeight / height;
                    }
                }
                else
                {
                    float width = iconHeight * iconAspectRatio;
                    if (width > iconWidth)
                    {
                        scaleBgProgression = iconWidth / width;
                    }
                }

                for (int index = iconProgession; index < totalIconCount; index++)
                {
                    RawImage tempImage = index < poolCount ? iconPool[index]
                                                                : CreateIcon();
                    tempImage.texture = bgIcons[index].iconCache;
                    tempImage.color = bgIcons[index].iconColor;

                    if (tempImage.TryGetComponent(out AspectRatioFitter ratioFitter))
                    {
                        ratioFitter.aspectRatio = iconAspectRatio;
                        ratioFitter.aspectMode = aspectMode;
                        tempImage.transform.localScale = Vector3.one * scaleBgProgression;
                    }

                    tempImage.gameObject.SetActive(true);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutTransform);

            iconSet = true;
        }

        /// <summary>
        /// Clears all currently listed icons.
        /// </summary>
        private void ClearIcons()
        {
            if (iconPool == null) return;

            int poolCount = iconPool.Count;
            for (int i = 0; i < poolCount; i++)
            {
                iconPool[i].texture = null;
                iconPool[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Create an icon from the cache address.
        /// </summary>
        private RawImage CreateIcon()
        {
            RawImage newIcon = Instantiate(iconPool[0], iconLayout.transform);
            iconPool.Add(newIcon);
            return newIcon;
        }
    }
}