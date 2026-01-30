using UnityEngine;

public class ExitGame : ButtonBaseClass
{
    public override void OnClick()
    {        
        Application.Quit();
    }
}