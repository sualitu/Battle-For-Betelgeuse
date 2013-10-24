    using UnityEngine;

 

    public class CameraControl : MonoBehaviour

    {

        private const int LevelArea = 90;

 

        private const int ScrollArea = 25;

        private const int ScrollSpeed = 25;

        private const int DragSpeed = 80;

 

        private const int ZoomSpeed = 25;

        private const int ZoomMin = 8;

        private const int ZoomMax = 70;

 

        private const int PanSpeed = 50;

        private const int PanAngleMin = 35;

        private const int PanAngleMax = 60;

 

        // Update is called once per frame

        void Update()

        {

            // Init camera translation for this frame.

            var translation = Vector3.zero;
		
			var zoomDelta = Input.GetAxis("Mouse ScrollWheel")*ZoomSpeed*Time.deltaTime;
			
            if (zoomDelta!=0)

            {
                translation -= Vector3.up * ZoomSpeed * zoomDelta;
            }
		
			     // Move camera with arrow keys
			if(GameControl.IsMulti) {
				if(PhotonNetwork.isMasterClient) {
            		translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				} else {
            		translation += new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
				}
			} else {
            	translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			}
 
			// Keep camera within level and zoom area

            var desiredPosition = camera.transform.position + translation;

            if (desiredPosition.x < 15 || LevelArea < desiredPosition.x)

            {

                translation.x = 0;

            }

            if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)

            {

                translation.y = 0;

            }

            if (desiredPosition.z < -10 || (!GameControl.IsMulti || PhotonNetwork.isMasterClient ? LevelArea : LevelArea+30) < desiredPosition.z)

            {

                translation.z = 0;

            }

            // Finally move camera parallel to world axis
			if(GameControl.GameStarted()) {
            	camera.transform.position += translation;
			}
        }

    }