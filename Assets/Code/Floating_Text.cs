using UnityEngine;
using System.Collections;

public class Floating_Text : MonoBehaviour 
{

    private static readonly GUISkin skin = Resources.Load<GUISkin>("GameSkin");

    private GUIContent _Content;
    private IFloating_Text_Positioner _Positioner;
    public string Text { get { return _Content.text; } set { _Content.text = value; } }
    public GUIStyle Style { get; set; }

    public static Floating_Text Show(string Text, string style, IFloating_Text_Positioner positioner)
    {
        var go = new GameObject("Floating Text");
        var floatingText = go.AddComponent<Floating_Text>();
        floatingText.Style = skin.GetStyle(style);
        floatingText._Positioner = positioner;
        floatingText._Content = new GUIContent(Text);
        return floatingText;

    }

    public void OnGUI()
    {
        var position = new Vector2();
        var ContentSize = Style.CalcSize(_Content);

        if (!_Positioner.GetPosition(ref position, _Content, Style.CalcSize(_Content)))
        {
            Destroy(gameObject);
            return;
        }

        GUI.Label(new Rect(position.x, position.y, ContentSize.x, ContentSize.y), _Content, Style);
    }

}
