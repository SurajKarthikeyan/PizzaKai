﻿using NaughtyAttributes;

/// <summary>
/// Use to instantly kill the character this is attached to.
/// This script is only used for testing.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class DieModule : Module
{
    [Button]
    private void GoAndDie()
    {
        Master.Die();
    }
}