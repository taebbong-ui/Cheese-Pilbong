using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject_T : MonoBehaviour
{
    public bool canBePressed;

    public KeyCode keyToPress;

    private bool obtained = false;

    public GameObject hitEffect, goodEffect, perfectEffect, missedEffect;

    private Vector3 startPosition;



    void Start()
    {
        startPosition = transform.position; 
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                obtained = true;
                gameObject.SetActive(false);

                if (gameObject.name.StartsWith("Left"))
                {
                    if (Mathf.Abs(transform.position.y) > 0.25)
                    {
                        Debug.Log("Hit");
                        GameManager_T.instance.NormalHit();
                        Instantiate(hitEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), hitEffect.transform.rotation);
                    }
                    else if (Mathf.Abs(transform.position.y) > 0.05f)
                    {
                        Debug.Log("Good");
                        GameManager_T.instance.GoodHit();
                        Instantiate(goodEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), goodEffect.transform.rotation);

                    }
                    else
                    {
                        Debug.Log("Perfect");
                        GameManager_T.instance.PerfectHit();
                        Instantiate(perfectEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), perfectEffect.transform.rotation);
                    }
                }

                else if (this.gameObject.name.StartsWith("Right"))
                {
                    if (Mathf.Abs(transform.position.y) > 0.25)
                    {
                        Debug.Log("Hit");
                        GameManager_T.instance.NormalHit();
                        Instantiate(hitEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), hitEffect.transform.rotation);
                    }
                    else if (Mathf.Abs(transform.position.y) > 0.05f)
                    {
                        Debug.Log("Good");
                        GameManager_T.instance.GoodHit();
                        Instantiate(goodEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), goodEffect.transform.rotation);
                    }
                    else
                    {
                        Debug.Log("Perfect");
                        GameManager_T.instance.PerfectHit();
                        Instantiate(perfectEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), perfectEffect.transform.rotation);
                    }
                }
            }
        }
    }
                /*if(Input.GetKeyDown(keyToPress))
                {
                    if (canBePressed)
                    {
                        obtained = true;
                        gameObject.SetActive(false);

                        //GameManager.instance.NoteHit();

                        if(Mathf.Abs(transform.position.y) > 0.25)
                        {
                            Debug.Log("Hit");
                            GameManager.instance.NormalHit();
                            Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                        }
                        else if(Mathf.Abs(transform.position.y) > 0.05f)
                        {
                            Debug.Log("Good");
                            GameManager.instance.GoodHit();
                            Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);

                        }
                        else
                        {
                            Debug.Log("Perfect");
                            GameManager.instance.PerfectHit();
                            Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                        }
                    }        
                }*/
          

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;
            if (!obtained)
            {
                if (gameObject.name.StartsWith("Left"))
                {
                    GameManager_T.instance.NoteMissed();
                    Instantiate(missedEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), missedEffect.transform.rotation);
                }
                else if (this.gameObject.name.StartsWith("Right"))
                {
                    GameManager_T.instance.NoteMissed();
                    Instantiate(missedEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), missedEffect.transform.rotation);
                }
            }  
        }
    }

    public void ResetNote()
    {
        canBePressed = false;
        obtained = false;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
}
