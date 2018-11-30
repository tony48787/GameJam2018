using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRule {
    /*
     * 
     * https://gamedev.stackexchange.com/questions/20934/how-to-create-adjustable-formula-for-rpg-level-up-requirements
     * 
     * 
    */
    private int level;
    private long valForFirstStep;
    private long valForLastStep;

    private Dictionary<int, long> levelToValueDict;

    public PowerRule(int level, long valForFirstStep, long valForLastStep)
    {
        this.level = level;
        this.valForLastStep = valForLastStep;
        this.valForFirstStep = valForFirstStep;

        levelToValueDict = new Dictionary<int, long>();

        computeLevelMap();
    }

    private void computeLevelMap()
    {
        float B = Mathf.Log((float) valForLastStep / valForFirstStep) / (level - 1);
        float A = (float) (valForFirstStep / (Mathf.Exp(B) - 1.0));

        for (int i = 1; i <= level; i++)
        {
            int old_xp = (int) Mathf.Round(A * Mathf.Exp(B * (i - 1)));
            int new_xp = (int) Mathf.Round(A * Mathf.Exp(B * i));
            levelToValueDict[i] = new_xp - old_xp;
        }
    }

    public long retrieveValueForLevel(int level)
    {
        if (level <= 0) return 1;
        if (level > this.level) return levelToValueDict[this.level];
        return levelToValueDict[level];
    }

    public void printAllLevelValue(string name = "") {
        foreach (int key in levelToValueDict.Keys) {
            Debug.Log("rule name: " + name + ": key: " + key + " value: " + levelToValueDict[key]);
        }
    }
}
