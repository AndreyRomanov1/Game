using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEducationButtonScript : MonoBehaviour
{
    private readonly Dictionary<ButtonsEnum, GameObject> buttonObjects = new()
    {
        { ButtonsEnum.F, null },
        { ButtonsEnum.Space, null }
    };

    private void Start()
    {
        InitButtonDict();

        if (Model.IsEducation)
            ShowButtonIcon(ButtonsEnum.Space);
    }

    public void ShowButtonIcon(ButtonsEnum buttonEnum) => buttonObjects[buttonEnum].SetActive(true);

    public void HideButtonIcon(ButtonsEnum buttonEnum) => buttonObjects[buttonEnum].SetActive(false);

    private void InitButtonDict()
    {
        var buttonsFolder = transform.Find("Tools").Find("Buttons");
        foreach (var buttonName in buttonObjects.Keys.ToArray())
            buttonObjects[buttonName] = buttonsFolder.Find($"{Buttons.EnumToName[buttonName]}_button").gameObject;
    }
}