using System.Collections;
using UnityEngine;
using TMPro;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; private set; }

        protected IEnumerator WriteText(string input, TMP_Text textHolder, float delay, float delayBetweenLines)
        {
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(delayBetweenLines);
            // yield return new WaitUntil(() => ); // fix this to work with new Input System!

            finished = true;
        }
    }
}