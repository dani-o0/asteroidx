using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    public float shipSpeed = 10f;
    
    static private PlayerShip _S;
    static public PlayerShip S
    {
        get
        {
            return _S;
        }
        private set
        {
            _S = value;
        }
    }

    Rigidbody rigid;
    public GameObject bulletPrefab;
    
    void Start()
    {
        S = this;
        
        rigid = GetComponent<Rigidbody>();
    }

    public void OnFire()
    {
        Fire();
    }

    public void OnMove(InputValue value)
    {
        Vector2 vel = value.Get<Vector2>();
        rigid.velocity = vel * shipSpeed;
    }

    void Fire()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = -Camera.main.transform.position.z;
        Vector3 mPos3D = Camera.main.ScreenToWorldPoint(mPos);
        
        SpawnBullet(mPos3D);
    }

    void SpawnBullet(Vector3 mPos3D)
    {
        GameObject go = Instantiate(bulletPrefab);
        go.transform.position = transform.position;
        go.transform.LookAt(mPos3D);
    }
}
