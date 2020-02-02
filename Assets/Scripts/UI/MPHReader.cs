namespace Assets.Scripts.UI
{
    using Assets.Scripts.Driving;

    using UnityEngine;
    using UnityEngine.UI;

    public class MPHReader : MonoBehaviour
    {
        private Car car;

        [SerializeField]
        private Text text;

        // Start is called before the first frame update
        private void Start()
        {
            this.car = GameObject.FindObjectOfType<Car>();
        }

        // Update is called once per frame
        private void Update()
        {
            this.text.text = this.car.CurrentSpeed.ToString();
        }
    }
}