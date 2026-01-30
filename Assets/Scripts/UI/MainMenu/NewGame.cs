using UnityEngine.SceneManagement;

public class NewGame : ButtonBaseClass
{
    public override void OnClick()
    {        
        SceneManager.LoadScene(1);
    }
}