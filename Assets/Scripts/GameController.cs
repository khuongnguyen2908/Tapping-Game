using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public double money;
    public double moneyPerSec
    {
        get
        {
            return Math.Ceiling(healthCap / 14) / healthCap * dps;
        }
    }
    public double dmg;
    public double dps;
    public double health;
    public double healthCap
    {
        get
        {
            return 10 * System.Math.Pow(2, stage - 1) * isBoss;
        }

    }

    // public double reward;
    public int stage;
    public int stageMax;
    public int kills;
    public int killsMax;
    public int isBoss;
    public float timer;
    public int timerCap;

    // Start is called before the first frame update
    public Text moneyText;
    public Text dmgText;
    public Text dpsText;
    public Text StageText;
    public Text KillText;
    public Text healthText;
    public Text TimerText;

    public GameObject back;
    public GameObject forward;

    public Image healthBar;
    public Image timerBar;

    public Animator CoinExplore;
    public GameObject coinexploreGO;
    public Animator ClockMoving;
    public GameObject ClockMove;

    //Offline
    public DateTime currentDate;
    public DateTime oldTime;
    public int OfflineProgressCheck;
    public float idleTime;
    public Text offlineTimeText;
    public float savetime;
    public GameObject offlineBox;
    public int offlineLoadCount;

    //Mult
    public Text multText;
    public double multValue;
    public float timerMult;
    public float timerMultCap;
    public double multValueMoney;
    public GameObject multBox;

    //Username
    public TMP_InputField usernameInput;
    public string username;
    public Text usernameText;
    public int newPlayer;
    public GameObject usernameBOx;

    //Upgrades
    public Text pCostText;
    public Text pLevelText;
    public Text pPowerText;
    public double pCost
    {
        get
        {
            return 10 * Math.Pow(1.07, pLevel);
        }
    }
    public int pLevel;
    public double pPower
    {
        get
        {
            return 5 * pLevel;
        }
    }
    public Text cCostText;
    public Text cLevelText;
    public Text cPowerText;
    public double cCost
    {
        get
        {
            return 10 * Math.Pow(1.07, cLevel);
        }
    }
    public int cLevel;
    public double cPower
    {
        get
        {
            return 2 * cLevel;
        }
    }

    //BG
    //public Image bg;
    public Image bgBoss;

    //Enemy
    public Image BossEnemy;
    public Image Enemy1;



    //Magic
    public double m1cost;
    public double m1power;
    public double m2cost;
    public double m2power;

  



    public void Start()
    {
        // dmg = 1;
        // dps = 1;
        //stage = 1;
        //stageMax = 1;
        // particleSystem.GetComponent<ParticleSystem>().enableEmission = false;
        Enemy1.gameObject.SetActive(false);
        offlineBox.gameObject.SetActive(false);
        multBox.gameObject.SetActive(false);
        Load();
        if (username == "<Username>")
            usernameBOx.gameObject.SetActive(true);
        else
            usernameBOx.gameObject.SetActive(false);
        iSBossChecker();
        health = healthCap;
        
        
        timerCap = 30;
       // System.Random ran = new System.Random();
        multValue = new System.Random().Next(20, 100);
        timerMultCap = new System.Random().Next(5, 10);
        timerMult = timerMultCap;
    }

    public void Update()
    {
        health -= dps;
        if (health <= 0) Kill();
        else
            health -= dps *Time.deltaTime;


        //Mult
        multValueMoney = multValue * moneyPerSec;
        
        multText.text = "$" + WordNotation(multValueMoney, "F2");
        if (timerMult <= 0) multBox.gameObject.SetActive(true);
        else
            timerMult -= Time.deltaTime;
        

        moneyText.text = WordNotation(money, "F2");
        
        StageText.text = "Stage - " + stage;
        KillText.text = kills + "/" + killsMax + " Kills";
        healthText.text = WordNotation(health, "F2") + "/" + WordNotation(healthCap, "F2") + "HP";
        dmgText.text =  WordNotation(dmg, "F2") + "";
        dpsText.text = WordNotation(dps, "F2") + "";


        healthBar.fillAmount = (float)(health / healthCap);

        if (stage > 1) back.gameObject.SetActive(true);
        else
            back.gameObject.SetActive(false);
        if (stage != stageMax) forward.gameObject.SetActive(true);
        else
            forward.gameObject.SetActive(false);
        iSBossChecker();
        usernameText.text = username;
        Upgrades();
        
        savetime += Time.deltaTime;
        if(savetime >= 5)
        {
            savetime = 0;
            Save();
        }    
    }

    public void Upgrades()
    {
         cCostText.text = "$" + WordNotation(cCost, "F2");
         cLevelText.text = "level: " + cLevel;
         cPowerText.text = "+2 per hit";

        pCostText.text = "$" + WordNotation(pCost, "F2");
        pLevelText.text = "level: " + pLevel;
        pPowerText.text =  "+5 per second";
        dps = pPower;
        dmg = 1 + cPower;
    }

    public void UsernameChange()
    {
        username = usernameInput.text;
        usernameText.text = username;
    }

    public void CloseUsernameBox()
    {
        usernameBOx.gameObject.SetActive(false);
    }

    public void iSBossChecker()
    {
        if (stage % 5 == 0)
        {
            isBoss = 10;

            StageText.text = "(BOSS!) Stage - " + stage;
            timer -= Time.deltaTime;
            ClockMoving.Play("ClockAnimation", 0, 0);
            if (timer <= 0) Back();           
            TimerText.text = timer + "/" + timerCap;
            timerBar.gameObject.SetActive(true);
            timerBar.fillAmount = timer / timerCap;
            killsMax = 1;
            BossEnemy.gameObject.SetActive(true);
            
            bgBoss.gameObject.SetActive(true);
        }
        else
        {
            isBoss = 1;
            StageText.text = "Stage - " + stage;
            TimerText.text = "";
            timerBar.gameObject.SetActive(false);
            timer = 30;
            killsMax = 10;
            BossEnemy.gameObject.SetActive(false);
            bgBoss.gameObject.SetActive(false);
        }
    }

    public void Hit()
    {
        health -= dmg;
        if(health <= 0)
        {
            Kill();
            
        }
    }

    public void Kill()
    {
        Enemy1.gameObject.SetActive(true);
        money += System.Math.Ceiling(healthCap / 14);
        CoinExplore.Play("CoinAnimation", 0, 0);
        if (stage == stageMax)
        {
            kills += 1;
           
            if (kills >= killsMax)
            {
                kills = 0;
                stage += 1;
                stageMax += 1;
            }
        }
        iSBossChecker();
        health = healthCap;
        if (isBoss > 1) timer = timerCap;
        killsMax = 10;
    }

    public void Back()
    {
        stage -= 1;
        iSBossChecker();
        health = healthCap;
    }

    public void Forward()
    {
        stage += 1;
        iSBossChecker();
        health = healthCap;
    }

    public void BuyUpgrade(string id)
    {
        switch (id)
        {
            case "p1":
                if (money >= pCost) UpgradeDefaults(ref pLevel, pCost);
                break;
            case "c1":
                if (money >= cCost) UpgradeDefaults(ref cLevel, cCost);
                break;
        }
    }

    public void UpgradeDefaults(ref int level,double cost)
    {
        
        money -= cost;
        level++;
    }

    public void BuyMagic(string id)
    {
        switch (id)
        {
            case "m1":
                m1cost = 50;
                m1power = 20;
                if (money >= m1cost)
                {
                    //particleSystem.GetComponent<ParticleSystem>().enableEmission = true;
                    money -= m1cost;
                    health -= m1power;
                    if (health <= 0)
                    {
                        Kill();

                    }

                }
                break;
            case "m2":
                m2cost = 100;
                m2power = 40;
                if (money >= m2cost)                                    
                {
                    money -= m2cost;
                    health -= m2power;
                    if (health <= 0)
                    {
                        Kill();

                    }

                }
                break;
        }
    }

   

    public string WordNotation(double number, string digits)
    {
        double digitsTemp = Math.Floor(Math.Log10(number));

        IDictionary<double, string> prefixes = new Dictionary<double, string>()
        {
            {3, "K" },
            
            {6, "M" },
            
            {9, "B" },
            
            {12, "T" },
            
            {15, "Qa" },
            
            {18, "Qi" },
           
            {21, "Qi" },
            
            {24, "Sep" }
            

        };
        double digitsEvery3 = 3 * Math.Floor(digitsTemp / 3);
        if (number >= 1000)
            return (number / Math.Pow(10, digitsEvery3)).ToString(digits) + prefixes[digitsEvery3];
        return number.ToString(digits);

    }

    public void Save()
    {
        OfflineProgressCheck = 1;
        //newPlayer = 1;
        PlayerPrefs.SetString("money", money.ToString());
        PlayerPrefs.SetString("dmg", dmg.ToString());
        PlayerPrefs.SetString("dps", dps.ToString());
        PlayerPrefs.SetString("username", username.ToString());
        PlayerPrefs.SetInt("stage", stage);
        PlayerPrefs.SetInt("stageMax", stageMax);
        PlayerPrefs.SetInt("kills", kills);
        PlayerPrefs.SetInt("killsMax", killsMax);
        PlayerPrefs.SetInt("iSBoss", isBoss);
        PlayerPrefs.SetInt("pLevel", pLevel);
        PlayerPrefs.SetInt("cLevel", cLevel);
        PlayerPrefs.SetInt("OfflineProgressCheck", OfflineProgressCheck);
        PlayerPrefs.SetInt("newPlayer", newPlayer);

        PlayerPrefs.SetString("OfflineTime", DateTime.Now.ToBinary().ToString());
        
    }

    public void Load()
    {
       money = double.Parse(PlayerPrefs.GetString("money", "0"));
        dmg = double.Parse(PlayerPrefs.GetString("dmg", "1"));
        dps =  double.Parse(PlayerPrefs.GetString("dps", "0"));
 
        stage = PlayerPrefs.GetInt("stage", 1);
        stageMax = PlayerPrefs.GetInt("stageMax", 1);
        kills = PlayerPrefs.GetInt("kills", 0);
        killsMax = PlayerPrefs.GetInt("killsMax", 10);
        isBoss = PlayerPrefs.GetInt("iSBoss", 1);
        pLevel = PlayerPrefs.GetInt("pLevel", 0);
        cLevel = PlayerPrefs.GetInt("cLevel", 0);

        OfflineProgressCheck = PlayerPrefs.GetInt("OfflineProgressCheck", 0);
       // newPlayer = PlayerPrefs.GetInt("newPlayer", 0);
        username = (PlayerPrefs.GetString("username", "<Username"));
        LoadOfflineProduction();
        
    }

    public void LoadOfflineProduction()
    {
        if (OfflineProgressCheck == 1)
        {
            offlineBox.gameObject.SetActive(true);
            long previousTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
            oldTime = DateTime.FromBinary(previousTime);
            currentDate = DateTime.Now;
            TimeSpan difference = currentDate.Subtract(oldTime);
            idleTime = (float)difference.TotalSeconds;

            var moneyToEarn = (Math.Ceiling(healthCap / 14) / healthCap) * (dps / 5 ) * idleTime;
            money += moneyToEarn;
            TimeSpan timer = TimeSpan.FromSeconds(idleTime);
            offlineTimeText.text = "You were gone for: " + timer.ToString(@"hh\:mm\:ss") + "\nYou earned: $ " + moneyToEarn.ToString("F2") ;
        }
    }

    public void CloseOfflineBox()
    {
        offlineBox.gameObject.SetActive(false);
    }

    public void OpenMult()
    {
        multBox.gameObject.SetActive(false);
        money += multValueMoney;
        timerMultCap = new System.Random().Next(5, 10);
        timerMult = timerMultCap;   
        multValue = new System.Random().Next(20, 100);
    }
}
