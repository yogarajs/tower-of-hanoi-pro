using System;
using System.Text;
using TowerOfHanoi_Universal_App.Common;

namespace TowerOfHanoi_Universal_App.Logic
{
    /// <summary>
    /// Game helper class.
    /// </summary>
    public static class GameHelper
    {
        static StringBuilder bestMoveDetailText;

        /// <summary>
        /// Calculates best moves for given number of disks.
        /// </summary>
        /// <param name="numberOfDisk">Number of disks</param>
        /// <returns>Best moves for given number of disks</returns>
        public static StringBuilder CalculateBestMoves(int numberOfDisk)
        {
            bestMoveDetailText = new StringBuilder();
            int position = 0, value = 1, pointerAdjustment = 0;
            var bestMoves = Math.Pow(2, numberOfDisk) - 1;

            CalculateBestMoves(numberOfDisk, "Pole A", "Pole C", "Pole B");
            
            for (int i = 0; i < bestMoves; i++)
            {
                bestMoveDetailText.Insert(position, value);
                value++;
                if (value > 10 && value < 99)
                {
                    pointerAdjustment = 1;
                }
                else if (value > 100)
                {
                    pointerAdjustment = 2;
                }
                position += 37 + pointerAdjustment;
            }
            return bestMoveDetailText;
        }

        /// <summary>
        /// Gets best moves text for the given number of disks.
        /// </summary>
        /// <param name="numberOfDisk">Number of disks</param>
        /// <param name="poleA">Pole A</param>
        /// <param name="poleC">Pole B</param>
        /// <param name="poleB">Pole B</param>
        /// <returns>Best moves for given number of disks.</returns>
        static StringBuilder CalculateBestMoves(int numberOfDisk, string poleA, string poleC, string poleB)
        {
            if (numberOfDisk == 1)
            {
                bestMoveDetailText.AppendFormat(Constants.MOVE_DISK, poleA, poleC);
            }
            else
            {
                CalculateBestMoves(numberOfDisk - 1, poleA, poleB, poleC);
                bestMoveDetailText.AppendFormat(Constants.MOVE_DISK, poleA, poleC);
                CalculateBestMoves(numberOfDisk - 1, poleB, poleC, poleA);
            }
            return bestMoveDetailText;
        }
    }
}
