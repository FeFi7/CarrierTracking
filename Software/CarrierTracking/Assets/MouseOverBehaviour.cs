using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverBehaviour : MonoBehaviour
{
    public string MouseOverText;

    Color m_MouseOverColor = Color.blue;
    Color m_StandardColor = Color.red;

    Color labelBackgroundColor;

    string currentToolTipText = "";
    GUIStyle guiStyleFore;
    GUIStyle guiStyleBack;
    Texture2D labelBackgroundTexture;

    MeshRenderer m_Renderer;

    void Start()
    {
        //Hole die aktuelle Mesh Komponente
        m_Renderer = GetComponent<MeshRenderer>();
        //Lege die Standardfarbe fest
        m_Renderer.material.color = m_StandardColor;

        //initialisiere Textobjekt
        guiStyleFore = new GUIStyle();
        guiStyleFore.normal.textColor = Color.white;
        guiStyleFore.fontSize = 12;
        guiStyleFore.alignment = TextAnchor.UpperCenter;
        guiStyleFore.wordWrap = true;
        guiStyleBack = new GUIStyle();
        guiStyleBack.normal.textColor = Color.black;
        guiStyleBack.alignment = TextAnchor.UpperCenter;
        guiStyleBack.wordWrap = true;

        labelBackgroundTexture = new Texture2D(1, 1);
        labelBackgroundColor = Color.black;
        labelBackgroundColor.a = 0.42f;
        labelBackgroundTexture.SetPixel(0, 0, labelBackgroundColor);
        labelBackgroundTexture.Apply();
        guiStyleBack.normal.background = labelBackgroundTexture;
    }

    void OnMouseOver()
    {
        // Wechsel die Farbe bei MouseOver und setze Tooltip Text
        m_Renderer.material.color = m_MouseOverColor;
        currentToolTipText = MouseOverText;
    }

    void OnMouseExit()
    {
        // Wechsel wieder auf Standard Farbe und entferne TooltipText
        m_Renderer.material.color = m_StandardColor;
        currentToolTipText = "";
    }

    void OnGUI()
    {
        if (currentToolTipText != "")
        {
            var x = Event.current.mousePosition.x;
            var y = Event.current.mousePosition.y;
            GUI.Label(new Rect(x - 95, y - 48, 190, 30), "", guiStyleBack);
            GUI.Label(new Rect(x - 150, y - 40, 300, 60), currentToolTipText, guiStyleFore);
            
        }
    }
}
