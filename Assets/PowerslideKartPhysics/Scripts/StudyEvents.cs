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

            if (Input.GetKey("t")) {
                Debug.Log("TurnAMt: " + kart.GetTurnSpeed());
            }

            if (Input.GetKey("b"))
            {
                kart.SetBrake(1);

                Debug.Log("Force brake");

                /// <summary>Builds a master command and sends it, e.g. SendJob(2, 'H', 0.5f, 10f) -> "2:H.T0.5.10000ms"</summary>
                //int motorId = 2;
                //char mode = 'H';
                //float torque = 0.4f;
                //float durationMs = 2000f;
                //string t = torque.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
                //string commandBuilt = $"{motorId}:{char.ToUpperInvariant(mode)}.T{t}.{durationMs}ms";
                //Debug.Log(commandBuilt);
                //nucleo.SendCommand(commandBuilt);
                //nucleo.MoveUp();
                InitiateHapticOutput(2, 'H', 0.4f, 2000f);
            }

            if (braking)
            {
                kart.SetBrake(5);
            }



            if (kart.drifting) {
               // nucleo.MoveRight();
            }




        }

        // break on break trigger
        void OnTriggerEnter(Collider other)
        {

            HapticTrigger collidedTrigger = null;
            HapticTrigger.HapticType hapType = HapticTrigger.HapticType.Idle;
            
            if (other.GetComponent<HapticTrigger>())
            {
                collidedTrigger = other.GetComponent<HapticTrigger>();
                hapType = collidedTrigger.hapticType;


            }
            //if (other.GetComponent<BoostPad>()) { braking = false; }
            //Debug.Log("Entered: " + other.name);

            if (hapType == HapticTrigger.HapticType.Brake)
            {
                braking = true;
                StartCoroutine(BrakeForSeconds(3f));
                //nucleo.MoveFront(); "1:H.T-0.5.10s", "3:H.T-0.5.10s
                InitiateHapticOutput(1, 'H', -0.5f * collidedTrigger.intensityMultiplier, 1000f);
                InitiateHapticOutput(3, 'H', -0.5f * collidedTrigger.intensityMultiplier, 1000f);
            }

            if (hapType == HapticTrigger.HapticType.Jump)
            {
                //nucleo.MoveUp();
                InitiateHapticOutput(2, 'H', 0.7f*collidedTrigger.intensityMultiplier, 2000f);

                // also speed
                InitiateHapticOutput(1, 'H', 0.7f * collidedTrigger.intensityMultiplier, 1000f);
                InitiateHapticOutput(3, 'H', 0.7f * collidedTrigger.intensityMultiplier, 1000f);

            }

            if (hapType == HapticTrigger.HapticType.Landing)
            {
                //nucleo.MoveUp();
                nucleo.Pause();
                InitiateHapticOutput(4, 'H', 0.7f, 500f);

            }

            if (hapType == HapticTrigger.HapticType.DriftRight)
            {
                //nucleo.MoveRight(); ["3:H.T0.5.10s",  "1:H.T-0.3.10s"]),

                InitiateHapticOutput(3, 'H', 0.5f, 1000f);
                InitiateHapticOutput(1, 'H', -0.3f, 1000f);

            }



            if (hapType == HapticTrigger.HapticType.Speed)
            {
                //nucleo.MoveBack();
                // "3:H.T0.5.10s"

                float duration = 1600f * collidedTrigger.durationMultiplier;

                InitiateHapticOutput(1, 'H', 0.7f* collidedTrigger.intensityMultiplier, duration);
                InitiateHapticOutput(3, 'H', 0.7f* collidedTrigger.intensityMultiplier, duration);
            }


            if (hapType == HapticTrigger.HapticType.CurveRightStart)
            {

                nucleo.SendCommand("3:L.T.0.0>T0.3.2000ms");
                nucleo.SendCommand("1:L.T.0.0>T-0.15.2000ms");
                Debug.Log("CurveStartAt: "+Time.realtimeSinceStartup * 1000f);
            }

            if (hapType == HapticTrigger.HapticType.CurveRightPeak)
            {

                nucleo.SendCommand("3:L.T.0.3>T0.0.800ms");
                nucleo.SendCommand("1:L.T.-0.15>T0.0.800ms");
                Debug.Log("CurvePeakAt" +Time.realtimeSinceStartup * 1000f);
            }

        }

        void InitiateHapticOutput(int motorId, char mode, float torque, float durationMs)
        {

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