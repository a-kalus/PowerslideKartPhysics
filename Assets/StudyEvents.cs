using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerslideKartPhysics
{
    [RequireComponent(typeof(Kart))]
    public class StudyEvents : MonoBehaviour
    {

        Kart kart;
        public USBStringSender nucleo;
        bool braking = false;
        // Start is called before the first frame update
        void Awake()
        {
            kart = GetComponent<Kart>();
        }

        // Update is called once per frame
        void Update()
        {


            if (Input.GetKey("space"))
            {
                kart.SetSteer(1);
                kart.SetDrift(true);
                Debug.Log("Force drift");
            }

            if (Input.GetKey("q"))
            {
                kart.SetBrake(1);

                Debug.Log("Force brake");

                /// <summary>Builds a master command and sends it, e.g. SendJob(2, 'H', 0.5f, 10f) -> "2:H.T0.5.10000ms"</summary>
                int motorId = 2;
                char mode = 'H';
                float torque = 0.4f;
                float durationMs = 2000f;
                string t = torque.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
                string commandBuilt = $"{motorId}:{char.ToUpperInvariant(mode)}.T{t}.{durationMs}ms";
                Debug.Log(commandBuilt);
                nucleo.SendCommand(commandBuilt);
                //nucleo.MoveUp();
    }

            if (braking)
            {
                kart.SetBrake(5);
            }




        }

        // break on break trigger
        void OnTriggerEnter(Collider other)
        {
            //if (other.GetComponent<BoostPad>()) { braking = false; }

            if (other.tag == "BrakePoint")
            {
                braking = true;
                StartCoroutine(BrakeForSeconds(3f));
            }
            Debug.Log("Entered: " + other.name);


        }

        void InitiateHapticOutput(int motorId, char mode, float torque, float durationMs) {

            string t = torque.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
            string commandBuilt = $"{motorId}:{char.ToUpperInvariant(mode)}.T{t}.{durationMs}ms";
            Debug.Log(commandBuilt);
            nucleo.SendCommand(commandBuilt);
        }

        IEnumerator BrakeForSeconds(float duration)
        {
       

            yield return new WaitForSeconds(duration);

            // end effect
            Debug.Log("effect off");
            braking = false;

        }
    }
}