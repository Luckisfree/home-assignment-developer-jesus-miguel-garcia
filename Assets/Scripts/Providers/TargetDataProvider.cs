using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TargetDataProvider;

public enum TargetType
{
    TargetsPlayer,
    TargetsClosestEnemy
}
public class TargetInfo
{
    public CharacterStateController target;
    public Vector2 direction;

    public bool HasValidTarget()
    {
        return IsTargetSet() &&
           !target.HealthComponent.IsDying();
    }

    public bool IsTargetSet()
    {
        return target != null;
    }

    public void Reset()
    {
        target = null;
        direction = Vector2.zero;
    }
}

public class TargetDataProvider : MonoBehaviour
{
    public struct CacheClosestEnemyTargetData
    {
        public CharacterStateController closestEnemy;
        public float closestDistanceSqr;
        public Vector2 positionVector2;
        public Vector2 targetPositionVector2;
        public Vector2 cacheDirection;
        public float distanceSqr;

        public void Reset()
        {
            closestEnemy = null;
            closestDistanceSqr = float.MaxValue;
            positionVector2 = Vector2.zero;
            targetPositionVector2 = Vector2.zero;
            cacheDirection = Vector2.zero;
            distanceSqr = float.MaxValue;
        }
    }

    [SerializeField] TargetType _targetType;

    private TargetInfo _targetInfo = new TargetInfo();
    private CacheClosestEnemyTargetData _cacheClosestEnemyTargetData = new CacheClosestEnemyTargetData();

    public TargetInfo GetClosestTargetFromPosition(Vector3 position)
    {
        _cacheClosestEnemyTargetData.Reset();
        switch (_targetType)
        {
            case TargetType.TargetsPlayer:
                {
                    _targetInfo.target = ServiceLocator.Instance.GetCharactersData().GetPlayer();
                    TargetsPlayerLogic.GetClosestTargetFromPosition(_targetInfo, 
                                                                    position,
                                                                    _targetInfo.target.transform.position,
                                                                    _cacheClosestEnemyTargetData);
                    return _targetInfo;
                   
                }
            case TargetType.TargetsClosestEnemy:
                 {
                    TargetsClosestEnemyLogic.GetClosestTargetFromPosition(_targetInfo, 
                                                                          position, 
                                                                          _cacheClosestEnemyTargetData,
                                                                          ServiceLocator.Instance.GetCharactersData().GetEnemies());
                    return _targetInfo;
                }
            default: break;
        }
        return null;
    }
}

public static class TargetsPlayerLogic
{
    public static void GetClosestTargetFromPosition(TargetInfo distanceInfo, Vector3 currentPosition, Vector3 targetPosition, CacheClosestEnemyTargetData cacheClosestEnemyTargetData)
    {
        cacheClosestEnemyTargetData.positionVector2.Set(currentPosition.x, currentPosition.z);
        cacheClosestEnemyTargetData.targetPositionVector2.Set(targetPosition.x, (targetPosition.z));
        distanceInfo.direction = (cacheClosestEnemyTargetData.targetPositionVector2 - cacheClosestEnemyTargetData.positionVector2);
    }
}

public static class TargetsClosestEnemyLogic
{
    public static void GetClosestTargetFromPosition(TargetInfo distanceInfo, Vector3 position, CacheClosestEnemyTargetData cacheClosestEnemyTargetData, List<CharacterStateController> enemies)
    {
        cacheClosestEnemyTargetData.positionVector2.Set(position.x, position.z);
        foreach (var enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }
            cacheClosestEnemyTargetData.targetPositionVector2.Set(enemy.transform.position.x, (enemy.transform.position.z));
            cacheClosestEnemyTargetData.cacheDirection = (cacheClosestEnemyTargetData.targetPositionVector2 - cacheClosestEnemyTargetData.positionVector2);
            cacheClosestEnemyTargetData.distanceSqr = cacheClosestEnemyTargetData.cacheDirection.sqrMagnitude;

            if (cacheClosestEnemyTargetData.distanceSqr < cacheClosestEnemyTargetData.closestDistanceSqr)
            {
                cacheClosestEnemyTargetData.closestDistanceSqr = cacheClosestEnemyTargetData.distanceSqr;
                cacheClosestEnemyTargetData.closestEnemy = enemy;
                distanceInfo.direction = cacheClosestEnemyTargetData.cacheDirection;
            }
        }
        distanceInfo.target = cacheClosestEnemyTargetData.closestEnemy;
    }
}

