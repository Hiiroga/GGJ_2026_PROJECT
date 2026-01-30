using UnityEngine.SceneManagement;

public class NewGame : ButtonBaseClass
{
    public override void OnClick()
    {        
        SfxManager.Instance.Play("playbutton");
        SceneManager.LoadScene(1);
    }
}