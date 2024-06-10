using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SoftwareKingdom.Chess.UI
{
    public interface SeedSelectionPanelListener
    {
        void OnSeedSelected(int seedIndex);
    }

    public class SeedSelectionPanel : MonoBehaviour
    {

        // Settings

        // Connections
        public string[] seedNames;
        public Sprite[] seedIcons;
        public SeedSelectionPanelListener listener;
        public Image[] targetImages;
        // State variables
        Dictionary<string, Sprite> seedNameIconDictionary;
        void Awake()
        {
            InitConnections();
        }

        void Start()
        {
            InitState();
        }

        void InitConnections()
        {
            seedNameIconDictionary = new Dictionary<string, Sprite>();

            for (int i = 0; i < seedNames.Length; i++)
            {
                seedNameIconDictionary.Add(seedNames[i], seedIcons[i]);
            }

        }

        void InitState()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetListener(SeedSelectionPanelListener listener)
        {
            this.listener = listener;
        }

        public void SetSeedsDisplay(string[] seedsAvailable)
        {
            for(int i=0; i<seedsAvailable.Length; i++)
            {
                targetImages[i].gameObject.SetActive(true);
                Sprite seedIcon = seedNameIconDictionary[seedsAvailable[i]];
                targetImages[i].sprite = seedIcon;
            }
        }

        public void SetNumberOfSeeds(int seeds)
        {

        }


        public void OnSeedButtonClicked(int seedIndex)
        {
            listener.OnSeedSelected(seedIndex);
        }
    }
}


