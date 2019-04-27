using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestDataManager
{

	[Test]
	public void testPersistentString()
    {
    	GameDataManager gameDataManager = new GameDataManager(true);
        string testSubject = "Robert";
        gameDataManager.SaveString(testSubject, "testPersistentString");
        string loadSubject = gameDataManager.LoadString("testPersistentString");
       

        Assert.AreEqual(testSubject, loadSubject);
    }

	[Test]
	public void testPersistentInt()
	{
		GameDataManager gameDataManager = new GameDataManager(true);
		int randomInt = Random.Range(1, 100);
		gameDataManager.SaveInt(randomInt, "testPersistentInt");
		int loadedInt = gameDataManager.LoadInt("testPersistentInt");

		
		Assert.AreEqual(randomInt, loadedInt);
	}

	[Test]
	public void testPersistentFloat()
	{
		GameDataManager gameDataManager = new GameDataManager(true);
		float randomFloat = Random.Range(0.0f, 1.0f);
		gameDataManager.SaveFloat(randomFloat, "testPersistentFloat");
		float loadedFloat = gameDataManager.LoadFloat("testPersistentFloat");
	
		
		Assert.AreEqual(randomFloat, loadedFloat);
	}

	[Test]
	public void testPersistentBool()
	{
		GameDataManager gameDataManager = new GameDataManager(true);
		int randomInt = Random.Range(0, 1);
		bool randomBool = randomInt == 1;
		gameDataManager.SaveBool(randomBool, "testPersistentBool");
		bool loadedBool = gameDataManager.LoadBool("testPersistentBool");
		
		Assert.AreEqual(randomBool, loadedBool);
	}
}
