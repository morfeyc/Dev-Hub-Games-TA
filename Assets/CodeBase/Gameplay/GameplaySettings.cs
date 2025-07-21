using System.Linq;
using CodeBase.Gameplay.Features.Avatar;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Gameplay
{
  public class GameplaySettings : ScriptableObject
  {
    public int RoundTime;
    public int TimeBetweenRounds;
    public int NumberOfAICharacters;
    public Vector3 SpawnPosition;
    
    [Button("Get Spawn Pos"), PropertyTooltip("Get spawn position from scene")]
    private void GetSpawnPosBtn()
    {
      // we assume that there is only one active spawn marker at the scene
      SpawnMarker spawnMarker = FindObjectsByType<SpawnMarker>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault();
      if (!spawnMarker)
        return;
      
      SpawnPosition = spawnMarker.transform.position;
    }
  }
}