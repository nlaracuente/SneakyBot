using UnityEngine;
using System.Collections;

// Put this script on a Camera
public class DrawLines : MonoBehaviour {

    // Fill/drag these in from the editor

    // Choose the Unlit/Color shader in the Material Settings
    // You can change that color, to change the color of the connecting lines
    public Material lineMat;

    public GameObject[] mainPoints;
    public GameObject[] points;

    //This is so we can set mutliple main points and mutile endpoints.  The indexes must match 1 to 1
    void DrawConnectingLines() {
        if (mainPoints.Length > 0 && points.Length > 0 && mainPoints.Length == points.Length) {
            // Loop through each point to connect to the mainPoint
            //foreach (GameObject point in points) {
            for(int i = 0; i < points.Length; i++) { 
                Vector3 mainPointPos = mainPoints[i].transform.position;
                Vector3 pointPos = points[i].transform.position;

                GL.Begin(GL.LINES);
                lineMat.SetPass(0);
                GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
                GL.Vertex3(mainPointPos.x, mainPointPos.y, mainPointPos.z);
                GL.Vertex3(pointPos.x, pointPos.y, pointPos.z);
                GL.End();
            }
        }
    }

    // To show the lines in the game window whne it is running
    void OnPostRender() {
        DrawConnectingLines();
    }

    // To show the lines in the editor
    void OnDrawGizmos() {
        DrawConnectingLines();
    }
}