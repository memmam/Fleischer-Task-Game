// System libraries
using System.IO;
using System.Linq;

// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// Sets up and manages results screen
public class Score : MonoBehaviour
{
    int gameMode;
    int difficulty;

    // Holders for final game state
    int unanswered;
    int wrong;
    int score;
    float avgTime;
    float bestTime;
    float worstTime;
    float bestCongTime;
    float worstCongTime;
    float bestIncongTime;
    float worstIncongTime;
    float congTimeAvg;
    float incongTimeAvg;
    float flankerEffect;

    // Holders for rounded averages / Flanker Effect (for results screen)
    float avgTimeRound;
    float bestTimeRound;
    float worstTimeRound;
    float bestCongTimeRound;
    float bestIncongTimeRound;
    float worstCongTimeRound;
    float worstIncongTimeRound;
    float congTimeAvgRound;
    float incongTimeAvgRound;
    float flankerRound;

    // Best time output string, so the game shows if there is no best time
    // (i.e. the player got all trials wrong)
    string bestString;

    // ID for entry in CSV file
    int resultNum;

    // Game elements for results readout and 'View Results' button
    public Text scoreText;
    public Text leftText;
    public Text rightText;
    public Text bottomText;
    public GameObject resultsButton;
    public Button restartButton;
    public Button mainMenuButton;

    float allTimeBest;
    float allTimeBestAvg;
    float allTimeBestCong;
    float allTimeBestIncong;
    float allTimeBestAvgCong;
    float allTimeBestAvgIncong;
    float allTimeBestFlanker;

    float allTimeWorst;
    float allTimeWorstAvg;
    float allTimeWorstCong;
    float allTimeWorstIncong;
    float allTimeWorstAvgCong;
    float allTimeWorstAvgIncong;
    float allTimeWorstFlanker;

