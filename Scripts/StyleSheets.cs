using UnityEngine;

namespace ArmouryOfTheExpanse
{
    public static partial class Main
    {

        public static void Bullet(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("•", GUILayout.Width(15));
            GUILayout.Label(text, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
        }


        public static void InitGUIStyle()
        {
            //onGui draw every frame; I'm trying to cache the header style
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(GUI.skin.label);
                headerStyle.fontSize = 25;
                headerStyle.fontStyle = FontStyle.Bold;
            }

            //same for the vertical box/div behind
            if (panelStyle == null)
            {
                panelTexture = new Texture2D(1, 1);
                panelTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.6f)); // black, 60% opacity
                panelTexture.Apply();

                panelStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = panelTexture },
                    padding = new RectOffset(12, 12, 12, 12)
                };
            }
        }
 
    } 

}

