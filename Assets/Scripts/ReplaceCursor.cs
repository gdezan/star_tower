using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceCursor : MonoBehaviour {
    //The texture that's going to replace the current cursor  
    public Texture2D cursorTexture;

    //This variable flags whether the custom cursor is active or not  
    public bool ccEnabled = false;

    void Start() {
        //Call the 'SetCustomCursor' (see below) with a delay of 2 seconds.   
        Invoke("SetCustomCursor", 0.0f);
    }

    void OnDisable() {
        //Resets the cursor to the default  
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //Set the _ccEnabled variable to false  
        this.ccEnabled = false;
    }

    private void SetCustomCursor() {
        //Replace the 'cursorTexture' with the cursor 
        Vector2 cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(this.cursorTexture, cursorHotspot, CursorMode.Auto);
        Debug.Log("Custom cursor has been set.");
        //Set the ccEnabled variable to true  
        this.ccEnabled = true;
    }
}
