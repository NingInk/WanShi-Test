using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class BaseUnit : MonoBehaviour
{
    AniState currnetState = AniState.None;

    protected NavMeshAgent nav;
    protected Animation ani;
    public string _name;
    public int bloodVolume = 10;//血量
    public int aggressivity = 2;//攻击力
    public float CD = 1;//攻击CD 
    public int speed = 1;//移动速度
    [Header("视野")]
    public float view = 2;
    [Header("攻击")]
    public float act = 0.5f;
    public GameObject target;//目标

    AniState tempState;

    float timer;
    List<GameObject> enemys;
    protected virtual void Awake()
    {
        enemys = new List<GameObject>();
        GetComponentInChildren<Weapon>().master = gameObject;
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animation>();
    }

    protected virtual void OnEnable()
    {
        nav.stoppingDistance = act;
        ani.Play(Ani.idle);
        if (UIController.IsPlaying)
        {
            currnetState = AniState.Tour;
        }
        else
        {
            UIController.playAct += () => { currnetState = AniState.Tour; };
        }
    }

    Coroutine current;

    protected virtual void Update()
    {

        ObserveBlood();
        if (ani.IsPlaying(Ani.die))
        {
            if (ani[Ani.die].normalizedTime >= 0.9f)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (!target)
        {
            foreach (var enemy in enemys)
            {
                if (enemy)
                {
                    target = enemy;
                }
            }
        }

        switch (currnetState)
        {
            case AniState.None:
                break;
            case AniState.Tour:
                if (!ani.IsPlaying(Ani.walk))
                {
                    ani.CrossFade(Ani.walk, 0.2f);
                    if (current != null)
                    {
                        StopCoroutine(current);
                        current = null;
                    }
                    StartCoroutine(MakeAnInspectionTour());
                }
                nav.destination = transform.position + transform.forward;
                nav.speed = 0.5f * speed;
                Debug.Log(nav.speed);
                break;
            case AniState.Trace:
                if (!target)
                {
                    currnetState = AniState.Tour;
                    return;
                }
                if (!ani.IsPlaying(Ani.run))
                {
                    ani.CrossFade(Ani.run, 0.2f);
                }
                nav.destination = target.transform.position;
                nav.speed = 1 * speed;

                if (Vector3.Distance(target.transform.position, transform.position) <= act)
                {
                    currnetState = AniState.Attack;
                }

                break;
            case AniState.Attack:
                if (!target)
                {
                    currnetState = AniState.Tour;
                    return;
                }
                if (Vector3.Distance(target.transform.position, transform.position) > view)
                {
                    currnetState = AniState.Trace; return;
                }
                if (!(ani.IsPlaying(Ani.attack1) || ani.IsPlaying(Ani.idle)))
                {
                    att = false;
                    ani.CrossFade(Ani.attack1, 0.2f);
                }
                if (ani.IsPlaying(Ani.attack1))
                {
                    if (ani[Ani.attack1].normalizedTime >= 0.5f && !att)
                    {
                        Attack();
                        att = true;
                    }
                    if (ani[Ani.attack1].normalizedTime >= 0.8f)
                    {
                        ani.CrossFade(Ani.idle, 0.2f);
                        timer = 0;
                    }
                }

                if (ani.IsPlaying(Ani.idle))
                {
                    timer += Time.deltaTime;
                    if (timer >= CD)
                    {
                        timer = 0;
                        ani.CrossFade(Ani.attack1, 0.2f);
                        att = false;
                    }
                }
                break;
            case AniState.Death:
                if (!ani.IsPlaying(Ani.die))
                {
                    ani.CrossFade(Ani.die, 0.2f);
                }
                nav.speed = 0;
                break;
            case AniState.Danmage:
                if (!ani.IsPlaying(Ani.danmage))
                {
                    ani.CrossFade(Ani.danmage, 0.2f);
                }
                if (ani.IsPlaying(Ani.danmage) && ani[Ani.danmage].normalizedTime >= 0.9f)
                {
                    currnetState = tempState;
                }
                break;
            default:
                break;
        }



    }
    bool att;

    protected virtual void Attack()
    {

    }

    public void SetBlood(int i, GameObject enemy)
    {
        if (target)
        {
            enemys.Add(enemy);
        }
        else
        {
            target = enemy;
        }
        bloodVolume -= i;

        tempState = currnetState;
        currnetState = AniState.Danmage;
    }

    protected virtual void ObserveBlood()
    {
        if (bloodVolume <= 0)
        {
            currnetState = AniState.Death;
        }
    }

    /// <summary>
    /// 巡视逻辑
    /// </summary>
    /// <returns></returns>
    IEnumerator MakeAnInspectionTour()
    {
        while (true)
        {
            foreach (Transform child in transform.tag == "Red" ? UIController.Instance.BlueBornAt.transform : UIController.Instance.redBornAt.transform)
            {
                if (Vector3.Distance(child.position, transform.position) <= view)
                {
                    target = child.gameObject;
                    currnetState = AniState.Trace;
                    yield break;
                }
            }



            Ray ray = new Ray(transform.position + (Vector3.up * 0.25f), transform.forward);
            RaycastHit hit /*= GetHit(ray)*/;

            if (Physics.Raycast(ray, out hit, 1f))
            {
                if (hit.collider.gameObject.name == "Cube(Clone)")
                {
                    //ray = new Ray(transform.position + (Vector3.up * 0.25f), transform.right);
                    //if (Physics.Raycast(ray, out hit, 1f))
                    {
                        transform.localEulerAngles += Vector3.up * 3 * Time.deltaTime;
                    }
                    //ray = new Ray(transform.position + (Vector3.up * 0.25f), transform.right);

                }
            }
            yield return null;
        }
    }

    RaycastHit GetHit(Ray ray)
    {
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1f);
        return hit;
    }
}

public enum AniState
{
    None, Tour, Trace, Attack, Death, Danmage
}