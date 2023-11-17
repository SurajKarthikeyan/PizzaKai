using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/Rule Tile/Platform Rule Tile")]
public class PlatformRuleTile : RuleTile<PlatformRuleTile.Neighbor>
{
    public enum Options
    {
        /// <summary>
        /// Includes both left and right facing platforms.
        /// </summary>
        LeftAndRight,

        /// <summary>
        /// Includes only left facing platforms.
        /// </summary>
        LeftOnly,

        /// <summary>
        /// Includes only right facing platforms.
        /// </summary>
        RightOnly
    }

    public Options options;

    private IEnumerable<Sprite> leftSprites;

    private IEnumerable<Sprite> rightSprites;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int LeftTag = 3;
        public const int RightTag = 4;
        public const int Other = 5;
        public const int ThisStrict = 6;
        public const int Null = 7;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap,
        GameObject instantiatedGameObject)
    {
        GenerateSpriteLists();

        return base.StartUp(position, tilemap, instantiatedGameObject);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        GenerateSpriteLists();

        base.RefreshTile(position, tilemap);
    }

    private void GenerateSpriteLists()
    {
        leftSprites = m_TilingRules.Where(
            tr =>
                tr.m_GameObject &&
                tr.m_GameObject.CompareTag("LeftPlatform")
        ).Select(
            tr => tr.m_Sprites.FirstOrDefault()
        );

        rightSprites = m_TilingRules.Where(
            tr =>
                tr.m_GameObject &&
                tr.m_GameObject.CompareTag("RightPlatform")
        ).Select(
            tr => tr.m_Sprites.FirstOrDefault()
        );
    }

    public override bool RuleMatches(TilingRule rule, Vector3Int position,
        ITilemap tilemap, ref Matrix4x4 transform)
    {
        // Only determines if the platform section is valid.
        bool platformValid = false;

        foreach (var neighborKVP in rule.GetNeighbors())
        {
            if (neighborKVP.Value == Neighbor.LeftTag ||
                neighborKVP.Value == Neighbor.RightTag)
            {
                for (int y = 0; y < 1024; y++)
                {
                    var pos = neighborKVP.Key + position + new Vector3Int(0, y);
                    var sprite = tilemap.GetSprite(pos);

                    // Not interested in other tiles.
                    if (!(sprite && tilemap.GetTile(pos) is PlatformRuleTile))
                        break;
                        
                    if (neighborKVP.Value == Neighbor.LeftTag)
                    {
                        if (leftSprites == null)
                            GenerateSpriteLists();

                        if (leftSprites.Contains(sprite))
                        {
                            // Found the correct sprite. This rule tile
                            // thing lies below a left-side platform. Break.
                            platformValid = true;
                            break;
                        }
                    }
                    else
                    {
                        if (rightSprites == null)
                            GenerateSpriteLists();

                        if (rightSprites.Contains(sprite))
                        {
                            // Found the correct sprite. This rule tile
                            // thing lies below a right-side platform.
                            // Break.
                            platformValid = true;
                            break;
                        }
                    }

                }

                if (!platformValid)
                {
                    // Return false if no sprites found.
                    return false;
                }
            }
        }

        return base.RuleMatches(rule, position, tilemap, ref transform);
    }

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        switch (neighbor)
        {
            case TilingRuleOutput.Neighbor.NotThis:
                return other is not PlatformRuleTile;
            case TilingRuleOutput.Neighbor.This:
                return other is PlatformRuleTile;
            case Neighbor.Other:
                return other != null && other is not PlatformRuleTile;
            case Neighbor.ThisStrict:
                return other == this
                    || (other is PlatformRuleTile prt
                    && prt.options == Options.LeftAndRight);
            case Neighbor.Null:
                return other == null;
            default:
                return base.RuleMatch(neighbor, other);
        }
    }
}