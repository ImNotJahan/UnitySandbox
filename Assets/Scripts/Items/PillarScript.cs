using UnityEngine;
using System.Collections;

public class PillarScript : MonoBehaviour
{
    public void Interacted(MouseLook mouseLook)
    {
        StartCoroutine(Disolve(mouseLook));
    }

    IEnumerator Disolve(MouseLook mouseLook)
    {
        for(float k = -1; k < 1; k += 0.01f)
        {
            gameObject.GetComponent<MeshRenderer>().materials[1].SetFloat("DisolveAmount", k);
            yield return new WaitForSeconds(0.007f);
        }

        gameObject.GetComponent<BoxCollider>().size = new Vector3(0.02f, 0.02f, 0.08f);
        gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.04f);

        StartCoroutine(BringIntoGround());
        StartCoroutine(mouseLook.CameraShake(0.02f * transform.localScale.y * 2.5f, .1f));
    }

    IEnumerator BringIntoGround()
    {
        while(transform.position.y > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 0.1f);
            yield return new WaitForSeconds(0.02f);
        }

        gameObject.SetActive(false);
    }
}
