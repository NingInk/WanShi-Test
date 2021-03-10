using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class Unit_Base : MonoBehaviour
{
    public AniState currnetState = AniState.None;

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
    public int m_angle = 10;
    public int rot_speed = 1;
    AniState tempState;
    Vector3 tempPos;
    float timer;
    List<GameObject> enemys;
    public Image i_blood;

    float totalBlood;
    protected virtual void Awake()
    {
        totalBlood = bloodVolume;
        i_blood = GetComponentInChildren<Image>();
        enemys = new List<GameObject>();
        GetComponentInChildren<Weapon_Base>().master = gameObject;
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animation>();
    }

    protected virtual void OnEnable()
    {
        nav.stoppingDistance = act;
        ani.Play(Ani.idle);
        if (Controller.IsPlaying)
        {
            currnetState = AniState.Tour;
        }
        else
        {
            Controller.playAct += () => { currnetState = AniState.Tour; };
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

        if (target == null)
        {
            foreach (var enemy in enemys)
            {
                if (enemy)
                {
                    target = enemy;
                    break;
                }
            }
            if (Controller.IsPlaying && !target)
            {
                currnetState = AniState.Tour;
            }
        }
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= act)
            {
                Ray ray = new Ray(transform.position + Vector3.up * 0.25f, target.transform.position - transform.position);
                RaycastHit hit;
                Vector3 dir = target.transform.position - transform.position;
                if (Vector3.Angle(transform.forward, dir) > m_angle)
                {
                    if (Vector3.Cross(transform.forward, dir).y < 0)
                    {
                        transform.localEulerAngles = transform.localEulerAngles -= Vector3.up * Time.deltaTime * rot_speed;
                    }
                    else
                    {
                        transform.localEulerAngles = transform.localEulerAngles += Vector3.up * Time.deltaTime * rot_speed;
                    }
                }
                else if (Physics.Raycast(ray, out hit) && hit.collider.tag != target.tag)
                {
                    currnetState = AniState.Trace;
                }
                else
                {
                    currnetState = AniState.Attack;
                }
            }
            if (Vector3.Distance(target.transform.position, transform.position) > view)
            {
                currnetState = AniState.Trace;
            }
        }

        switch (currnetState)
        {
            case AniState.None:
                if (!Controller.IsPlaying)
                {
                    ani.CrossFade(Ani.idle, 0.2f);
                }
                break;
            case AniState.Tour:
                if (!ani.IsPlaying(Ani.walk))
                {
                    tempPos = Controller.floors[Random.Range(0, Controller.floors.Length)].transform.position;
                    ani.CrossFade(Ani.walk, 0.2f);
                    if (current != null)
                    {
                        StopCoroutine(current);
                        current = null;
                    }
                    StartCoroutine(MakeAnInspectionTour());
                }
                if (Vector3.Distance(transform.position, tempPos) < act)
                {

                    tempPos = Controller.floors[Random.Range(0, Controller.floors.Length)].transform.position;
                }

                nav.destination = tempPos/*transform.position + transform.forward*/;

                nav.speed = 0.5f * speed;
                break;
            case AniState.Trace:
                if (!(ani.IsPlaying(Ani.run) || ani.IsPlaying(Ani.idle)))
                {
                    ani.CrossFade(Ani.run, 0.2f);
                }
                nav.destination = target.transform.position;
                nav.speed = 1 * speed;
                break;
            case AniState.Attack:
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
                    Debug.Log("CD中......");
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
        if (currnetState != AniState.Death)
        {
            currnetState = AniState.Danmage;
        }
    }

    protected virtual void ObserveBlood()
    {
        if (bloodVolume <= 0)
        {
            currnetState = AniState.Death;
            if (!ani.IsPlaying(Ani.die))
            {
                ani.CrossFade(Ani.die, 0.2f);
            }
            nav.speed = 0;
        }
        i_blood.fillAmount = bloodVolume / totalBlood;
    }

    /// <summary>
    /// 巡视逻辑
    /// </summary>
    /// <returns></returns>
    IEnumerator MakeAnInspectionTour()
    {
        while (true)
        {
            foreach (Transform child in transform.tag == "Red" ? Controller.Instance.BlueBornAt.transform : Controller.Instance.redBornAt.transform)
            {
                if (Vector3.Distance(child.position, transform.position) <= view)
                {
                    target = child.gameObject;
                    currnetState = AniState.Trace;
                    yield break;
                }
            }



            Ray ray = new Ray(transform.position + (Vector3.up * 0.25f), transform.forward);
            RaycastHit hit, lhit, rhit;

            if (Physics.Raycast(ray, out hit, 1f))
            {
                if (hit.collider.gameObject.tag == "Wall")
                {
                    Ray rray = new Ray(transform.position + (Vector3.up * 0.25f), transform.right);
                    Ray lray = new Ray(transform.position + (Vector3.up * 0.25f), -transform.right);
                    Physics.Raycast(lray, out lhit, Mathf.Infinity);
                    Physics.Raycast(rray, out rhit, Mathf.Infinity);
                    if (Vector3.Distance(lhit.point, transform.position) > Vector3.Distance(rhit.point, transform.position))
                    {
                        transform.localEulerAngles -= Vector3.up * 90 * Time.deltaTime;
                    }
                    else
                    {
                        transform.localEulerAngles += Vector3.up * 90 * Time.deltaTime;
                    }
                }
            }
            yield return null;
        }
    }

}

public enum AniState
{
    None, Tour, Trace, Attack, Death, Danmage
}