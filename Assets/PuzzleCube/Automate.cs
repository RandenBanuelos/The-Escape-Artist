﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of a Rubik's Cube in Unity
    // https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/
    public class Automate : MonoBehaviour
    {
        #region Public Static Fields

        public static List<string> moveList = new List<string>() { };

        #endregion

        #region Private Fields

        private readonly List<string> allMoves = new List<string>()
        { "U", "D", "L", "R", "F", "B",
          "U2", "D2", "L2", "R2", "F2", "B2",
          "U'", "D'", "L'", "R'", "F'", "B'"
        };

        private CubeState cubeState;

        private ReadCube readCube;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            cubeState = FindObjectOfType<CubeState>();
            readCube = FindObjectOfType<ReadCube>();
        }

        private void Update()
        {
            if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
            {
                // Do the move at the first index;
                DoMove(moveList[0]);

                // Remove the move at the first index
                moveList.Remove(moveList[0]);
            }
        }

        #endregion

        #region Public Methods

        public void Shuffle()
        {
            List<string> moves = new List<string>();
            int shuffleLength = Random.Range(10, 30);
            for (int i = 0; i < shuffleLength; i++)
            {
                int randomMove = Random.Range(0, allMoves.Count);
                moves.Add(allMoves[randomMove]);
            }
            moveList = moves;
        }

        #endregion

        #region Private Methods

        private void DoMove(string move)
        {
            readCube.ReadState();
            CubeState.autoRotating = true;
            if (move == "U")
            {
                RotateSide(cubeState.up, -90);
            }
            if (move == "U'")
            {
                RotateSide(cubeState.up, 90);
            }
            if (move == "U2")
            {
                RotateSide(cubeState.up, -180);
            }
            if (move == "D")
            {
                RotateSide(cubeState.down, -90);
            }
            if (move == "D'")
            {
                RotateSide(cubeState.down, 90);
            }
            if (move == "D2")
            {
                RotateSide(cubeState.down, -180);
            }
            if (move == "L")
            {
                RotateSide(cubeState.left, -90);
            }
            if (move == "L'")
            {
                RotateSide(cubeState.left, 90);
            }
            if (move == "L2")
            {
                RotateSide(cubeState.left, -180);
            }
            if (move == "R")
            {
                RotateSide(cubeState.right, -90);
            }
            if (move == "R'")
            {
                RotateSide(cubeState.right, 90);
            }
            if (move == "R2")
            {
                RotateSide(cubeState.right, -180);
            }
            if (move == "F")
            {
                RotateSide(cubeState.front, -90);
            }
            if (move == "F'")
            {
                RotateSide(cubeState.front, 90);
            }
            if (move == "F2")
            {
                RotateSide(cubeState.front, -180);
            }
            if (move == "B")
            {
                RotateSide(cubeState.back, -90);
            }
            if (move == "B'")
            {
                RotateSide(cubeState.back, 90);
            }
            if (move == "B2")
            {
                RotateSide(cubeState.back, -180);
            }
        }

        private void RotateSide(List<GameObject> side, float angle)
        {
            // Automatically rotate the side by the angle
            PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
            pr.StartAutoRotate(side, angle);
        }

        #endregion
    }
}
