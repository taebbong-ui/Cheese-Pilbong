using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoteObject2_T : MonoBehaviour
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
                        GameManager2_T.instance.NormalHit();
                        Instantiate(hitEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), hitEffect.transform.rotation);
                    }
                    else if (Mathf.Abs(transform.position.y) > 0.05f)
                    {
                        Debug.Log("Good");
                        GameManager2_T.instance.GoodHit();
                        Instantiate(goodEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), goodEffect.transform.rotation);

                    }
                    else
                    {
                        Debug.Log("Perfect");
                        GameManager2_T.instance.PerfectHit();
                        Instantiate(perfectEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), perfectEffect.transform.rotation);
                    }
                }

                else if (this.gameObject.name.StartsWith("Right"))
                {
                    if (Mathf.Abs(transform.position.y) > 0.25)
                    {
                        Debug.Log("Hit");
                        GameManager2_T.instance.NormalHit();
                        Instantiate(hitEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), hitEffect.transform.rotation);
                    }
                    else if (Mathf.Abs(transform.position.y) > 0.05f)
                    {
                        Debug.Log("Good");
                        GameManager2_T.instance.GoodHit();
                        Instantiate(goodEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), goodEffect.transform.rotation);
                    }
                    else
                    {
                        Debug.Log("Perfect");
                        GameManager2_T.instance.PerfectHit();
                        Instantiate(perfectEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), perfectEffect.transform.rotation);
                    }
                }
            }
        }
    }


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
                    GameManager2_T.instance.NoteMissed();
                    Instantiate(missedEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), missedEffect.transform.rotation);
                }
                else if (this.gameObject.name.StartsWith("Right"))
                {
                    GameManager2_T.instance.NoteMissed();
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