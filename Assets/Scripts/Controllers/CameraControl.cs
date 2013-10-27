    using UnityEngine;

 

    public class CameraControl : MonoBehaviour

    {

        

 

        // Update is called once per frame

        void Update()

        {

            // Init camera translation for this frame.

            var translation = Vector3.zero;
		
			var zoomDelta = Input.GetAxis("Mouse ScrollWheel")*Settings.ZoomSpeed*Time.deltaTime;
			
            if (zoomDelta!=0)

            {
                translation -= Vector3.up * Settings.ZoomSpeed * zoomDelta;
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

            if (desiredPosition.x < 15 || Settings.LevelArea < desiredPosition.x)

            {

                translation.x = 0;

            }

            if (desiredPosition.y <Settings.ZoomMin || Settings.ZoomMax < desiredPosition.y)

            {

                translation.y = 0;

            }

            if (desiredPosition.z < -15 || (!GameControl.IsMulti || PhotonNetwork.isMasterClient ? Settings.LevelArea : Settings.LevelArea+30) < desiredPosition.z)

            {

                translation.z = 0;

            }

            // Finally move camera parallel to world axis
			if(GameControl.GameStarted()) {
            	camera.transform.position += translation;
			}
        }

    }