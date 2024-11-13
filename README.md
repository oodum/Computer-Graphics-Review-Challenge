# Computer Graphics Review Challenge
## Adam Tam (100868600) & Sidharth Suresh (100938544)

# How to play
A or D to dodge, and space to block

The enemy will take a step forward right before it attacks. Make sure to dodge properly!

When you die, you will turn red.

We made use of two different shaders

- Combined shader with both **Toon Shader** with **Outline Shader**
- Negative-Color Shader

> Note: Both shaders are located at `/Assets/_Project/Shaders/`

# Now to the explanations!

## Negative Color Shader

### `Properties`

```csharp
Properties
{
    _MainTex ("Texture", 2D) = "white" {}
    _Tint ("Tint", Color) = (1, 0, 1, 1)
    _TintStrength ("Strength", Range(0, 1)) = 0.5
}
```

- `_MainTex_` is used for the albedo texture of the object.
- `_Tint` is the color that will be used to tint the object.
- `_TintStrength` defines the strength of the tint from 0-1.

### `SubShader`

```csharp
CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
float3 _PrevColor;
float3 _CurrColor;
float3 _Tint;
float _TintStrength;

struct Input
{
    float2 uv_MainTex;
};

void surf(Input IN, inout SurfaceOutput o)
{
    _PrevColor = tex2D(_MainTex, IN.uv_MainTex);
    _CurrColor.r = abs(1 - _PrevColor.r);
    _CurrColor.g = abs(1 - _PrevColor.g);
    _CurrColor.b = abs(1 - _PrevColor.b);
    o.Albedo = (_CurrColor + (_Tint*_TintStrength)).rgb;
}
ENDCG
```

#### The logic behind it...

We get the color of the object on its surface and try to invert it, there by causing a negative effect.

In pseudocode, it'd look like this:
```python
surface_color = (1, 1, 0, 1) # RGB values
inverted_color = abs(1 - surface_color)
```

With the tinting, it'd look like this:
```python
surface_color = (1, 1, 0) # RGB values
inverted_color = abs(1 - surface_color)

tint_color = (1, 0, 1) # RGB values
tint_strength = 0.5

tinted_inverted_color = inverted_color + (tint_color * tint_strength)
```

# Game Implementation (Adam)
In order to get the shader to be in the game, this is how we did it
When the player dodges, it sets the shader to the negative shader and then after a delay, it unsets the shader back to the normal shader.
```csharp

    async void DodgeLeft() {
        isDodging = true;
        transform.position = initialPos - transform.right;
        SetNegative();
        await Task.Delay((int)(dodgeTime * 1000));
        UnsetNegative();
        transform.position = initialPos;
        isDodging = false;
    }
    
    void SetNegative() {
        renderer.material = negativeShader;
    }

    void UnsetNegative() {
        renderer.material = normalShader;
    }
```

When the player attacks, it sets the main color's tint to blue:
```csharp
    async void Attack() {
        renderer.material.SetColor(COLOR_TINT, Color.blue);
        await Task.Delay(100);
        renderer.material.SetColor(COLOR_TINT, Color.white);
    }
```

When the player takes damage, it's going to check if the health is <= 0. If it is, it'll set the color to red, and the player cannot move anymore:
```csharp
    void OnAttack() {
        if (isDodging) return;
        TakeDamage();
    }
    void TakeDamage() {
        if (--healthComponent.Health <= 0) {
            GameManager.Instance.EndGame();
            renderer.material = normalShader;
            renderer.material.SetColor(COLOR_TINT, Color.red);
        }
    }
```
