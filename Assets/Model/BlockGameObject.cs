using System;
using UnityEngine;

    public class BlockGameObject
    {
        public readonly BlockDirections EntranceDirection;
        public readonly BlockDirectionsNumbers EntranceNumber;
        public readonly BlockDirections ExitDirection;
        public readonly BlockDirectionsNumbers ExitNumber;
        public readonly int Number;
        public readonly GameObject Prefab;

        public BlockGameObject(GameObject prefab)
        {
            var prefabName = prefab.name;
            (EntranceDirection, EntranceNumber, ExitDirection, ExitNumber, Number) = ParsePrefabName(prefabName);
            Prefab = prefab;
        }

        private (BlockDirections, BlockDirectionsNumbers, BlockDirections, BlockDirectionsNumbers, int) ParsePrefabName(
            string prefabName)
        {
            var m = prefabName.Split('_');
            return (GetBlockDirectionByStr(m[0]), GetBlockDirectionNumberByStr(m[1]),
                GetBlockDirectionByStr(m[2]), GetBlockDirectionNumberByStr(m[3]), int.Parse(m[4]));
        }

        private static BlockDirections GetBlockDirectionByStr(string strDirection)
        {
            return strDirection switch
            {
                "Down" => BlockDirections.Down,
                "Right" => BlockDirections.Right,
                "Up" => BlockDirections.Up,
                "Start" => BlockDirections.Start,
                "End" => BlockDirections.End,
                "Base" => BlockDirections.Base,
                _ => throw new ArgumentOutOfRangeException(nameof(strDirection), strDirection, null)
            };
        }

        private static BlockDirectionsNumbers GetBlockDirectionNumberByStr(string strDirectionNumber)
        {
            return strDirectionNumber switch
            {
                "0" => BlockDirectionsNumbers.Zero,
                "1" => BlockDirectionsNumbers.First,
                "2" => BlockDirectionsNumbers.Second,
                "3" => BlockDirectionsNumbers.Third,
                _ => throw new ArgumentOutOfRangeException(nameof(strDirectionNumber), strDirectionNumber, null)
            };
        }
    }
