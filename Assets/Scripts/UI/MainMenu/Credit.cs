using UnityEngine.SceneManagement;

public class Credit : ButtonBaseClass
{
    public override void OnClick()
    {
        SceneManager.LoadScene(4);
    }
}