﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{

    public GameObject _resultLayer;
    public GameObject _winText;
    public GameObject _loseText;

    public Text _gearText;

    public Button _skill1Button;
    public Button _skill2Button;
    public Button _skill3Button;

    void Start()
    {
        Messenger.AddListener<bool>("GameResult", OnGameResult);
        Messenger.AddListener<int>("UIUpdateGears", OnUIUpdateGears);
        Messenger.AddListener<GameManager.Skill, int, bool>("UIUpdateSkill", OnUIUpdateSkill);
    }

    public void OnSkill1ButtonClicked()
    {
        Debug.Log("skill level up");
        Messenger.Broadcast<string>("SkillLevelUp", "movespeed");
    }

    void OnUIUpdateGears(int gears)
    {
        if (_gearText != null)
        {
            _gearText.text = gears.ToString();
        }
    }

    void OnUIUpdateSkill(GameManager.Skill skill, int level, bool active)
    {
        Button button = null;
        if (skill.id == "movespeed")
        {
            button = _skill1Button;
        }

        if (button != null)
        {
            button.interactable = active;

            var text = button.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = skill.name + "\nLV " + (level + 1).ToString() + "\n";
                if (level >= skill.levels.Length - 1)
                {
                    text.text += "MAX";
                }
                else
                {
                    text.text += ("Next: " + skill.levels[level].cost.ToString());
                }
            }
        }

    }

    void OnGameResult(bool win)
    {
        if (_resultLayer != null)
        {
            _resultLayer.SetActive(true);
        }

        if (win)
        {
            _winText.SetActive(true);
            _loseText.SetActive(false);
        }
        else
        {
            _winText.SetActive(false);
            _loseText.SetActive(true);
        }
    }
}
