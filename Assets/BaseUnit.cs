using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class BaseUnit : MonoBehaviour
{
    protected NavMeshAgent nav;
    protected Animation ani;
    public string _name;
    public int bloodVolume = 10;//血量
    public int aggressivity = 2;//攻击力
    public float CD = 1;//攻击CD 
    public int speed = 1;//移动速度

    public Vector3 target;//目标

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animation>();
    }

    protected virtual void OnEnable()
    {
        ani.Play(Ani.idle);
    }

    Coroutine current;

    protected virtual void Update()
    {
        if (UIController.isPlaying)
        {
            if (current != null)
            {
                StopCoroutine(current);
                current = null;
            }
            if (ani.IsPlaying(Ani.idle))
                ani.CrossFade(Ani.walk, 0.2f);
            StartCoroutine(MakeAnInspectionTour());
        }
        if (ani.IsPlaying(Ani.walk))
        {
            nav.destination = transform.position + transform.forward;
            nav.speed = 0.5f;
        }
    }
    private void LateUpdate()
    {
        if (!ani.IsPlaying(Ani.die) && bloodVolume <= 0)
        {

            ani.CrossFade(Ani.die, 0.2f);
        }
        if (ani.IsPlaying(Ani.die) && ani[Ani.die].normalizedTime >= 0.9f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Attack()
    {

    }

    protected virtual void FollowUp()
    {

    }

    /// <summary>
    /// 巡视逻辑
    /// </summary>
    /// <returns></returns>
    IEnumerator MakeAnInspectionTour()
    {
        while (true)
        {
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
    //if (ani.IsPlaying(Ani.idle) && ani[Ani.idle].normalizedTime >= 1)
    //{
    //     ani.CrossFade(Ani.walk, 0.2f);
    //}
}
