using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSingleClick : MonoBehaviour
{
    [SerializeField] Button btn;

    private void Awake()
    {
        btn.onClick.AddListener(NoParamaterOnclick);
    }

    private void NoParamaterOnclick()
    {
        btn.enabled = false;
    }
}
