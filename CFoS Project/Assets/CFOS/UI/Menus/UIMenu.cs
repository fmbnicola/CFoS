using CFoS.Experimentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CFoS.UI
{
    public class UIMenu : MonoBehaviour
    {
        public UIMenu ParentMenu { get; set; } = null;
        public HashSet<UIMenu> ChildMenus { get; set; } = null;

        public bool Hidden { get; set; } = false;

        public float DelayTime = 0.25f;

        // Init
        protected void Awake()
        {
            ChildMenus = new HashSet<UIMenu>();
        }

        // Hide and Show menu
        protected void SetVisibility(bool val)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(val);
            }
            Hidden = !val;
        }
        public void Show() { SetVisibility(true); }
        public void Hide() { SetVisibility(false); }


        // Open Sub-menu
        public void OpenSubMenu(GameObject subMenuObj)
        {
            // spawn sub-menu in the parent menu position
            GameObject instance = Instantiate(subMenuObj, UIManager.Instance.transform);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;

            // set child/parent relation-ship for menu and then hide parent
            var submenu = instance.GetComponent<UIMenu>();
            if (submenu != null)
            {
                submenu.ParentMenu = this;
                ChildMenus.Add(submenu);
            }
            Hide();
        }

        public void OpenSubMenuDelayed(GameObject subMenuObj)
        {
            StartCoroutine(DoOpenSubMenuDelayed(subMenuObj, DelayTime));
        }

        protected IEnumerator DoOpenSubMenuDelayed(GameObject subMenuObj, float time)
        {
            yield return new WaitForSeconds(time);
            OpenSubMenu(subMenuObj);
        }


        // Close menu
        public void CloseMenu()
        {
            // if menu has children, make sure they are destroyed
            if (ChildMenus.Count != 0)
            {
                foreach(var menu in ChildMenus)
                {
                    Destroy(menu.gameObject);
                }
            }
            ChildMenus.Clear();

            // if menu has parent remove self and Show it
            if(ParentMenu != null)
            {
                ParentMenu.ChildMenus.Remove(this);
                ParentMenu.Show();
            }

            Destroy(gameObject);
        }

        public void CloseMenuDelayed()
        {
            StartCoroutine(DoCloseMenuDelayed(DelayTime));
        }

        protected IEnumerator DoCloseMenuDelayed(float time)
        {
            yield return new WaitForSeconds(time);
            CloseMenu();
        }


        // Load Experiment
        public void LoadExperiment(string experimentName)
        {
            UIManager.Instance.CloseMainMenu();
            ExperimentManager.Instance.LoadExperiment(experimentName);
        }

        public void LoadExperiment(int experimentIndex)
        {
            UIManager.Instance.CloseMainMenu();
            ExperimentManager.Instance.LoadExperiment(experimentIndex);
        }

        public void LoadExperiment(Experiment experiment)
        {
            var name = experiment.gameObject.name;
            LoadExperiment(name);
        }

        public void NextTask()
        {
            ExperimentManager.Instance.NextTask();
        }
    }
}