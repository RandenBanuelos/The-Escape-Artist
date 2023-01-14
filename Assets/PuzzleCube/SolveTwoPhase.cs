using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;

namespace TheEscapeArtist
{
    // Based on Megalomobile's implementation of a Rubik's Cube in Unity
    // https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/
    //
    // Also based on Herbert Kociemba's two-phase algorithm for solving Rubik's Cubes
    // http://kociemba.org/cube.htm
    public class SolveTwoPhase : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private List<PivotRotation> pivots = new List<PivotRotation>();

        [SerializeField] private float superSpeed = 600f;

        #endregion

        public ReadCube readCube;
        public CubeState cubeState;
        private bool doOnce = true;

        // Start is called before the first frame update
        void Start()
        {
            readCube = FindObjectOfType<ReadCube>();
            cubeState = FindObjectOfType<CubeState>();
        }

        // Update is called once per frame
        void Update()
        {
            if (CubeState.started && doOnce)
            {
                doOnce = false;
                Solver();
            }
        }

        public void ChangePivotSpeed(float newSpeed)
        {
            foreach (PivotRotation pivot in pivots)
            {
                pivot.speed = newSpeed;
            }
        }

        public void Solver()
        {
            ChangePivotSpeed(superSpeed);

            readCube.ReadState();

            // get the state of the cube as a string
            string moveString = cubeState.GetStateString();

            // solve the cube
            string info = "";

            // First time build the tables
            // string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

            //Every other time
            string solution = Search.solution(moveString, out info);

            // convert the solved moves from a string to a list
            List<string> solutionList = StringToList(solution);

            // Automate the list
            Automate.moveList = solutionList;
        }

        public void Checkerboard()
        {
            Solver();

            // Checkerboard algorithm from https://ruwix.com/the-rubiks-cube/rubiks-cube-patterns-algorithms/
            Automate.moveList.AddRange(new List<string>() { "F2", "B2", "L2", 
                                                            "R2", "U2", "D2" });
        }

        public void GiftBox()
        {
            Solver();

            // Gift box algorithm from https://ruwix.com/the-rubiks-cube/rubiks-cube-patterns-algorithms/
            Automate.moveList.AddRange(new List<string>() { "U", "B2", "R2", 
                                                            "B2", "L2", "F2", 
                                                            "R2", "D'", "F2", 
                                                            "L2", "B", "F'", 
                                                            "L", "F2", "D", 
                                                            "U'", "R2", "F'", 
                                                            "L'", "R'" });
        }

        public void Staircase()
        {
            Solver();

            // Staircase algorithm from https://ruwix.com/the-rubiks-cube/rubiks-cube-patterns-algorithms/
            Automate.moveList.AddRange(new List<string>() { "L2", "F2", "D'",
                                                            "L2", "B2", "D'",
                                                            "U'", "R2", "B2",
                                                            "U'", "L'", "B2",
                                                            "L", "D", "L",
                                                            "B'", "D", "L'",
                                                            "U" });
        }

        public void CubedCube()
        {
            Solver();

            // Cubed cube algorithm from https://ruwix.com/the-rubiks-cube/rubiks-cube-patterns-algorithms/
            Automate.moveList.AddRange(new List<string>() { "U'", "L'", "U'",
                                                            "F'", "R2", "B'",
                                                            "R", "F", "U",
                                                            "B2", "U", "B'",
                                                            "L", "U'", "F",
                                                            "U", "R", "F'" });
        }

        public void BasicAlgorithm()
        {
            Solver();

            Automate.moveList.AddRange(new List<string>() { "U", "D", "L" });
        }

        private List<string> StringToList(string solution)
        {
            List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
            return solutionList;
        }
    }
}
