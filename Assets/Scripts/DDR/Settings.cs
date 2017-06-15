using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDR;
public class Settings : MonoBehaviour
{
    [SerializeField] int m_Difficulty;
    [SerializeField] ColorScheme m_ColorScheme;
    [SerializeField] ControlScheme m_ControlsPlayer1;
    [SerializeField] ControlScheme m_ControlsPlayer2;

}


public class ControlScheme
{
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Right;
}