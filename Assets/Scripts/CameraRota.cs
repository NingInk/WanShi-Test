using UnityEngine;

/// <summary>
/// 开发项目中使用的，修剪了一下
/// </summary>
public class CameraRota : MonoBehaviour
{
    public Vector3 initPos = new Vector3(-90, 580, 650);
    public Vector3 initRot = new Vector3(36, 180, 0);

    public float wheelSpeed = 5;
    public float MoveSpeed = 10;
    public float rotateSpeed = 5;
    public GameObject centerPoint;
    private Vector3 CameraPoint;
    private float angle;

    private void Awake()
    {
        self = GetComponent<Camera>();
    }

    private void Start()
    {
        transform.localPosition = initPos;
        transform.localRotation = Quaternion.Euler(initRot);
    }

    void Update()
    {
        if (Vector3.Distance(centerPoint.transform.position, transform.position) > 950)
        {
            transform.position = centerPoint.transform.position + (transform.position - centerPoint.transform.position).normalized * 950;
        }

        CameraFOV();
        float _mouseX = Input.GetAxis("Mouse X");
        float _mouseY = Input.GetAxis("Mouse Y");

        CameraRotate(_mouseX, _mouseY);

        NING_CameraMove();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h != 0 || v != 0)
        {
            transform.Translate(new Vector3(h * MoveSpeed, 0, v * MoveSpeed / 2));
        }
    }
    #region ...

    public float rotaSpeed = 1;
    public float timelag = 0.5f;
    float mX;
    float mY;
    Ray ray;
    RaycastHit hit;

    public GameObject target;
    Camera self;
    float cameraFoV;
    Bounds targetBounds;
    public Vector3 center = new Vector3();
    float timer = 0;
    bool begin = false;

    void NING_CameraMove()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int dis = (int)(Mathf.Abs(Vector3.Distance(center, transform.position)) / 10);

        mX = Input.GetAxis("Mouse X");
        mY = Input.GetAxis("Mouse Y");
        //绕自身旋转
        //按下alt和鼠标左键   绕视线中心旋转摄像机
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            float angle = Vector3.Angle(transform.forward, Vector3.up);
            if (Mathf.Abs(mX) > Mathf.Abs(mY))
            {
                transform.RotateAround(center, Vector3.up, mX * rotaSpeed);
            }

            if (angle < 100 && mY < 0 || angle > 170 && mY > 0 || 100 < angle && angle < 170)
            {
                if (Mathf.Abs(mX) < Mathf.Abs(mY))
                {
                    transform.RotateAround(center, transform.right, -mY * rotaSpeed);
                }
            }
        }
        //按下alt和鼠标右键   缩放摄像机
        else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(1))
        {
            if (Mathf.Abs(Vector3.Distance(transform.localPosition, center)) > 0.5)
            {
                if (mX > 0)
                {
                    transform.localPosition += transform.forward * Time.deltaTime * dis * 5;
                }
            }
            if (mX < 0)
            {
                transform.localPosition -= transform.forward * Time.deltaTime * dis * 5;
            }
        }
        //按下alt和鼠标中键   平移摄像机   
        else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(2))
        {
            transform.localPosition = transform.localPosition + transform.right * -mX * dis * Time.deltaTime;
            transform.localPosition = transform.localPosition + transform.up * -mY * dis * Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Red" || hit.collider.tag == "Blue")
                {
                    target = hit.collider.gameObject;
                }
            }
            SetPos();
        }
        else
        {
            if (target)
            {
                center = target.GetComponent<Collider>().bounds.center;
            }
            else
            {
                ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out hit))
                {
                    center = hit.point;
                }
            }
        }

        if (begin && timer < timelag)
        {
            timer += Time.deltaTime;
        }
        else if (timer > timelag)
        {
            begin = false;
        }

    }

    void SetPos()
    {
        if (target == null) return;

        cameraFoV = self.fieldOfView;

        targetBounds = target.GetComponent<Collider>().bounds;

        float zDistance = (Vector3.Distance(targetBounds.max, targetBounds.min) / 2) / Mathf.Tan((cameraFoV / 2));

        transform.position = targetBounds.center + (transform.position - targetBounds.center).normalized * 10 * Mathf.Abs(zDistance);

        transform.LookAt(targetBounds.center);
    }

    #endregion

    float add;


    /// <summary>
    /// 滚轮控制相机视角缩放
    /// </summary>
    public void CameraFOV()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * wheelSpeed * 1000;
        add += wheel;
        if (add < 650 && add > -1000)
        {
            //改变相机的位置
            this.transform.Translate(Vector3.forward * wheel);
        }
        else
        {
            add -= wheel;
        }
    }

    float xx;
    float yy;
    public static int IsRote = 0;

    /// <summary>
    /// 右键控制旋转
    /// </summary>
    /// <param name="_mouseX"></param>
    /// <param name="_mouseY"></param>
    public void CameraRotate(float _mouseX, float _mouseY)
    {
        angle = Vector3.Angle(transform.forward, centerPoint.transform.up);
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftAlt))
        {
            IsRote = 1;
        }
        if (Input.GetMouseButtonUp(1))
        {
            IsRote = -1;
        }
        if (IsRote == 1)
        {
            xx = _mouseX * rotateSpeed;
            if (xx > rotateSpeed)
            {
                xx = rotateSpeed;
            }
            yy = _mouseY * rotateSpeed;
            if (yy > rotateSpeed)
            {
                yy = rotateSpeed;
            }
            //控制相机绕中心点(centerPoint)水平旋转
            this.transform.RotateAround(CameraPoint, Vector3.up, _mouseX * rotateSpeed);

            //如果总角度超出指定范围，结束这一帧（！用于解决相机旋转到模型底部的Bug！）
            if (angle < 100 && _mouseY < 0 || angle > 170 && _mouseY > 0 || 100 < angle && angle < 170)
            {
                //控制相机绕中心点垂直旋转(！注意此处的旋转轴时相机自身的x轴正方向！)
                this.transform.RotateAround(CameraPoint, this.transform.right, -_mouseY * rotateSpeed);
            }

        }
        if (IsRote == -1)
        {
            if (xx > 0) { xx -= rotateSpeed * Time.deltaTime; }
            else { xx += rotateSpeed * Time.deltaTime; }

            if (yy > 0) { yy -= rotateSpeed * Time.deltaTime; }
            else { yy += rotateSpeed * Time.deltaTime; }

            //控制相机绕中心点(centerPoint)水平旋转
            transform.RotateAround(CameraPoint, Vector3.up, xx);
            if (xx <= 0.1 && xx > -0.1)
            {
                xx = 0;
                IsRote = 0;
            }
        }
    }
}
