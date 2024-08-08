using Fusion;
using UnityEngine;

public class ButtonShoot : SimulationBehaviour
{
    [Networked] public bool isPressShootButton { get; set; }
    private void Start()
    {
        isPressShootButton = false;
    }
    //Event Button
    public void IsPressShootButton()
    {
        isPressShootButton = !isPressShootButton;
    }
    //Get value
    public bool GetValueShootButton()
    {
        return isPressShootButton;
    }
}
