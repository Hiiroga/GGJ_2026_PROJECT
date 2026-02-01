using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : ButtonBaseClass
{
    public override void OnClick()
    {        
        SfxManager.Instance.Play("playbutton");
        SceneManager.LoadScene(1);
        int resetPrefs = 0;

        PlayerPrefs.SetInt("Score",resetPrefs);
    }
}