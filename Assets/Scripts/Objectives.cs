using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Objectives : MonoBehaviour
{
    public GameObject canvas;
    public Toggle topToggle;
    private List<Toggle> objectives;
    

    // Start is called before the first frame update
    void Start()
    {
        objectives = new List<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {


    }




    public void AddObjective(string text,Image image)
    {
        GameObject toggle = CreateToggle(canvas);
        GameObject bg = CreateBackground(toggle);
        GameObject checkMark = CreateCheckmark(bg);
        GameObject label = CreateLabel(toggle);
        FinalizeToggle(toggle, bg, checkMark, label,text);
    }


    GameObject CreateToggle(GameObject canvas)
    {
        GameObject toggle = new GameObject("Toggle");
        toggle.transform.SetParent(canvas.transform);
        toggle.layer = LayerMask.NameToLayer("UI");
        return toggle;
    }

    GameObject CreateBackground(GameObject toggle)
    {
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(toggle.transform);
        bg.layer = LayerMask.NameToLayer("UI");
        return bg;
    }

    GameObject CreateCheckmark(GameObject bg)
    {
        GameObject chmk = new GameObject("Checkmark");
        chmk.transform.SetParent(bg.transform);
        chmk.layer = LayerMask.NameToLayer("UI");
        return chmk;
    }

    GameObject CreateLabel(GameObject toggle)
    {
        GameObject lbl = new GameObject("Label");
        lbl.transform.SetParent(toggle.transform);
        lbl.layer = LayerMask.NameToLayer("UI");
        return lbl;
    }

    void FinalizeToggle(GameObject toggle, GameObject bg, GameObject chmk, GameObject lbl, string objectiveText)
    {
        Text txt = lbl.AddComponent<Text>();
        txt.text = objectiveText;
        Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        txt.font = arialFont;
        txt.lineSpacing = 1;
        txt.color = new Color(50 / 255, 50 / 255, 50 / 255, 255 / 255);
        RectTransform txtRect = txt.GetComponent<RectTransform>();
        txtRect.anchorMin = new Vector2(0, 0);
        txtRect.anchorMax = new Vector2(1, 1);

        Image chmkImage = chmk.AddComponent<Image>();
        chmkImage.sprite = (Sprite)AssetDatabase.GetBuiltinExtraResource(typeof(Sprite), "UI/Skin/Checkmark.psd");
        chmkImage.type = Image.Type.Simple;


        Image bgImage = bg.AddComponent<Image>();
        bgImage.sprite = (Sprite)AssetDatabase.GetBuiltinExtraResource(typeof(Sprite), "UI/Skin/UISprite.psd");
        bgImage.type = Image.Type.Sliced;
        RectTransform bgRect = txt.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 1);
        bgRect.anchorMax = new Vector2(0, 1);

        Toggle toggleComponent = toggle.AddComponent<Toggle>();
        toggleComponent.transition = Selectable.Transition.ColorTint;
        toggleComponent.targetGraphic = bgImage;
        toggleComponent.isOn = true;
        toggleComponent.toggleTransition = Toggle.ToggleTransition.Fade;
        toggleComponent.graphic = chmkImage;
        toggle.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
    }




}
