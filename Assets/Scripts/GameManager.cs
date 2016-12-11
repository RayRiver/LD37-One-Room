using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public EnemySpawner _enemySpawner;

    [System.Serializable]
    public class SkillLevel
    {
        public int cost;
        public float effect;
    }
    [System.Serializable]
    public class Skill
    {
        public string id;
        public string name;
        public SkillLevel[] levels;
    }
    public Skill[] _skills;
    private Dictionary<string, Skill> _skillDict;

    private Dictionary<string, int> _skillLevels;

    private bool _gameOver = false;
    private bool _win = true;
    private int _wave = -1;
    private int _nextWave = 0;
    private bool _waitingForRestart = false;

    private int _gears = 0;

    public delegate IEnumerator WaveProcess();
    private WaveProcess[] Waves;

    IEnumerator Wave1()
    {
        yield return new WaitForSeconds(2);

        _enemySpawner.CreateEnemy("Enemy", "LeftTop");
        _enemySpawner.CreateEnemy("Enemy", "LeftBottom");
        _enemySpawner.CreateEnemy("Enemy", "RightTop");
        _enemySpawner.CreateEnemy("Enemy", "RightBottom");

        yield return new WaitForSeconds(6);

        _enemySpawner.CreateEnemy("Enemy", "Top");
        _enemySpawner.CreateEnemy("Enemy", "Bottom");
        _enemySpawner.CreateEnemy("Enemy", "Left");
        _enemySpawner.CreateEnemy("Enemy", "Right");

        yield return new WaitForSeconds(3);
    }

    IEnumerator Wave2()
    {
        yield return new WaitForSeconds(3);

        _enemySpawner.CreateEnemy("Enemy", "Top");
        _enemySpawner.CreateEnemy("Enemy", "Bottom");
        _enemySpawner.CreateEnemy("Enemy", "Left");
        _enemySpawner.CreateEnemy("Enemy", "Right");
        _enemySpawner.CreateEnemy("Enemy2", "Left");

        yield return new WaitForSeconds(8);

        _enemySpawner.CreateEnemy("Enemy", "LeftTop");
        _enemySpawner.CreateEnemy("Enemy", "LeftBottom");
        _enemySpawner.CreateEnemy("Enemy", "RightTop");
        _enemySpawner.CreateEnemy("Enemy", "RightBottom");
        _enemySpawner.CreateEnemy("Enemy2", "Right");

        yield return new WaitForSeconds(3);
    }

    IEnumerator Wave3()
    {
        yield return new WaitForSeconds(3);

        _enemySpawner.CreateEnemy("Enemy", "Top");
        _enemySpawner.CreateEnemy("Enemy", "LeftTop");
        _enemySpawner.CreateEnemy("Enemy", "Left");
        _enemySpawner.CreateEnemy("Enemy2", "RightBottom");
        _enemySpawner.CreateEnemy("Enemy2", "Right");

        yield return new WaitForSeconds(12);

        _enemySpawner.CreateEnemy("Enemy", "Bottom");
        _enemySpawner.CreateEnemy("Enemy", "RightBottom");
        _enemySpawner.CreateEnemy("Enemy", "Right");
        _enemySpawner.CreateEnemy("Enemy2", "LeftTop");
        _enemySpawner.CreateEnemy("Enemy2", "Left");

        yield return new WaitForSeconds(3);
    }

    IEnumerator Wave4()
    {
        yield return new WaitForSeconds(4);

        _enemySpawner.CreateEnemy("Enemy", "Bottom");
        _enemySpawner.CreateEnemy("Enemy", "RightBottom");
        _enemySpawner.CreateEnemy("Enemy", "Right");
        _enemySpawner.CreateEnemy("Enemy2", "LeftTop");
        _enemySpawner.CreateEnemy("Enemy2", "Left");

        yield return new WaitForSeconds(9);

        _enemySpawner.CreateEnemy("Enemy", "Top");
        _enemySpawner.CreateEnemy("Enemy", "LeftTop");
        _enemySpawner.CreateEnemy("Enemy", "Left");
        _enemySpawner.CreateEnemy("Enemy2", "RightBottom");
        _enemySpawner.CreateEnemy("Enemy2", "Right");

        yield return new WaitForSeconds(4);
    }

    IEnumerator Wave5()
    {
        yield return new WaitForSeconds(4);

        _enemySpawner.CreateEnemy("Enemy", "Center");
        _enemySpawner.CreateEnemy("Enemy", "CenterLeft");
        _enemySpawner.CreateEnemy("Enemy", "CenterRight");

        yield return new WaitForSeconds(5);

        _enemySpawner.CreateEnemy("Enemy2", "Top");
        _enemySpawner.CreateEnemy("Enemy2", "Bottom");
        _enemySpawner.CreateEnemy("Enemy2", "Left");
        _enemySpawner.CreateEnemy("Enemy2", "Right");

        yield return new WaitForSeconds(6);

        _enemySpawner.CreateEnemy("Enemy2", "RightTop");
        _enemySpawner.CreateEnemy("Enemy2", "LeftBottom");

        yield return new WaitForSeconds(4);
    }

    IEnumerator Wave6()
    {
        yield return new WaitForSeconds(4);

        _enemySpawner.CreateEnemy("Enemy", "Center");

        yield return new WaitForSeconds(2);

        _enemySpawner.CreateEnemy("Enemy2", "Left");
        _enemySpawner.CreateEnemy("Enemy2", "Right");

        yield return new WaitForSeconds(2);

        _enemySpawner.CreateEnemy("Enemy2", "Left");
        _enemySpawner.CreateEnemy("Enemy2", "Right");

        yield return new WaitForSeconds(2);

        _enemySpawner.CreateEnemy("Enemy2", "Top");
        _enemySpawner.CreateEnemy("Enemy2", "Bottom");

        yield return new WaitForSeconds(2);

        _enemySpawner.CreateEnemy("Enemy2", "Top");
        _enemySpawner.CreateEnemy("Enemy2", "Bottom");

        yield return new WaitForSeconds(2);

        _enemySpawner.CreateEnemy("Enemy2", "Center");
        _enemySpawner.CreateEnemy("Enemy2", "CenterLeft");
        _enemySpawner.CreateEnemy("Enemy2", "CenterRight");

        yield return new WaitForSeconds(4);
    }

    IEnumerator Wave7()
    {
        yield return new WaitForSeconds(5);

        _enemySpawner.CreateEnemy("Enemy", "CenterLeft");
        _enemySpawner.CreateEnemy("Enemy", "CenterRight");

        yield return new WaitForSeconds(2);

        _enemySpawner.CreateEnemy("Enemy2", "LeftTop");
        _enemySpawner.CreateEnemy("Enemy2", "RightTop");
        _enemySpawner.CreateEnemy("Enemy2", "LeftBottom");
        _enemySpawner.CreateEnemy("Enemy2", "RightBottom");

        yield return new WaitForSeconds(3);

        _enemySpawner.CreateEnemy("Enemy3", "Center");

        yield return new WaitForSeconds(5);
    }

    void Awake()
    {
        Messenger.AddListener("PlayerDead", OnPlayerDead);
        Messenger.AddListener("GetGear", OnGetGear);
        Messenger.AddListener<string>("SkillLevelUp", OnSkillLevelUp);
    }

    void Start()
    {
        Waves = new WaveProcess[]
        {
            Wave1,
            Wave2,
            Wave3,
            Wave4,
            Wave5,
            Wave6,
            Wave7,
        };

        _skillLevels = new Dictionary<string, int>();
        _skillDict = new Dictionary<string, Skill>();
        for (var i = 0; i < _skills.Length; ++i)
        {
            _skillLevels[_skills[i].id] = 0;
            _skillDict[_skills[i].id] = _skills[i];
        }

        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (_waitingForRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Game");
                return;
            }
        }
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    void UpdateSkill(string id)
    {
        // find skill;
        var skill = _skillDict[id];
        if (skill == null) return;

        var level = _skillLevels[skill.id];

        // reach max level;
        if (level >= skill.levels.Length - 1)
        {
            Messenger.Broadcast<Skill, int, bool>("UIUpdateSkill", skill, level, false);
            return;
        }

        // check can level up;
        var skillLevel = skill.levels[level];
        var active = false;
        if (_gears >= skillLevel.cost)
        {
            active = true;
        }
        Messenger.Broadcast<Skill, int, bool>("UIUpdateSkill", skill, level, active);
    }

    void OnGetGear()
    {
        _gears++;

        Messenger.Broadcast<int>("UIUpdateGears", _gears);

        // check skill update;
        foreach (var skill in _skills)
        {
            UpdateSkill(skill.id);
        }
    }

    void OnPlayerDead()
    {
        _gameOver = true;
        _win = false;
    }

    void OnSkillLevelUp(string id)
    {
        if (IsGameOver()) return;

        // find skill;
        var skill = _skillDict[id];
        if (skill == null)
        {
            Debug.Log("skill not found");
            return;
        }

        // reach max level;
        var level = _skillLevels[skill.id];
        if (level >= skill.levels.Length - 1)
        {
            Messenger.Broadcast<Skill, int, bool>("UIUpdateSkill", skill, level, false);
            Debug.Log("skill max level");
            return;
        }

        // skill level up;
        var skillLevel = skill.levels[level];
        if (_gears >= skillLevel.cost)
        {
            _skillLevels[skill.id]++;
            _gears -= skillLevel.cost;

            var current_level = _skillLevels[skill.id];
            Messenger.Broadcast<string, float>("SkillLevelUpFinished", skill.id, skill.levels[current_level].effect);

            Messenger.Broadcast<int>("UIUpdateGears", _gears);
            Messenger.Broadcast<string, Vector3>("PlaySfx", "LevelUp", Vector3.zero);
        }

        UpdateSkill(skill.id);
    }

    void CheckWaveEnd()
    {
        if (_enemySpawner.GetEnemyCount() <= 0)
        {
            _wave++;
        }
    }

    IEnumerator GameLoop()
    {
        if (CheatMode.On)
        {
            _gears = 99;
        }
        else
        {
            _gears = 0;
        }
        Messenger.Broadcast<int>("UIUpdateGears", _gears);
        foreach (var s in _skills)
        {
            var skill = s;
            var level = _skillLevels[skill.id];
            Messenger.Broadcast<Skill, int, bool>("UIUpdateSkill", skill, level, false);
        }

        yield return new WaitForSeconds(1);

        _gameOver = false;
        while (!_gameOver)
        {
            if (_nextWave > _wave)
            {
                _wave = _nextWave;

                Debug.Log("wave " + _wave);

                Messenger.Broadcast<int, int>("WaveStart", _wave + 1, Waves.Length);

                yield return Waves[_wave]();
            }
            else
            {
                // check all enmey died;
                if (_enemySpawner.GetEnemyCount() <= 0)
                {
                    _nextWave++;
                    if (_nextWave >= Waves.Length)
                    {
                        _gameOver = true;
                        _win = true;
                        Debug.Log("wave over");
                        break;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }

        Messenger.Broadcast<bool>("GameResult", _win);
        _waitingForRestart = true;

    }

    public float GetSkillEffect(string id)
    {
        var skill = _skillDict[id];
        var level = _skillLevels[id];
        return skill.levels[level].effect;
    }
}
