using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
    public InputField Input;
    public Button ConfirmButton;
    public int MaxLength = 10;

    public void CheckNameLength()
    {
        if (Input.text.Length > MaxLength)
        {
            Input.text = Input.text.Substring(0, 10);
        }
        NameManager.Instance.Name = Input.text;
        ConfirmButton.interactable = Input.text.Length > 0;
    }
}