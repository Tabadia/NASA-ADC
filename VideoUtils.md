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

