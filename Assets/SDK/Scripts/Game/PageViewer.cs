using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#if ProjectCore
using Sirenix.OdinInspector;
#endif

namespace BS
{
    public class PageViewer : MonoBehaviour
    {
#if ProjectCore
        [ValueDropdown("GetAllPageGroupsId")]
#endif
        public string pageGroupId;
        public Button buttonPrevious;
        public Button buttonNext;
        public Text title;
        public Text text;
        public Image image;


#if ProjectCore
        protected int currentPage;

        public List<ValueDropdownItem<string>> GetAllPageGroupsId()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            foreach (TextData.PageGroup pageGroup in Catalog.current.GetTextData().pageGroups)
            {
                dropdownList.Add(new ValueDropdownItem<string>(pageGroup.id, pageGroup.id));
            }
            return dropdownList;
        }

        protected void Awake()
        {
            buttonPrevious.onClick.AddListener(delegate { Previous(); });
            buttonNext.onClick.AddListener(delegate { Next(); });
            if (pageGroupId == "Tips") Next();
            else Refresh();
        }

        [Button]
        public void Refresh()
        {
            if (pageGroupId != null)
            {
                TextData.PageGroup pageGroup = Catalog.current.GetTextData().GetPageGroup(pageGroupId);
                title.text = pageGroup.title;
                if (pageGroup != null)
                {
                    if (pageGroupId == "Tips" && GameManager.playerData != null)
                    {
                        text.text = pageGroup.pages[GameManager.playerData.tipIndex].text;
                        image.sprite = pageGroup.pages[GameManager.playerData.tipIndex].sprite;
                        buttonPrevious.gameObject.SetActive(pageGroup.pages.Count > 1 ? true : false);
                        buttonNext.gameObject.SetActive(pageGroup.pages.Count > 1 ? true : false);
                    }
                    else if (pageGroupId == "Inputs")
                    {
                        text.text = pageGroup.pages[(int)PlayerControl.controller].text;
                        image.sprite = pageGroup.pages[(int)PlayerControl.controller].sprite;
                        buttonPrevious.gameObject.SetActive(false);
                        buttonNext.gameObject.SetActive(false);
                    }
                    else
                    {
                        text.text = pageGroup.pages[currentPage].text;
                        image.sprite = pageGroup.pages[currentPage].sprite;
                        buttonPrevious.gameObject.SetActive(pageGroup.pages.Count > 1 ? true : false);
                        buttonNext.gameObject.SetActive(pageGroup.pages.Count > 1 ? true : false);
                    }
                }
                else
                {
                    text.text = Catalog.current.GetString("Unknown");
                    image.sprite = null;
                    buttonPrevious.gameObject.SetActive(false);
                    buttonNext.gameObject.SetActive(false);
                }
            }
            else
            {
                text.text = Catalog.current.GetString("Unknown");
                image.sprite = null;
                buttonPrevious.gameObject.SetActive(false);
                buttonNext.gameObject.SetActive(false);
            }
            text.gameObject.SetActive(text.text != null && text.text != "" ? true : false);
            image.gameObject.SetActive(image.sprite != null ? true : false);
        }

        [Button]
        public void Next()
        {
            TextData.PageGroup pageGroup = Catalog.current.GetTextData().GetPageGroup(pageGroupId);
            if (pageGroupId == "Tips" && GameManager.playerData != null)
            {
                GameManager.playerData.tipIndex++;
                if (GameManager.playerData.tipIndex > pageGroup.pages.Count - 1) GameManager.playerData.tipIndex = 0;

            }
            else
            {
                currentPage++;
                if (currentPage > pageGroup.pages.Count - 1) currentPage = 0;
            }
            Refresh();
        }

        [Button]
        public void Previous()
        {
            TextData.PageGroup pageGroup = Catalog.current.GetTextData().GetPageGroup(pageGroupId);
            if (pageGroupId == "Tips" && GameManager.playerData != null)
            {
                GameManager.playerData.tipIndex--;
                if (GameManager.playerData.tipIndex < 0) GameManager.playerData.tipIndex = pageGroup.pages.Count - 1;

            }
            else
            {
                currentPage--;
                if (currentPage < 0) currentPage = pageGroup.pages.Count - 1;
            }
            Refresh();
        }
#endif
    }
}
