using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharactersData
{
    private List<CharacterStateController> _enemies = new List<CharacterStateController>();
    private CharacterStateController _player;

    public event Action<Vector3> OnTargetDefeated;

    public CharactersData()
    {
        _player = null;
        _enemies = new List<CharacterStateController>();
    }

    public void SetEnemiesTarget(CharacterStateController player)
    {
        _player = player;
    }

    public CharacterStateController GetPlayer()
    {
        return _player;
    }

    public List<CharacterStateController> GetEnemies()
    {
        return _enemies;
    }

    public void AddEnemy(CharacterStateController enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(CharacterStateController enemy)
    {
        _enemies.Remove(enemy);
        OnTargetDefeated?.Invoke(enemy.transform.position);
    }

    public void ResetEnemies()
    {
        _player = null;
        _enemies.Clear();
    }
}
