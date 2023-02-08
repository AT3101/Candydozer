using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    const int  MaxShotPower = 5;
    const int RecoverySeconds = 3;
    int shotPower = MaxShotPower;
    //フィールド生成
    public GameObject[] candyPrefabs;
    public Transform candyParentTransform;
    public CandyManager candyManager;
    public float shotForce;
    public float shotTorque;
    public float baseWidth;//制作したステージの幅

    void Update()
    {
        if(Input.GetButtonDown("Fire1")) shot();
    }
    //戻り値の型
    GameObject SampleCandy()
    {
        int index = Random.Range(0,candyPrefabs.Length);
        return candyPrefabs[index];
    }

    Vector3 GetInstantiatePosition()
    {
        float x = baseWidth *
            (Input.mousePosition.x / Screen.width) - (baseWidth / 2);
            //Debug.Log(Input.mousePosition.x+";"+Input.mousePosition.y);
            //Debug.Log(Screen.width);
            //Debug.Break();
        return transform.position + new Vector3(x,0,0);
    }
    public void shot()
    {
        if(candyManager.GetCandyAmount() <= 0) return;
        if(shotPower <= 0) return;

        GameObject candy = (GameObject)Instantiate(
            SampleCandy(),
            GetInstantiatePosition(),
            Quaternion.identity
        );
    candy.transform.parent = candyParentTransform;

    Rigidbody candyRigidBody = candy.GetComponent<Rigidbody>();
    candyRigidBody.AddForce(transform.forward * shotForce);
    candyRigidBody.AddTorque(new Vector3(0,shotTorque,0));

    candyManager.ConsumeCandy();

    ConsumPower();

    }
    void OnGUI()
    {
        GUI.color = Color.black;

        string label = "";
        for(int i=0;i<shotPower;i++)label = label+"+";
            GUI.Label(new Rect(50,65,100,30),label);
    }
    void ConsumPower()
    {
        shotPower--;
        StartCoroutine(RecoverPower());
    }
    IEnumerator RecoverPower()
    {
        yield return new WaitForSeconds(RecoverySeconds);
        shotPower++;
    }
}
