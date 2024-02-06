using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private TMP_Text textHolder;
        
        [Header("Text Options")]
        [SerializeField] private string input;

        [Header("Time Parameters")]
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;

        [Header("Display Image")]
        [SerializeField] private Image storyImage;

        void Awake() 
        {
            textHolder = GetComponent<TMP_Text>();
            textHolder.text = "";
            storyImage.preserveAspect = true;
        }

        private void Start() 
        {
            StartCoroutine(WriteText(input, textHolder, delay, delayBetweenLines));
        }
    }
}
