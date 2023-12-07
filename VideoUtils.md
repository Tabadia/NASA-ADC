-------
THIS IS NOT A VIDEO SCRIPT. 
THIS IS AN OUTLINE.
-------

# App Components

## Pathfinding

TBD 

## Player Data - Azimuth, Distance, Coordinates, Elevation Angle, Elevation Angle To Earth

### Azimuth

Using two coordinates (player position and Earth position), we used the formula 
$$\arctan((x1 - x2) / (y1 - y2))$$ 
to figure out azimuth.

### Distance 

To figure out coordinates, we plugged in {x1, y1, z1} and {x2, y2, z2} into
$$\√[(x2 – x1)^2 + (y2 – y1)^2 + (z2 – z1)^2]$$

### Coordinates 
We used our game engine's built-in coordinate system and added our own transforms onto it.
```p1.transform.position;```

### Elevation angle

Using two raycasts,
```Physics.Raycast()```
We were able to use the distance that it traveled and averaged those out to get elevation angle.

### Elevation Angle To Earth

Same Thing.

## Color By:

### Height

Using a builtin function ```Color.Lerp()```, we normalized the value of the y position of a given vertice and got a value in a gradient based on that.

### Angle 

For a 3x3 chunk of vertices(to reduce a single vertice massively skewing the color), we took the difference between the y positions and combined it with the distance.

```
distance = Vector3.Distance(coords[min], coords[max]);
angle = Math.Atan((coords[max].y - coords[min].y) / distance) * 180/Math.PI
```

### Azimuth

For a 3x3 chunk of vertices, we calculated the azimuth for the average value of that chunk.
```    
float azimuth = Mathf.Atan((xVal - EARTH_LOCATION.x) / (yVal - EARTH_LOCATION.y)) * 180/Math.PI;
```

## 2 Player System

Realistically, a mission has more than one person, so we wanted to model that too.

### Movement

We listened for keyboard events, and applied velocity changes when they were fired.

### First Person Camera

We locked the user mouse, then applied transforms to the camera rotation. Additionally, values were clamped to increase realism.

### Minimap 

We placed a "camera" of sorts above the entire simulation, and used the values of all the important areas to construct it.

### Alternate Player View 

We set the scripts up so that one player always had access to what the other player was doing, meaning that we could display the other person's camera.

## Communication Links

Using prefabs, we set up communication link checkpoints so that they would be unobstucted, along the path, and near each other.

## Landing and areas of interest

TBD hobbes


# Challenges

## Incompatibility

As the project grew bigger, there were some commits from members of our team the could not be easily integrated into our project. For example, the package ```Microsoft.VisualBasic.FileIO``` was not accessible from our development environment, causing us to need to use a replacement of the ```TextFieldParser``` class.

## Performance

### Terrain Coloring - Azimuth

This line of code
```    
float azimuth = Mathf.Atan((xVal - EARTH_LOCATION.x) / (yVal - EARTH_LOCATION.y)) * 180/Math.PI;
```

was extremely performance intensive, especially because it was being called 10 million times. To solve this problem, we tried to call the function as few times as possible. We first split it into mini-chunks

```
for(var y = 0; y < azimuthRadius;  y++) {
    for(var z = 0; z < azimuthRadius; z++) {
        int newIdx = idx + y + z*100;

        Vector3 vertice = vertices[newIdx];
        xVal = Math.Max(vertice.x, xVal);
        yVal = Math.Max(vertice.y, yVal);
    }
}
```

And then ran the calculation. This led to that line of code being run 100x less, leading to an 80x performance increase.

### Mesh Generation

Since the raw mesh was too large to be shared, we had to generate it dynamically at runtime. To solve this, we ran the mesh code on a different thread, letting us compute other things while the mesh generated.

## Bugs 


### Player

#### Trail

We added a trail to the player, and this let the player jump on it, which was not an intended feature. In the end, we assigned a new layer to the trail, and added code to the player to detect whether it was touching that layer and ignore it.

### Terrain

#### Generation

The CSV file was too big to parse into one big piece, so we had to split it into many smaller chunks to process. Additionally, we would sometimes mess up and cause the terrain to be extremely deformed. There were also many cases where it would crash our computers and we would have to restart it.

#### Coloring

##### Angle

Writing code to color by angle was difficult, because there were small pockets where the slope was extremely high in relatively flat areas, and vice versa. To solve this, we sampled many vertices at once and averaged it out to get a better picture.

##### Azimuth

Azimuth does not vary much on the scale of Peak Near Shackleton, so when we tried to normalize between -90 and 90, everything was colored the same. To solve this, we massively amped up the contrast, normalizing from a much smaller range.


# Student Aquired Skills

## Edwin

Started out knowing nothing about Unity and C#, Learned:
> C#, Unity Meshes, Trigonometry applications in space engineering, Raycasting, Game Deployment, Chunk Rendering, Anchors, GameObject Hierarchy

## Thalen

> etc

## Alex

> etc

## Winston

> etc

## Sidd

> etc

## Tanush

> etc






