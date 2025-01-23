using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float cutoutSize = 0.1f;
    [SerializeField] private float falloffSize = 0.05f;
    private Camera mainCamera;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    //get the target position and change material of object on front of the target to cutout a circle for sight
    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; m++)
            {
                materials[m].SetVector("_Cutout_Position", cutoutPos);
                materials[m].SetFloat("_Cutout_Size", cutoutSize);
                materials[m].SetFloat("_Falloff_Size", falloffSize);
            }
        }
    }

    #endregion
    
}
