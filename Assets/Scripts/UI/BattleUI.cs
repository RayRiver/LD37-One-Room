using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{

    public GameObject _resultLayer;
    public GameObject _winText;
    public GameObject _loseText;

    public Text _gearText;
    public Text _healthText;

    public Button _skill1Button;
    public Button _skill2Button;
    public Button _skill3Button;

    void Start()
    {
        Messenger.AddListener<bool>("GameResult", OnGameResult);
        Messenger.AddListener<int>("UIUpdateGears", OnUIUpdateGears);
        Messenger.AddListener<float, float>("UIUpdateHealth", OnUIUpdateHealth);
        Messenger.AddListener<GameManager.Skill, int, bool>("UIUpdateSkill", OnUIUpdateSkill);
    }

    public void OnSkill1ButtonClicked()
    {
        Messenger.Broadcast<string>("SkillLevelUp", "movespeed");
    }

    public void OnSkill2ButtonClicked()
    {
        Messenger.Broadcast<string>("SkillLevelUp", "maxhp");
    }

    public void OnSkill3ButtonClicked()
    {
        Messenger.Broadcast<string>("SkillLevelUp", "power");
    }

    void OnUIUpdateGears(int gears)
    {
        if (_gearText != null)
        {
            _gearText.text = gears.ToString();
        }
    }

    void OnUIUpdateHealth(float hp, float maxhp)
    {
        if (_healthText != null)
        {
            _healthText.text = hp.ToString() + " / " + maxhp.ToString();
        }
    }

    void OnUIUpdateSkill(GameManager.Skill skill, int level, bool active)
    {
        Button button = null;
        if (skill.id == "movespeed")
        {
            button = _skill1Button;
        }
        else if (skill.id == "maxhp")
        {
            button = _skill2Button;
        }
        else if (skill.id == "power")
        {
            button = _skill3Button;
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
