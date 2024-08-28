using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TrixieGames.Maze.Utility;
using UnityEngine;
using UnityEngine.TestTools;

namespace TrixieGames.Maze.UnitTesting
{
    public class MazeGenerationTests
    {
        private GameObject m_testGameObject;
        private MazeGenerator m_maze;

        [SetUp]
        public void Setup()
        {
            m_testGameObject = Object.Instantiate(new GameObject());
            m_maze = m_testGameObject.AddComponent<MazeGenerator>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(m_testGameObject);
        }

        [UnityTest]
        public IEnumerator GenerationLoopTest()
        {
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
