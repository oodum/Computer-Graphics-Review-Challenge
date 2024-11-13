# Computer Graphics Review Challenge
## Adam Tam (100868600) & Sidharth Suresh (100938544)

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