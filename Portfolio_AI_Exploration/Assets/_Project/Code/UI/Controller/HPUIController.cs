using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private RectTransform playerRoot; // ← 最大HPの長さ
    [SerializeField] private Image playerHPBar;

    [Header("Enemy")]
    [SerializeField] private RectTransform enemyRoot;
    [SerializeField] private Image enemyHPBar;

    [SerializeField] private float maxBarWidth = 300f; // 最大HP100のときの幅

    [SerializeField] private TextMeshProUGUI enemyHPText;
    private BattleAI player;
    private BattleAI enemy;


    private float playerTarget;
    private float enemyTarget;

    public void Init(BattleAI player, BattleAI enemy)
    {
        this.player = player;
        this.enemy = enemy;

        // 最大HPに応じてバーの長さ決定
        SetMaxHPBar(playerRoot, player.Blackboard.MaxHP);
        SetMaxHPBar(enemyRoot, enemy.Blackboard.MaxHP);

        enemyHPText.text = $"{enemy.Blackboard.CurrentHP} / {enemy.Blackboard.MaxHP}";
        player.Blackboard.OnHPChanged += OnHPChanged;
        enemy.Blackboard.OnHPChanged += OnHPChanged;

        UpdateBar(player, playerHPBar);
        UpdateBar(enemy, enemyHPBar);
    }

    private void OnHPChanged(BattleAI owner, int current, int max)
    {
        if (owner == player)
            UpdateBar(player, playerHPBar);

        else if (owner == enemy)
        {
            UpdateBar(enemy, enemyHPBar);
            enemyHPText.text = $"{current} / {max}";

        }
    }

    void SetMaxHPBar(RectTransform root, int maxHP)
    {
        float referenceMaxHP = 100f;

        float ratio = Mathf.Log10(maxHP + 1) / Mathf.Log10(referenceMaxHP + 1);

        float width = maxBarWidth * ratio;

        root.sizeDelta = new Vector2(width, root.sizeDelta.y);
    }
    


    void UpdateBar(BattleAI ai, Image bar)
    {
        float ratio = (float)ai.Blackboard.CurrentHP / ai.Blackboard.MaxHP;
        bar.fillAmount = ratio;
    }
}
