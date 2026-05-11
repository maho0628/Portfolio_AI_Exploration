using UnityEngine;

public class TPTextController
    : FloatingTextControllerBase<TPTextController>
{
    public void Play(int tp)
    {
        gameObject.SetActive(true);

        textUI.text =tp.ToString(); 

        PlayAnimation();
    }
}