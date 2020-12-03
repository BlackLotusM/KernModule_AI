using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [Header("Boole's")]
    public boole isInSmoke;
    public boole hasWeapon;
    public boole needsWeapon;
    public boole cantAttack;

    [Header("Enemy")]
    public GameObject weaponHolder;
    public NavMeshAgent agent;

    [Header("Important Postions")]
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject[] WayPoints;

    [Header("Node Stuff")]
    public TextMeshProUGUI showNode;
    private BTSelector rootNode;
    private BTBaseNode CheckWeapon;
    private BTBaseNode AttackTree;
    private BTBaseNode PatrollTree;
    private BTBaseNode CheckSmoke;

    [Header("View Stuff")]
    [SerializeField] private float viewRadius;
    [Range(0, 360)]
    [SerializeField] private float viewAngle;
    [SerializeField] private float lowestDist = Mathf.Infinity;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();
    public gameobecte ClosestPlayer;
    [SerializeField] private LayerMask checkMask;

    [Header("Light Inication")]
    [SerializeField] private Light lighting;
    [SerializeField] private Color PatrolCol;
    [SerializeField] private Color CheckForWeaponCol;
    [SerializeField] private Color WeaponCol;
    

    private void Start()
    {
        CheckSmoke =
                new BTSequence(showNode,
                    new BTCheckSmoke(isInSmoke, agent)
                );

        AttackTree =
                new BTSequence(showNode,
                    new CheckPlayerRange(ClosestPlayer, hasWeapon, needsWeapon),
                    new BTMove(agent, ClosestPlayer, showNode, weaponHolder, hasWeapon, cantAttack, lighting, WeaponCol)
                );

        CheckWeapon =
                new BTSequence(showNode,
                    new FindWeapon(needsWeapon),
                    new BTGoForWeapon(agent, weapon, hasWeapon, needsWeapon, weaponHolder, weapon, lighting, CheckForWeaponCol)
                );

        PatrollTree =
                new BTSequence(showNode,
                    new BTPatrol(WayPoints, agent, lighting, PatrolCol)
                );

        
        rootNode = new BTSelector(new List<BTBaseNode> { CheckSmoke, CheckWeapon, AttackTree, PatrollTree });
        rootNode.Run();
    }

    private void FixedUpdate()
    {
        checkSmoke();
        FindVisibleTargets();
        rootNode?.Run();
    }

    void checkSmoke()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, 3, checkMask);
        if (hitColliders.Length > 0)
        {
            isInSmoke.active = true;
        }
        else
        {
            isInSmoke.active = false;
        }
    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        lowestDist = Mathf.Infinity;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        for (int i = 0; i < visibleTargets.Count; i++)
        {
            float dist = Vector3.Distance(visibleTargets[i].transform.position, transform.position);
            if (dist < lowestDist)
            {
                lowestDist = dist;
                ClosestPlayer.active = visibleTargets[i].gameObject;
            }
        }

        if (visibleTargets.Count == 0 && ClosestPlayer != null)
        {
            ClosestPlayer.active = null;
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
