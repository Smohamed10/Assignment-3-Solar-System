using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RotationController : MonoBehaviour
{
    public Transform[] planets; // Array of planet game objects
    public Transform sun; // Sun game object
    public float[] orbitSpeeds; // Array of orbit speeds for each planet
    public float[] selfRotationSpeeds; // Array of self-rotation speeds for each planet, including the sun

    public Text planetNameText; // Text object to display planet name
    public Text planetInfoText; // Text object to display planet information

    // Array of planet names
    private string[] planetNames = { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto", "Sun" };

    // Array of planet information
    private string[] planetInfo = {
        "Mercury is the closest planet to the sun and has extreme temperature variations between its scorching days and freezing nights.",
        "Venus is often called Earth's twin due to its similar size and composition, but its thick atmosphere traps heat, making it the hottest planet in our solar system.",
        "Earth is the only known planet with life, featuring vast oceans, diverse ecosystems, and a protective atmosphere crucial for sustaining living organisms.",
        "Mars is known as the Red Planet due to its rusty surface. It has polar ice caps, evidence of ancient water, and is a target for future human exploration.",
        "Jupiter is the largest planet in the solar system, with its iconic Great Red Spot, a massive storm larger than Earth, raging for centuries in its atmosphere.",
        "Saturn is famous for its dazzling rings made of icy particles and dust, which encircle the planet and provide a breathtaking sight in the night sky.",
        "Uranus is an ice giant with a unique feature: it rotates on its side, possibly due to a collision early in its history, causing extreme seasonal changes.",
        "Neptune, the farthest planet from the sun, has vivid blue hues due to methane in its atmosphere and powerful storms, like the infamous Great Dark Spot.",
        "Though no longer considered a planet, Pluto is a dwarf planet in the Kuiper Belt, with a complex geology, including icy plains, mountains, and possibly a subsurface ocean.",
        "The sun is the star at the center of our solar system, providing light, heat, and energy essential for life on Earth, powered by nuclear fusion in its core."
    };

    private InputAction touchInputAction;

    private void Awake()
    {
        // Initialize the touch input action
        touchInputAction = new InputAction(type: InputActionType.PassThrough, binding: "<Touchscreen>/press");
        touchInputAction.performed += ctx => OnTouchOrClick(ctx);
        touchInputAction.Enable();

        // For mouse input
        var mouseInputAction = new InputAction(type: InputActionType.PassThrough, binding: "<Mouse>/leftButton");
        mouseInputAction.performed += ctx => OnTouchOrClick(ctx);
        mouseInputAction.Enable();
    }

    private void Update()
    {
        // Rotate each planet around the sun and around its own axis
        for (int i = 0; i < planets.Length; i++)
        {
            // Orbit rotation
            planets[i].RotateAround(sun.position, Vector3.forward, orbitSpeeds[i] * Time.deltaTime);

            // Self-rotation
            planets[i].Rotate(Vector3.up, selfRotationSpeeds[i] * Time.deltaTime);
        }

        // Rotate the sun around its own axis
        sun.Rotate(Vector3.up, selfRotationSpeeds[planets.Length] * Time.deltaTime);
    }

    private void OnTouchOrClick(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = Vector2.zero;

        if (Touchscreen.current != null && Touchscreen.current.press.isPressed)
        {
            touchPosition = Touchscreen.current.position.ReadValue();
            Debug.Log("Touch detected at: " + touchPosition);
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            touchPosition = Mouse.current.position.ReadValue();
            Debug.Log("Mouse click detected at: " + touchPosition);
        }

        // Raycast to detect if any planet is touched/clicked
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            // Check if the hit object is one of the planets
            for (int i = 0; i < planets.Length; i++)
            {
                if (hit.collider.gameObject == planets[i].gameObject)
                {
                    Debug.Log("Planet detected: " + planets[i].name);
                    // Set the planet name text
                    planetNameText.text = planets[i].name;

                    // Set the planet information text
                    int index = System.Array.IndexOf(planetNames, planets[i].name);
                    if (index != -1)
                    {
                        planetInfoText.text = planetInfo[index];
                    }
                    return; // Exit after finding the touched planet
                }
            }
        }
        else
        {
            Debug.Log("No hit detected by raycast");
        }
    }
}
