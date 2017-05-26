using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    private Camera thisCamera;
    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    private bool doMovement = true;

    public float scrollSpeed = 5f;

    public bool isOrthographic = true;
    public float minSize;
    public float maxSize;

    public float minHeight;
    public float maxHeight;
    public float orthSizeDivide;
    private float startOrthSize;
    private float currentOrthSize;


    public Vector3 mapMinBounds;
    public Vector3 mapMaxBounds;

    private Vector3 startTransform;
    private float fromStartDist;

    public static bool gameView = true;
    private Transform player;

    public KeyCode tpToBase;


    private void Awake()
    {
        thisCamera = this.GetComponent<Camera>();
        if (thisCamera == null)
        {
            Debug.LogError("No camera component you dingus!");
        }
    }
    private void Start()
    {
        startOrthSize = Camera.main.orthographicSize;
        currentOrthSize = startOrthSize;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        startTransform = transform.position;

        mapMaxBounds += transform.position;
        mapMinBounds += transform.position;
        //TODO: Get Cam min max bounds from middle of map using the 
    }

    void Update () {


        //TODO:Add functions to tp to base, ect ect. Currently only switches between game view.
       // if (Input.GetKeyDown(KeyCode.Tab))
       //     gameView = !gameView;

        if (gameView == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //Press Escape to toggle movement
                doMovement = !doMovement;

            if (doMovement == false)
                return;

            fromStartDist = (transform.position - startTransform).magnitude;
            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                if (transform.position.y < mapMaxBounds.y)
                transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                if (transform.position.y > mapMinBounds.y)
                    transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                if (transform.position.x < mapMaxBounds.x)
                    transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            {
                if (transform.position.x > mapMinBounds.x)
                    transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 cameraVel = new Vector3(dir.x, dir.y, 0);
            transform.Translate(cameraVel * (panSpeed * 4f) * Time.deltaTime, Space.World);
        }

        //if (GameManager.gameEnded)
       // {
       //     this.enabled = false;
       //     return;
       // }

        if (isOrthographic == true)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            currentOrthSize -= scroll * 500 * scrollSpeed * Time.deltaTime;
            currentOrthSize = Mathf.Clamp(currentOrthSize, minSize, maxSize);
            Camera.main.orthographicSize = currentOrthSize;
        }
        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            currentOrthSize -= scroll * 500 * scrollSpeed * Time.deltaTime;
            currentOrthSize = Mathf.Clamp(currentOrthSize, minHeight, maxHeight);
            Vector3 pos = new Vector3(transform.position.x, currentOrthSize, transform.position.z);

            transform.position = pos;
        }


    }
}
