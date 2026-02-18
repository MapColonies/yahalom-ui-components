# Yahalom UI Components

A custom Unity UI component library for MapColonies projects with built-in Hebrew (RTL) language support.

**Package:** `com.mapcolonies.yahalom-ui`
**Version:** 1.0.2
**Minimum Unity Version:** 6000.1

## Table of Contents

- [Installation](#installation)
- [Package Structure](#package-structure)
- [UI Components](#ui-components)
  - [YahalomButton](#yahalombutton)
  - [YahalomInputField](#yahalominputfield)
  - [Compass](#compass)
- [Fonts](#fonts)
- [Testing](#testing)
- [API Reference](#api-reference)

---

## Installation

### Option 1: Git URL (Recommended)

Add to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mapcolonies.yahalom-ui": "https://github.com/MapColonies/yahalom-ui-components.git?path=Assets/YahalomUIPackage"
  }
}
```

### Option 2: Local Package

1. Copy the `YahalomUIPackage` folder to your project's `Packages` directory
2. Unity will automatically detect and import the package

---

## Package Structure

```
YahalomUIPackage/
├── Runtime/
│   ├── YahalomButton/
│   │   └── YahalomButton.cs           # Button component script
│   ├── YahalomInputField/
│   │   └── YahalomInputField.cs       # Input field component script
│   ├── Compass/
│   │   └── Compass.cs                 # Compass component script
│   └── Resources/
│       ├── YahalomButton/
│       │   └── YahalomButton.prefab   # Button prefab
│       ├── YahalomInputField/
│       │   └── YahalomInputField.prefab # Input field prefab
│       └── Compass/
│           └── Compass.prefab         # Compass prefab
├── Editor/
│   └── YahalomButton/
│       └── YahalomButtonEditor.cs     # Custom inspector
├── Fonts/                             # Hebrew fonts (NotoSansHebrew)
├── UI/PNG/                            # UI sprite assets
├── Atlas/                             # Sprite atlas
└── package.json                       # Package configuration
```

---

## UI Components

### YahalomButton

A customizable button component with state-based color transitions for text and icons, supporting RTL text.

**Location:** `Runtime/YahalomButton/YahalomButton.cs`
**Prefab:** `Runtime/Resources/YahalomButton/YahalomButton.prefab`

#### Features

- Extends Unity's native `Button` class
- Separate color configurations for text and icon per state
- Smooth color transitions with configurable fade duration
- RTL text support via TextMeshPro
- Custom inspector for easy configuration

#### Button States

| State | Description |
|-------|-------------|
| Normal | Default appearance |
| Highlighted | Hover/focus state |
| Pressed | Click/tap state |
| Selected | Selected state |
| Disabled | Inactive state |

#### Usage

**Adding to Scene:**

1. **Via Prefab:** Drag `YahalomButton.prefab` from `Runtime/Resources/YahalomButton/` into your Canvas
2. **Via Code:**
   ```csharp
   var buttonPrefab = Resources.Load<GameObject>("YahalomButton/YahalomButton");
   var button = Instantiate(buttonPrefab, parentCanvas.transform);
   ```

**Configuring in Inspector:**

1. Select the YahalomButton in the hierarchy
2. In the Inspector, configure:
   - **Text Mesh:** Reference to the TextMeshProUGUI component
   - **Icon:** Reference to the icon Image component
   - **Text Colors:** Set colors for each button state
   - **Icon Colors:** Set colors for each button state
   - **Fade Duration:** Transition time between states (default: 0.1s)

#### Prefab Hierarchy

```
YahalomButton (Button, Image, HorizontalLayoutGroup)
├── Text - RTLTMP (TextMeshProUGUI)
└── Icon (Image)
```

---

### YahalomInputField

A customizable input field component with state-based color transitions, error validation support, and a clear button.

**Location:** `Runtime/YahalomInputField/YahalomInputField.cs`
**Prefab:** `Runtime/Resources/YahalomInputField/YahalomInputField.prefab`

#### Features

- Extends Unity's `TMP_InputField` class
- State-based text and placeholder color transitions
- Built-in error state with custom validation predicate
- Clear/close button to reset input content
- RTL text support via TextMeshPro

#### Input States

| State | Description |
|-------|-------------|
| Normal | Default appearance |
| Highlighted | Hover/focus state |
| Pressed | Click/tap state |
| Selected | Active/selected state with content |
| Disabled | Inactive state |
| Error | Validation failed state |

#### Usage

**Adding to Scene:**

1. **Via Prefab:** Drag `YahalomInputField.prefab` from `Runtime/Resources/YahalomInputField/` into your Canvas
2. **Via Code:**
   ```csharp
   var inputPrefab = Resources.Load<GameObject>("YahalomInputField/YahalomInputField");
   var inputField = Instantiate(inputPrefab, parentCanvas.transform);
   ```

**Setting Up Error Validation:**

```csharp
using YahalomUIPackage.Runtime.YahalomInputField;

// Get reference to input field component
YahalomInputField inputField = GetComponent<YahalomInputField>();

// Set custom error validation (e.g., minimum length)
inputField.SetErrorPredicate(value => value.Length < 3);

// Or validate email format
inputField.SetErrorPredicate(value => !value.Contains("@"));
```

#### Public API

| Method | Parameters | Description |
|--------|------------|-------------|
| `SetErrorPredicate(Func<string, bool>)` | `predicate`: validation function | Sets the error validation function. Returns true if input is invalid. |

#### Inspector Properties

| Property | Description |
|----------|-------------|
| Close Button | Reference to the clear/delete button |
| Normal Text Color | Text color in default state |
| Highlighted Text Color | Text color when focused/hovered |
| Pressed Text Color | Text color when pressed |
| Selected Text Color | Text color when selected with content |
| Disabled Text Color | Text color when disabled |
| Error Text Color | Text color in error state |
| Error Image Color | Background color in error state |

---

### Compass

A navigation compass component displaying heading direction, angle, and geographic coordinates.

**Location:** `Runtime/Compass/Compass.cs`
**Prefab:** `Runtime/Resources/Compass/Compass.prefab`

#### Features

- 8-point directional system (N, NE, E, SE, S, SW, W, NW)
- Smooth angle interpolation
- Latitude/Longitude coordinate display
- Debug mode for testing
- Auto-rotating direction labels

#### Usage

**Adding to Scene:**

1. **Via Prefab:** Drag `Compass.prefab` from `Runtime/Resources/Compass/` into your Canvas
2. **Via Code:**
   ```csharp
   var compassPrefab = Resources.Load<GameObject>("Compass/Compass");
   var compass = Instantiate(compassPrefab, parentCanvas.transform);
   ```

**Controlling the Compass:**

```csharp
using YahalomUIPackage.Runtime.Compass;

// Get reference to compass component
Compass compass = GetComponent<Compass>();

// Set heading (0-360 degrees, 0 = North)
compass.SetHeading(45f);  // Points NE

// Set coordinates
compass.SetCoordinates(new Vector2(35.2150f, 31.7825f));  // (longitude, latitude)
```

#### Public API

| Method | Parameters | Description |
|--------|------------|-------------|
| `SetHeading(float)` | `heading`: 0-360 degrees | Updates compass rotation and direction text |
| `SetCoordinates(Vector2)` | `latLong`: x=longitude, y=latitude | Updates coordinate display |

#### Inspector Properties

| Property | Description |
|----------|-------------|
| Compass Transform | Reference to the rotating compass RectTransform |
| Angle Text | TextMeshPro for heading display (e.g., "45°NE") |
| Coordinates Text | TextMeshPro for lat/long display |
| Directions Text | Array of 4 cardinal direction labels (N, E, S, W) |
| Start Angle | Offset angle for compass rotation |
| Smooth Time | Interpolation smoothness (default: 0.1s) |
| Enable Debug Slider | Toggle debug mode in editor |
| Debug Angle | Manual angle control (0-360) |
| Debug Coordinates | Manual coordinate input |

---

## Fonts

The package includes **NotoSansHebrew** fonts for Hebrew language support:

| Font Weight | Files |
|-------------|-------|
| ExtraLight | `NotoSansHebrew-ExtraLight.ttf`, `.asset` |
| Medium | `NotoSansHebrew-Medium.ttf`, `.asset` |
| SemiBold | `NotoSansHebrew-SemiBold.ttf`, `.asset` |

**Location:** `Fonts/`

These fonts are pre-configured as TextMesh Pro SDF assets for high-quality text rendering.

---

## Testing

### Testing in the Sample Scene

1. Open the sample scene: `Assets/Scenes/SampleScene.unity`
2. Add UI components from the package to test

### Testing YahalomButton

1. Create a Canvas in your scene (`GameObject > UI > Canvas`)
2. Drag the `YahalomButton.prefab` into the Canvas
3. Enter Play Mode
4. Interact with the button to verify:
   - Hover state (mouse over)
   - Click state (mouse down)
   - Normal state (mouse exit)
   - Disabled state (disable the button via Inspector)

**Verifying Color Transitions:**
- Observe text and icon color changes between states
- Adjust fade duration to test transition smoothness

### Testing YahalomInputField

1. Create a Canvas in your scene
2. Drag the `YahalomInputField.prefab` into the Canvas
3. Enter Play Mode
4. Interact with the input field to verify:
   - Focus state (click on input)
   - Text input and color changes
   - Clear button functionality (click X to clear text)
   - Disabled state (disable via Inspector)

**Testing Error Validation:**

```csharp
using UnityEngine;
using YahalomUIPackage.Runtime.YahalomInputField;

public class InputFieldTester : MonoBehaviour
{
    public YahalomInputField inputField;

    void Start()
    {
        // Set validation: error if less than 3 characters
        inputField.SetErrorPredicate(value => value.Length > 0 && value.Length < 3);
    }
}
```

### Testing Compass

1. Create a Canvas in your scene
2. Drag the `Compass.prefab` into the Canvas
3. Enable **Debug Mode** in the Compass Inspector:
   - Check "Enable Debug Slider"
   - Use the "Debug Angle" slider (0-360) to rotate the compass
   - Modify "Debug Coordinates" to test coordinate display
4. Enter Play Mode to see live updates

**Testing via Script:**

```csharp
using UnityEngine;
using YahalomUIPackage.Runtime.Compass;

public class CompassTester : MonoBehaviour
{
    public Compass compass;

    void Update()
    {
        // Rotate compass based on time
        float heading = (Time.time * 30f) % 360f;
        compass.SetHeading(heading);

        // Set test coordinates
        compass.SetCoordinates(new Vector2(35.2150f, 31.7825f));
    }
}
```

### Running Unit Tests

The project includes test assemblies for automated testing:

1. Open the Test Runner: `Window > General > Test Runner`
2. Run **Play Mode Tests** for runtime behavior testing
3. Run **Edit Mode Tests** for editor functionality testing

---

## API Reference

### YahalomButton

**Namespace:** `YahalomUIPackage.Runtime.YahalomButton`

```csharp
public class YahalomButton : Button
{
    // Serialized Fields (configurable in Inspector)
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private Image _icon;

    // Text color states
    [SerializeField] private Color _normalTextColor;
    [SerializeField] private Color _highlightedTextColor;
    [SerializeField] private Color _pressedTextColor;
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Color _disabledTextColor;

    // Icon color states
    [SerializeField] private Color _normalIconColor;
    [SerializeField] private Color _highlightedIconColor;
    [SerializeField] private Color _pressedIconColor;
    [SerializeField] private Color _selectedIconColor;
    [SerializeField] private Color _disabledIconColor;

    // Transition settings
    [SerializeField] private float _fadeDuration = 0.1f;
}
```

### YahalomInputField

**Namespace:** `YahalomUIPackage.Runtime.YahalomInputField`

```csharp
public class YahalomInputField : TMP_InputField
{
    // Public Methods
    public void SetErrorPredicate(Func<string, bool> predicate);

    // Serialized Fields (configurable in Inspector)
    [SerializeField] private Button _closeButton;

    // Text color states
    [SerializeField] private Color _normalTextColor;
    [SerializeField] private Color _highlightedTextColor;
    [SerializeField] private Color _pressedTextColor;
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Color _disabledTextColor;
    [SerializeField] private Color _errorTextColor;

    // Image color states
    [SerializeField] private Color _errorImageColor;
}
```

### Compass

**Namespace:** `YahalomUIPackage.Runtime.Compass`

```csharp
public class Compass : MonoBehaviour
{
    // Public Methods
    public void SetHeading(float heading);
    public void SetCoordinates(Vector2 latLong);

    // Inspector Properties
    [SerializeField] private RectTransform _compassTransform;
    [SerializeField] private TextMeshProUGUI _angleText;
    [SerializeField] private TextMeshProUGUI _coordinatesText;
    [SerializeField] private TextMeshProUGUI[] _directionsText;
    [SerializeField] private float _startAngle;
    [SerializeField] private float _smoothTime = 0.1f;

    // Debug Properties
    [SerializeField] private bool _enableDebugSlider;
    [SerializeField] private float _debugAngle;
    [SerializeField] private Vector2 _debugCoordinates;
}
```

---

## Sprite Assets

**Location:** `UI/PNG/`

### MainButton Sprites
- `primary_button_default.png` - Normal state
- `primary_button_hover.png` - Highlighted state
- `primary_button_click.png` - Pressed state
- `primary_button_disabled.png` - Disabled state

### Compass Sprites
- `Compass_BG@2x.png` - Compass background
- `Compass lines@2x.png` - Compass rose/direction lines

**Sprite Atlas:** `Atlas/MainUIAtlas.spriteatlasv2` - Optimized sprite collection

---

## Dependencies

- TextMesh Pro (included with Unity)
- RTLTMPro (for RTL text support)

---

## Changelog

See [CHANGELOG.md](../../CHANGELOG.md) for version history.

---

## Support

For issues or feature requests, please contact the MapColonies team or submit an issue to the repository.