    // Set up results readout, write to CSV file
    void Start()
    {
        Timer.timerStart = true;

        gameMode = PlayerPrefs.GetInt("GameMode");
        difficulty = PlayerPrefs.GetInt("Difficulty");

        allTimeBest = float.NaN;
        allTimeBestAvg = float.NaN;
        allTimeBestCong = float.NaN;
        allTimeBestIncong = float.NaN;
        allTimeBestAvgCong = float.NaN;
        allTimeBestAvgIncong = float.NaN;

        allTimeWorst = float.NaN;
        allTimeWorstAvg = float.NaN;
        allTimeWorstCong = float.NaN;
        allTimeWorstIncong = float.NaN;
        allTimeWorstAvgCong = float.NaN;
        allTimeWorstAvgIncong = float.NaN;

        // Set file path to CSV file
        string filePath = "flanker.csv";

        // Get game state from previous scene
        wrong = PlayerPrefs.GetInt("Wrong Answers");
        unanswered = PlayerPrefs.GetInt("Unanswered Trials");
        score = PlayerPrefs.GetInt("PlayerScore");
        avgTime = PlayerPrefs.GetFloat("avgTime");
        bestTime = PlayerPrefs.GetFloat("bestTime");
        worstTime = PlayerPrefs.GetFloat("worstTime");
        bestCongTime = PlayerPrefs.GetFloat("bestCongTime");
        worstCongTime = PlayerPrefs.GetFloat("worstCongTime");
        bestIncongTime = PlayerPrefs.GetFloat("bestIncongTime");
        worstIncongTime = PlayerPrefs.GetFloat("worstIncongTime");
        congTimeAvg = PlayerPrefs.GetFloat("avgCongTime");
        incongTimeAvg = PlayerPrefs.GetFloat("avgIncongTime");

        // Calculate rounded averages for results text
        avgTimeRound = Mathf.Round(avgTime * 1000) / 1000;
        bestTimeRound = Mathf.Round(bestTime * 1000) / 1000;
        worstTimeRound = Mathf.Round(worstTime * 1000) / 1000;
        bestCongTimeRound = Mathf.Round(bestCongTime * 1000) / 1000;
        bestIncongTimeRound = Mathf.Round(bestIncongTime * 1000) / 1000;
        worstCongTimeRound = Mathf.Round(worstCongTime * 1000) / 1000;
        worstIncongTimeRound = Mathf.Round(worstIncongTime * 1000) / 1000;
        congTimeAvgRound = Mathf.Round(congTimeAvg * 1000) / 1000;
        incongTimeAvgRound = Mathf.Round(incongTimeAvg * 1000) / 1000;

        // Calculate Flanker Effect
        if (congTimeAvg >= incongTimeAvg)
        {
            flankerEffect = congTimeAvg - incongTimeAvg;
        }
        else
        {
            flankerEffect = incongTimeAvg - congTimeAvg;
        }

        // Round Flanker Effect for results text
        flankerRound = Mathf.Round(flankerEffect * 1000) / 1000;

        allTimeBest = PlayerPrefs.GetFloat("allBestTime_" + stateManager.Instance.playerName, float.NaN);
        allTimeWorst = PlayerPrefs.GetFloat("allWorstTime_" + stateManager.Instance.playerName, float.NaN);
        // If no best time, don't output a best time
        if (float.IsNaN(bestTimeRound))
        {
            bestString = "\nNo best time...";
        }
        // Else, output best time, and if it is an all-time best time, update the all-time best and worst times
        else
        {
            bestString = "\nBest Time: " + bestTimeRound;
            if (bestTimeRound < allTimeBest || float.IsNaN(allTimeBest))
            {
                PlayerPrefs.SetFloat("allBestTime_" + stateManager.Instance.playerName, bestTimeRound);
                allTimeBest = bestTimeRound;
            }
            
            if (worstTimeRound > allTimeWorst || float.IsNaN(allTimeWorst))
            {
                PlayerPrefs.SetFloat("allWorstTime_" + stateManager.Instance.playerName, worstTimeRound);
                allTimeWorst = worstTimeRound;
            }
        }

        allTimeBestAvg = PlayerPrefs.GetFloat("allBestAvg_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time best average time
        if (!float.IsNaN(avgTimeRound))
        {
            if (avgTimeRound < allTimeBestAvg || float.IsNaN(allTimeBestAvg))
            {
                PlayerPrefs.SetFloat("allBestAvg_" + stateManager.Instance.playerName, avgTimeRound);
                allTimeBestAvg = avgTimeRound;
            }
        }


        allTimeWorstAvg = PlayerPrefs.GetFloat("allWorstAvg_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time worst average time
        if (!float.IsNaN(avgTimeRound))
        {
            if (avgTimeRound > allTimeWorstAvg || float.IsNaN(allTimeWorstAvg))
            {
                PlayerPrefs.SetFloat("allWorstAvg_" + stateManager.Instance.playerName, avgTimeRound);
                allTimeWorstAvg = avgTimeRound;
            }
        }

        allTimeBestAvgCong = PlayerPrefs.GetFloat("allBestCongAvg_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time best average congruent time
        if (!float.IsNaN(congTimeAvgRound))
        {
            if (congTimeAvgRound < allTimeBestAvgCong || float.IsNaN(allTimeBestAvgCong))
            {
                PlayerPrefs.SetFloat("allBestCongAvg_" + stateManager.Instance.playerName, congTimeAvgRound);
                allTimeBestAvgCong = congTimeAvgRound;
            }
        }

        allTimeBestAvgIncong = PlayerPrefs.GetFloat("allBestIncongAvg_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time best average incongruent time
        if (!float.IsNaN(incongTimeAvgRound))
        {
            if (incongTimeAvgRound < allTimeBestAvgIncong || float.IsNaN(allTimeBestAvgIncong))
            {
                PlayerPrefs.SetFloat("allBestIncongAvg_" + stateManager.Instance.playerName, incongTimeAvgRound);
                allTimeBestAvgIncong = incongTimeAvgRound;
            }
        }

        allTimeWorstAvgCong = PlayerPrefs.GetFloat("allWorstCongAvg_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time worst average congruent time
        if (!float.IsNaN(congTimeAvgRound))
        {
            if (congTimeAvgRound > allTimeWorstAvgCong || float.IsNaN(allTimeWorstAvgCong))
            {
                PlayerPrefs.SetFloat("allWorstCongAvg_" + stateManager.Instance.playerName, congTimeAvgRound);
                allTimeWorstAvgCong = congTimeAvgRound;
            }
        }

        allTimeWorstAvgIncong = PlayerPrefs.GetFloat("allWorstIncongAvg_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time worst average incongruent time
        if (!float.IsNaN(incongTimeAvgRound))
        {
            if (incongTimeAvgRound > allTimeWorstAvgIncong || float.IsNaN(allTimeWorstAvgIncong))
            {
                PlayerPrefs.SetFloat("allWorstIncongAvg_" + stateManager.Instance.playerName, incongTimeAvgRound);
                allTimeWorstAvgIncong = incongTimeAvgRound;
            }
        }

        allTimeBestCong = PlayerPrefs.GetFloat("allBestCong_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time best congruent time
        if (!float.IsNaN(bestCongTimeRound))
        {
            if (bestCongTimeRound < allTimeBestCong || float.IsNaN(allTimeBestCong))
            {
                PlayerPrefs.SetFloat("allBestCong_" + stateManager.Instance.playerName, bestCongTimeRound);
                allTimeBestCong = bestCongTimeRound;
            }
        }

        allTimeBestIncong = PlayerPrefs.GetFloat("allBestIncong_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time best incongruent time
        if (!float.IsNaN(bestIncongTimeRound))
        {
            if (bestIncongTimeRound < allTimeBestIncong || float.IsNaN(allTimeBestIncong))
            {
                PlayerPrefs.SetFloat("allBestIncong_" + stateManager.Instance.playerName, bestIncongTimeRound);
                allTimeBestIncong = bestIncongTimeRound;
            }
        }

        allTimeWorstCong = PlayerPrefs.GetFloat("allWorstCong_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time worst congruent time
        if (!float.IsNaN(worstCongTimeRound))
        {
            if (worstCongTimeRound > allTimeWorstCong || float.IsNaN(allTimeWorstCong))
            {
                PlayerPrefs.SetFloat("allWorstCong_" + stateManager.Instance.playerName, worstCongTimeRound);
                allTimeWorstCong = worstCongTimeRound;
            }
        }

        allTimeWorstIncong = PlayerPrefs.GetFloat("allWorstIncong_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time worst incongruent time
        if (!float.IsNaN(worstIncongTimeRound))
        {
            if (worstIncongTimeRound > allTimeWorstIncong || float.IsNaN(allTimeWorstIncong))
            {
                PlayerPrefs.SetFloat("allWorstIncong_" + stateManager.Instance.playerName, worstIncongTimeRound);
                allTimeWorstIncong = worstIncongTimeRound;
            }
        }

        allTimeBestFlanker = PlayerPrefs.GetFloat("allBestFlanker_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time best incongruent time
        if (!float.IsNaN(flankerRound))
        {
            if (flankerRound < allTimeBestFlanker || float.IsNaN(allTimeBestFlanker))
            {
                PlayerPrefs.SetFloat("allBestFlanker_" + stateManager.Instance.playerName, flankerRound);
                allTimeBestFlanker = flankerRound;
            }
        }

        allTimeWorstFlanker = PlayerPrefs.GetFloat("allWorstFlanker_" + stateManager.Instance.playerName, float.NaN);
        // Update all-time worst incongruent time
        if (!float.IsNaN(flankerRound))
        {
            if (flankerRound > allTimeWorstFlanker || float.IsNaN(allTimeWorstFlanker))
            {
                PlayerPrefs.SetFloat("allWorstFlanker_" + stateManager.Instance.playerName, flankerRound);
                allTimeWorstFlanker = flankerRound;
            }
        }

        // If CSV file does not exist, create it and set up label row
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "testNumber,name,gameMode,difficulty,score,wrong,unanswered,averageTime,averageCongruentTime,averageIncongruentTime,flankerEffect,bestTime,worstTime,bestCongruentTime,worstCongruentTime,bestIncongruentTime,worstIncongruentTime,globalBestTime,globalWorstTime,globalBestAvg,globalWorstAvg,globalBestCongruentTime,globalWorstCongruentTime,globalBestIncongruentTime,globalWorstIncongruentTime,globalBestCongruentAverage,globalWorstCongruentAverage,globalBestIncongruentAverage,globalWorstIncongruentAverage,globalBestFlankerEffect,globalWorstFlankerEffect\n");
        }

        // Set current game ID based on number of existing lines in CSV file
        resultNum = File.ReadLines(filePath).Count();

        // Write current game to CSV file
        File.AppendAllText(filePath, resultNum + "," + stateManager.Instance.playerName + "," + gameMode + "," + difficulty + "," + score + "," + wrong + "," + unanswered + "," + avgTimeRound + "," + congTimeAvgRound + "," + incongTimeAvgRound + "," + flankerRound + "," + bestTimeRound + "," + worstTimeRound + "," + bestCongTime + "," + worstCongTime + "," + bestIncongTime + "," + worstIncongTime + "," + allTimeBest + "," + allTimeWorst + "," + allTimeBestAvg + ","  + allTimeWorstAvg + "," + allTimeBestCong + "," + allTimeWorstCong + "," + allTimeBestIncong + "," + allTimeWorstIncong + "," + allTimeBestAvgCong + "," + allTimeWorstAvgCong + "," + allTimeBestAvgIncong + "," + allTimeWorstAvgIncong + "," + allTimeBestFlanker + "," + allTimeWorstFlanker+ ",\n");

        // handle NaNs for divide-by-zero
        if (float.IsNaN(avgTimeRound))
        {
            avgTimeRound = 0.0f;
        }
        if (float.IsNaN(congTimeAvgRound))
        {
            congTimeAvgRound = 0.0f;
        }
        if (float.IsNaN(incongTimeAvgRound))
        {
            incongTimeAvgRound = 0.0f;
        }
        if (float.IsNaN(flankerRound))
        {
            flankerRound = 0.0f;
        }

        // Populate game results text with information
        scoreText.text = "Score: " + score;
        leftText.text = "\nWrong: " + wrong +
                        "\nAvg Time: " + avgTimeRound +
                        bestString;
        rightText.text = "\nUnanswered: " + unanswered +
                         "\nCongruent Avg: " + congTimeAvgRound +
                         "\nIncongruent Avg: " + incongTimeAvgRound;
        bottomText.text = "\nFlanker Effect: " + flankerRound;
    }

    private void Update()
    {
        // Achievement: Stand Idle at the results screen
        // Bronze: 3 minutes
        // Silver: 5 minutes
        // Gold: 10 minutes
        if (Timer.getTimer() >= 600 && AchievementManager.Instance.achievementList[11].state == 2)
        {
            AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[11], 1);

            // Achievement: Get all achievements
            // Bronze: All bronze or better
            // Silver: All silver or better
            // Gold: All gold or better
            AchievementManager.Instance.achievementsAchievement();
        }
        else if (Timer.getTimer() >= 300 && AchievementManager.Instance.achievementList[11].state == 1)
        {
            AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[11], 1);

            // Achievement: Get all achievements
            // Bronze: All bronze or better
            // Silver: All silver or better
            // Gold: All gold or better
            AchievementManager.Instance.achievementsAchievement();
        }
        else if (Timer.getTimer() >= 180 && AchievementManager.Instance.achievementList[11].state == 0)
        {
            AchievementManager.Instance.getAchievement(AchievementManager.Instance.achievementList[11], 1);

            // Achievement: Get all achievements
            // Bronze: All bronze or better
            // Silver: All silver or better
            // Gold: All gold or better
            AchievementManager.Instance.achievementsAchievement();
        }
    }
}