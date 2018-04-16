using UnityEngine;
using UnityEditor;
using System.Collections;

public class ObjectGenerationHelper {
	[MenuItem ("Object/Set Border Thickness %&b")]
	public static void CreatePattern () {
        var go = Selection.activeGameObject;
        var top = go.transform.Find("Top");
        var right = go.transform.Find("Right");
        var left  = go.transform.Find("Left");
        var bottom = go.transform.Find("Bottom");

        if (top == null || right == null || left == null || bottom == null) {
            Debug.LogError("not a rect" + top + right + left + bottom);
            return;
        }

        var parentScale = go.transform.localScale;
        // alternative way would be to get the bounds
        var avgThickness = (bottom.localScale.y + top.localScale.y) * parentScale.y +
            (right.localScale.x + left.localScale.x) * parentScale.x;
        avgThickness /= 4f;

        InputBoxWindow.RequestFloat(
                "Border thickness", avgThickness.ToString(),
                delegate(float thickness) {
                    float yScale = thickness /parentScale.y;
                    float xScale = thickness / parentScale.x;
                    float width = parentScale.x;
                    float height = parentScale.y;
                    var parentPos = go.transform.position;
                    top.localScale = top.localScale.SwapY(yScale);
                    top.position = top.position.SwapY(parentPos.y+(height-thickness)/2);
                    bottom.localScale = bottom.localScale.SwapY(yScale);
                    bottom.position = bottom.position.SwapY(parentPos.y-(height-thickness)/2);
                    left.localScale = left.localScale.SwapX(xScale);
                    left.position = left.position.SwapX(parentPos.x-(width-thickness)/2);
                    right.localScale = right.localScale.SwapX(xScale);
                    right.position = right.position.SwapX(parentPos.x+(width-thickness)/2);
                });
    }
}

public delegate void FloatInputDelegate(float input);
public delegate void StringInputDelegate(string input);

public class InputBoxWindow: EditorWindow {

    public FloatInputDelegate floatDelegate;
    public string message;
    public string inputValue;

    public static void RequestFloat(string message, string defaultValue, FloatInputDelegate del) {
        InputBoxWindow window = ScriptableObject.CreateInstance<InputBoxWindow>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.message = message;
        window.floatDelegate = del;
        window.inputValue = defaultValue;
        window.ShowPopup();
        window.Focus();
    }


    void OnGUI() {
        EditorGUILayout.LabelField(message, EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        GUI.SetNextControlName("inputField");
        inputValue = GUILayout.TextField(inputValue);
        if (GUI.GetNameOfFocusedControl() == string.Empty) {
            GUI.FocusControl("inputField");
            EditorGUI.FocusTextInControl("inputField");
        }

        if (GUILayout.Button("Submit") || Event.current.keyCode == KeyCode.Return) {
            if (floatDelegate != null) {
                floatDelegate(float.Parse(inputValue));
            }
            Close();
        }
        if (GUILayout.Button("Cancel")) Close();

    }
}
