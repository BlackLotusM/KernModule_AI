using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Ally : MonoBehaviour
{
    private BTSelector rootNode;
    [SerializeField] private TextMeshProUGUI showNode;

    [SerializeField] private BTBaseNode SmokeEnemy;
    [SerializeField] private BTBaseNode GoToPlayer;
    [SerializeField] private BTBaseNode RotateAroundPlayer;

    [SerializeField] private boole underAttack;
    [SerializeField] private boole hasAttacked;
    [SerializeField] private boole isAtPlayer;

    [SerializeField] private GameObject prefabBom;
    [SerializeField] private GameObject child;
    [SerializeField] private GameObject[] cover;
    [SerializeField] private GameObject player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private TextMeshProUGUI enemyTXT;

    private void Start()
    {
        SmokeEnemy =
                new BTSequence(showNode,
                    new BTCheckAttack(underAttack, isAtPlayer, agent),
                    new BTFlyToCover(agent, cover),
                    new BTWait(3f),
                    new BTThrowSmoke(prefabBom, child, underAttack, hasAttacked)
                );

        RotateAroundPlayer =
                new BTSequence(showNode,
                    new BTTCheckAtPlayer(isAtPlayer),
                    new BTFlyArroundPlayer(isAtPlayer, agent, child, player)
                );

        GoToPlayer =
                new BTSequence(showNode,
                    new BTCheckAttackDone(hasAttacked, agent),
                    new BTMoveAlly(hasAttacked, isAtPlayer, child, agent, player)
                );

        rootNode = new BTSelector(new List<BTBaseNode> { SmokeEnemy, GoToPlayer, RotateAroundPlayer });
        rootNode.Run();
    }

    private void FixedUpdate()
    {
        if (enemyTXT.text == "BTMove")
        {
            underAttack.active = true;
        }
        rootNode?.Run();
    }
}
