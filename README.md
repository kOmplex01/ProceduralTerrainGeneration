# ProceduralTerrainGeneration
In this project, I have Implemented procedural terrain generation using Compute Shader, which does all the calcualtions on the GPU, hence are very fast. Therefore, the terrain can be created or updated seamlessly in the runtime.

## How to Use
The gameobject must have three components :
>Mesh Filter (Dont add any mesh)

>Mesh Renderer

>MeshGeneration.cs (Add MeshGeneration.compute in the Placeholder)

## Light Affected Terrain
Terrain gets affected by light realtime because it is a mesh under the hood.
## Featurres in Terrain System

#### Octaves
https://user-images.githubusercontent.com/98254989/170531986-6986a95e-5ef5-46bf-8f24-d03137192eab.mp4


#### Persistance
https://user-images.githubusercontent.com/98254989/170532022-e9e1e160-effe-4eba-bffb-3dcb87f51d26.mp4


#### Lacunarity
https://user-images.githubusercontent.com/98254989/170532042-f8e29778-5194-44bd-a7c9-aa5d41bb04a0.mp4


#### Depth
https://user-images.githubusercontent.com/98254989/170532063-37ce7045-c2d6-4515-baba-6bccdb2e8178.mp4


#### Scale
https://user-images.githubusercontent.com/98254989/170532077-493c450c-3e2c-4ec3-b3a8-f842828eebbe.mp4


#### OffsetX
https://user-images.githubusercontent.com/98254989/170532094-704b575f-f1a3-43a1-b59a-ef4e4086136e.mp4


#### OffsetZ
https://user-images.githubusercontent.com/98254989/170532103-175a151c-eac5-48a8-a84c-346874da05ef.mp4
