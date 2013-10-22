using UnityEngine;
using System.Collections;
 
public class LoadingScreen : MonoBehaviour
{
    public Texture2D texture;
    static LoadingScreen instance;
 
	void Update() {
				
	}
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
         hide();
            return;
        }
        instance = this;    
		gameObject.AddComponent<GUIText>().enabled = false;
		guiText.text = "Loading ...";
		guiText.fontSize = 40;
		guiText.anchor = TextAnchor.MiddleCenter;
		guiText.transform.position = new Vector3(0.5f, 0.5f, 1f);
        gameObject.AddComponent<GUITexture>().enabled = false;
        guiTexture.texture = texture;
        transform.position = new Vector3(0.5f, 0.5f, 1f);
        DontDestroyOnLoad(this); 
    }
 
    public static void show()
    {
       if (!InstanceExists()) 
       {
         return;
       }
       instance.guiTexture.enabled = true;
	   instance.guiText.enabled = true;
    }
 
	public static void AddString(string s) {
		if (!InstanceExists()) 
       {
         return;
       }
		instance.guiText.text += "\n" + s;
	}
	
    public static void hide()
    {
       if (!InstanceExists()) 
       {
         return;
       }
       instance.guiTexture.enabled = false;
		instance.guiText.enabled = false;
    }
 
    static bool InstanceExists()
    {
        if (!instance)
       {
         return false;
       }
       return true;
 
    }
 
}
