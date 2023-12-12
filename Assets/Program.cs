using Microsoft.VisualBasic.FileIO;
using System;
using UnityEngine;
using System.Collections;
using System.Formats.Asn1;
using System.Globalization;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

class GridCoordinates {
    public int xCoord;
    public int yCoord;

    public GridCoordinates(int xCoord, int yCoord) {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
    }
}
class CartesianCoordinates {
    public double xCoord;
    public double yCoord;
    public double zCoord;

    public CartesianCoordinates(double xCoord, double yCoord, double zCoord) {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
        this.zCoord = zCoord;
    }

    public Vector3 vector3() {
        //miniscule precision loss.
        return new Vector3((float)xCoord, (float)yCoord, (float)zCoord);
    }
}

class PolarCoordinates {
    public double longitude;
    public double latitude;
    public double distanceFromMoonCoreInKilometers;

    public PolarCoordinates(double longitude, double latitude) {
        this.longitude = longitude;
        this.latitude = latitude;
        this.distanceFromMoonCoreInKilometers = 0;
    }

    public PolarCoordinates(double longitude, double latitude, double distanceFromMoonCoreInKilometers) {
        this.longitude = longitude;
        this.latitude = latitude;
        this.distanceFromMoonCoreInKilometers = distanceFromMoonCoreInKilometers;
    }
}

//Plane equation formatting is ax + by + cz = d
class Plane {
    public double xCoefficient;
    public double yCoefficient;
    public double zCoefficient;
    public double constant;

    public Plane(double xCoefficient, double yCoefficient, double zCoefficient, double constant) {
        this.xCoefficient = xCoefficient;
        this.yCoefficient = yCoefficient;
        this.zCoefficient = zCoefficient;
        this.constant = constant;
    }
}

class Line {
    public CartesianCoordinates positionVector;
    public CartesianCoordinates directionVector;

    public Line(CartesianCoordinates positionVector, CartesianCoordinates directionVector) {
        this.positionVector = positionVector;
        this.directionVector = directionVector;
    }

    public CartesianCoordinates GetPointOnLine(double parameter) {
        double x = positionVector.xCoord + parameter * directionVector.xCoord;
        double y = positionVector.yCoord + parameter * directionVector.yCoord;
        double z = positionVector.zCoord + parameter * directionVector.zCoord;

        CartesianCoordinates point = new CartesianCoordinates(x, y, z);

        return point;
    }
}

class GridCartesianPair {
    public CartesianCoordinates cartesianPosition;
    public GridCoordinates gridPosition;

    public GridCartesianPair(CartesianCoordinates cartesianPosition, GridCoordinates gridPosition) {
        this.cartesianPosition = cartesianPosition;
        this.gridPosition = gridPosition;
    }
}

class MoonCalculator {
    public CartesianCoordinates EarthCoordinates = new CartesianCoordinates(361000, 0, -42100);

    /*public static void EarthTest() {
        Debug.Log(EarthCoordinates.xCoord);
    }*/

    public static double DegreesToRadians(double angleInDegrees) {
        double angleInRadians = angleInDegrees * (Math.PI / 180);

        return angleInRadians;
    }
    public static double RadiansToDegrees(double angleInRadians) {
        double angleInDegrees = angleInRadians * (180 / Math.PI);

        return angleInDegrees;
    }

    public static CartesianCoordinates GetSphericalToCartesianCoordinates(PolarCoordinates polarCoordinates, double height) {
        //Both values in Kilometers
        const double lunarRadius = 1737.4;
        double radius = 0;

        if (polarCoordinates.distanceFromMoonCoreInKilometers == 0) {
            radius = lunarRadius + 0.001 * height;
        } else {
            radius = polarCoordinates.distanceFromMoonCoreInKilometers;
        }

        //Debug.Log("longitude input: " + polarCoordinates.longitude);
        //Debug.Log("latitude input: " + polarCoordinates.latitude);
        //Debug.Log(height);
        //Debug.Log();

        double x = radius * (Cos(polarCoordinates.latitude) * Cos(polarCoordinates.longitude));
        double y = radius * (Cos(polarCoordinates.latitude) * Sin(polarCoordinates.longitude));
        double z = radius * (Sin(polarCoordinates.latitude));

        CartesianCoordinates CartesianCoordinates = new CartesianCoordinates(x, y, z);

        return CartesianCoordinates;
    }

    public static PolarCoordinates GetCartesianToSphericalCoordinates(CartesianCoordinates cartesianCoordinates) {
        double x = cartesianCoordinates.xCoord;
        double y = cartesianCoordinates.yCoord;
        double z = cartesianCoordinates.zCoord;
        double distanceFromMoonCoreInKilometers = Math.Sqrt(x * x + y * y + z * z);

        double longitude = Asin((y) / (Math.Sqrt(x * x + y * y)));
        double latitude = Asin((z) / (distanceFromMoonCoreInKilometers));

        return new PolarCoordinates(longitude, latitude, distanceFromMoonCoreInKilometers);
    }

    //This method might not be needed????
    public static double GetDistanceFromMoonCoreInKilometers(CartesianCoordinates cartesianCoordinates) {
        double x = cartesianCoordinates.xCoord;
        double y = cartesianCoordinates.yCoord;
        double z = cartesianCoordinates.zCoord;
        double distance = Math.Sqrt(x * x + y * y + z * z);

        return distance;
    }

    public static double Sin(double angleInDegrees) {
        double angleInRadians = DegreesToRadians(angleInDegrees);
        double sinOutput = Math.Sin(angleInRadians);

        return sinOutput;
    }

    public static double Cos(double angleInDegrees) {
        double angleInRadians = DegreesToRadians(angleInDegrees);
        double cosOutput = Math.Cos(angleInRadians);

        return cosOutput;
    }

    public static double Asin(double ratio) {
        double aSinOutput = RadiansToDegrees(Math.Asin(ratio));

        return aSinOutput;
    }

    public static double GetAzimuthAnglePolarInputs(PolarCoordinates polarCoordinatesA, PolarCoordinates polarCoordinatesB) {
        double longitudeA = polarCoordinatesA.longitude;
        double latitudeA = polarCoordinatesA.latitude;
        double longitudeB = polarCoordinatesB.longitude;
        double latitudeB = polarCoordinatesB.latitude;

        double y = Sin(longitudeB - longitudeA) * Cos(latitudeB);
        double x = (Cos(latitudeA) * Sin(latitudeB)) - (Sin(latitudeA) * Cos(latitudeB) * Cos(longitudeB - longitudeA));

        Debug.Log("y = " + y);
        Debug.Log("x = " + x);

        double azimuthAngleInRadians = Math.Atan2(y, x);
        double azimuthAngleInDegrees = RadiansToDegrees(azimuthAngleInRadians);

        return azimuthAngleInDegrees;
    }

    public static double GetAzimuthAngleOneCartesianInput(PolarCoordinates polarCoordinatesA, CartesianCoordinates cartesianCoordinates) {
        PolarCoordinates polarCoordinatesB = GetCartesianToSphericalCoordinates(cartesianCoordinates);

        return GetAzimuthAnglePolarInputs(polarCoordinatesA, polarCoordinatesB);
    }

    public static double GetElevationAngle(PolarCoordinates polarCoordRef, double heightRef, CartesianCoordinates targetLocation) {
        CartesianCoordinates referenceLocation = GetSphericalToCartesianCoordinates(polarCoordRef, heightRef);

        double xDif = targetLocation.xCoord - referenceLocation.xCoord;
        double yDif = targetLocation.yCoord - referenceLocation.yCoord;
        double zDif = targetLocation.zCoord - referenceLocation.zCoord;

        //CartesianCoordinates distanceVector = new CartesianCoordinates(xDif, yDif, zDif);

        double refToTargetDistance = Math.Sqrt(xDif * xDif + yDif * yDif + zDif * zDif);
        double rz = (xDif * Cos(polarCoordRef.latitude) * Cos(polarCoordRef.longitude)) + (yDif * Cos(polarCoordRef.latitude) * Sin(polarCoordRef.longitude) + (zDif * Sin(polarCoordRef.latitude)));

        double elevationAngle = Asin(rz / refToTargetDistance);

        return elevationAngle;
    }

    public static double GetDistanceBetweenCartesianPoints(CartesianCoordinates point1, CartesianCoordinates point2) {
        double xDistance = Math.Abs(point2.xCoord - point1.xCoord);
        double yDistance = Math.Abs(point2.yCoord - point1.yCoord);
        double zDistance = Math.Abs(point2.zCoord - point1.zCoord);

        double distance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance + zDistance * zDistance);

        return distance;
    }

    //Successfully Unit Tested
    public static CartesianCoordinates FindIntersectionOfLines(Line line1, Line line2) {
        CartesianCoordinates PosVec1 = line1.positionVector;
        CartesianCoordinates DirVec1 = line1.directionVector;
        CartesianCoordinates PosVec2 = line2.positionVector;
        CartesianCoordinates DirVec2 = line2.directionVector;
        double a = PosVec1.xCoord;
        double b = PosVec1.yCoord;
        double c = PosVec1.zCoord;
        double d = PosVec2.xCoord;
        double e = PosVec2.yCoord;
        double f = PosVec2.zCoord;
        double x = DirVec1.xCoord;
        double y = DirVec1.yCoord;
        double z = DirVec2.xCoord;
        double u = DirVec2.xCoord;
        double v = DirVec2.yCoord;
        double w = DirVec2.zCoord;

        double parameter = (e * x - b * x - d * y + a * y) / (u * y - v * x);

        CartesianCoordinates intersectionPoint = line2.GetPointOnLine(parameter);

        return intersectionPoint;
    }

    public static void CreateEarthVisibilityCSV() {
        StreamReader reader = new StreamReader("C:\\Users\\alex\\Documents\\HOWWRITE.txt");
        StreamWriter writer = new StreamWriter("C:\\Users\\alex\\Documents\\todolist.txt");

        Random random = new Random();

        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                int jim = random.Next(0, 2);

                writer.Write(jim);

                if (j < 9) {
                    writer.Write(",");
                }
            }

            writer.WriteLine();
        }

        writer.Close();
    }
}






class MoonMapper {
    public double[,] heightMap;
    public double[,] slopeMap;
    public double[,] latitudeMap;
    public double[,] longitudeMap;
    public CartesianCoordinates EarthPosition;
    public Plane landingSite;

    public MoonMapper(string heightFilePath, string slopeFilePath, string latitudeFilePath, string longitudeFilePath) {
        this.heightMap = CSVToArray(heightFilePath);
        this.slopeMap = CSVToArray(slopeFilePath);
        this.latitudeMap = CSVToArray(latitudeFilePath);
        this.longitudeMap = CSVToArray(longitudeFilePath);
        this.EarthPosition = new CartesianCoordinates(361000, 0, -42100);
        this.landingSite = GetPlaneFromData();
    }

    public CartesianCoordinates ConvertGridToCartesian(GridCoordinates point, double height) {
        PolarCoordinates pointPolar = new PolarCoordinates(this.longitudeMap[point.xCoord, point.yCoord], this.latitudeMap[point.xCoord, point.yCoord]);
        CartesianCoordinates pointCartesian = MoonCalculator.GetSphericalToCartesianCoordinates(pointPolar, height);

        return pointCartesian;
    }

    //Successfully Unit Tested
    public Plane GetPlaneFromData() {
        CartesianCoordinates[] threeArbitraryCartesianPoints = new CartesianCoordinates[3];
        GridCoordinates[] threeArbitraryGridPoints = new GridCoordinates[3];
        threeArbitraryGridPoints[0] = new GridCoordinates(1, 1);
        threeArbitraryGridPoints[1] = new GridCoordinates(10, 4);
        threeArbitraryGridPoints[2] = new GridCoordinates(6, 11);

        for (int i = 0; i < 3; i++) {
            threeArbitraryCartesianPoints[i] = ConvertGridToCartesian(threeArbitraryGridPoints[i], 0);
        }

        double xDif = threeArbitraryCartesianPoints[0].xCoord - threeArbitraryCartesianPoints[1].xCoord;
        double yDif = threeArbitraryCartesianPoints[0].yCoord - threeArbitraryCartesianPoints[1].yCoord;
        double zDif = threeArbitraryCartesianPoints[0].zCoord - threeArbitraryCartesianPoints[1].zCoord;
        CartesianCoordinates vector1 = new CartesianCoordinates(xDif, yDif, zDif);

        xDif = threeArbitraryCartesianPoints[0].xCoord - threeArbitraryCartesianPoints[2].xCoord;
        yDif = threeArbitraryCartesianPoints[0].yCoord - threeArbitraryCartesianPoints[2].yCoord;
        zDif = threeArbitraryCartesianPoints[0].zCoord - threeArbitraryCartesianPoints[2].zCoord;
        CartesianCoordinates vector2 = new CartesianCoordinates(xDif, yDif, zDif);

        double planeXCoefficient = (vector1.yCoord * vector2.zCoord) - (vector1.zCoord * vector2.yCoord);
        double planeYCoefficient = (vector1.zCoord * vector2.xCoord) - (vector1.xCoord * vector2.zCoord);
        double planeZCoefficient = (vector1.xCoord * vector2.yCoord) - (vector1.yCoord * vector2.xCoord);

        double testX = threeArbitraryCartesianPoints[0].xCoord;
        double testY = threeArbitraryCartesianPoints[0].yCoord;
        double testZ = threeArbitraryCartesianPoints[0].zCoord;

        double d = planeXCoefficient * testX + planeYCoefficient * testY + planeZCoefficient * testZ;

        Plane plane = new Plane(planeXCoefficient, planeYCoefficient, planeZCoefficient, d);

        /*Debug.Log("Plane x: " + plane.xCoefficient);
        Debug.Log("Plane y: " + plane.yCoefficient);
        Debug.Log("Plane z: " + plane.zCoefficient);
        Debug.Log("Plane constant: " + plane.constant);
        */
        /*
         Plane x: 0.000012980729403992085
         Plane y: -0.000023765633016050406
         Plane z: 0.0018746087170362822
         Plane constant: -3.257284982661449
         */

        return plane;
    }
    public bool IsTileTooHigh(GridCartesianPair tileToCheck, Plane landingSite, Line initPosToEarth) {
        //PolarCoordinates polarTileToCheck = new PolarCoordinates(this.longitudeMap[tileToCheck.gridPosition.xCoord, tileToCheck.gridPosition.yCoord], this.latitudeMap[tileToCheck.gridPosition.xCoord, tileToCheck.gridPosition.yCoord]);
        //CartesianCoordinates cartesianPointToCheck = MoonCalculator.GetSphericalToCartesianCoordinates(polarTileToCheck, 0);
        CartesianCoordinates planeNormalVector = new CartesianCoordinates(landingSite.xCoefficient, landingSite.yCoefficient, landingSite.zCoefficient);
        CartesianCoordinates cartesianPointToCheck = tileToCheck.cartesianPosition;
        Line normalLine = new Line(cartesianPointToCheck, planeNormalVector);
        CartesianCoordinates intersectionPoint = MoonCalculator.FindIntersectionOfLines(initPosToEarth, normalLine);
        double distanceToIntersection = MoonCalculator.GetDistanceBetweenCartesianPoints(cartesianPointToCheck, intersectionPoint);
        double height = this.heightMap[tileToCheck.gridPosition.xCoord, tileToCheck.gridPosition.yCoord] / 1000;

        if (distanceToIntersection > height + 0.003) {
            return false;

        } else {
            return true;

        }
    }
    public List<GridCartesianPair> ConvertProjectedLineToTiles(Line projectedLine, GridCoordinates startingPosition) {
        List<GridCartesianPair> tilesList = new List<GridCartesianPair>();
        int indexBound = 0;
        double parameter = 0;
        bool hasTileBeenFound = true;

        while (hasTileBeenFound) {
            hasTileBeenFound = false;
            CartesianCoordinates currentLocationCheck = projectedLine.GetPointOnLine(parameter);

            /*Debug.Log("---------------------------------------------------");
            Debug.Log("Line update X: " + currentLocationCheck.xCoord);
            Debug.Log("Line update Y: " + currentLocationCheck.yCoord);
            Debug.Log("Line update Z: " + currentLocationCheck.zCoord);
            Debug.Log();
            Debug.Log("Direction vector X: " + projectedLine.directionVector.xCoord);
            Debug.Log("Direction vector Y: " + projectedLine.directionVector.yCoord);
            Debug.Log("Direction vector Z: " + projectedLine.directionVector.zCoord);
            Debug.Log();
            */

            if (tilesList.Count < 1) {
                CartesianCoordinates tileCartesian = this.ConvertGridToCartesian(startingPosition, 0);

                /*Debug.Log("Starting X point: " + tileCartesian.xCoord);
                Debug.Log("Starting Y point: " + tileCartesian.yCoord);
                Debug.Log("Starting Z point: " + tileCartesian.zCoord);
                Debug.Log();
                Debug.Log("Line X point: " + currentLocationCheck.xCoord);
                Debug.Log("Line Y point: " + currentLocationCheck.yCoord);
                Debug.Log("Line Z point: " + currentLocationCheck.zCoord);
                Debug.Log();
                */

                if (MoonCalculator.GetDistanceBetweenCartesianPoints(currentLocationCheck, tileCartesian) < 0.003535533906) {
                    hasTileBeenFound = true;
                    GridCartesianPair tilePrecise = new GridCartesianPair(currentLocationCheck, startingPosition);
                    tilesList.Add(tilePrecise);
                }

                if (!hasTileBeenFound) {
                    //Debug.Log("No more");
                    //Debug.Log("Amount of tiles found: " + tilesList.Count);
                    return tilesList;
                }

            } else {
                int previousXCoord = tilesList[tilesList.Count - 1].gridPosition.xCoord;
                int previousYCoord = tilesList[tilesList.Count - 1].gridPosition.yCoord;
                CartesianCoordinates tileCartesian = this.ConvertGridToCartesian(tilesList[tilesList.Count - 1].gridPosition, 0);

                if (MoonCalculator.GetDistanceBetweenCartesianPoints(currentLocationCheck, tileCartesian) > 0.003535533906) {
                    GridCoordinates[] allEightDirections = this.GenerateAllEightDirections();
                    //arbitrarily large number (10 km)
                    double shortestDistanceFound = 10;

                    for (int i = 0; i < 8; i++) {
                        GridCoordinates directionToTest = allEightDirections[i];
                        int newXCoord = previousXCoord + directionToTest.xCoord;
                        int newYCoord = previousYCoord + directionToTest.yCoord;

                        if (0 <= newXCoord && newXCoord < this.heightMap.GetLength(0) && 0 <= newYCoord && newYCoord < this.heightMap.GetLength(0)) {
                            GridCoordinates tileToTest = new GridCoordinates(newXCoord, newYCoord);

                            PolarCoordinates tilePolar = new PolarCoordinates(this.longitudeMap[newXCoord, newYCoord], this.latitudeMap[newXCoord, newYCoord]);
                            tileCartesian = MoonCalculator.GetSphericalToCartesianCoordinates(tilePolar, 0);

                            /*Debug.Log("New point X: " + tileCartesian.xCoord);
                            Debug.Log("New point Y: " + tileCartesian.yCoord);
                            Debug.Log("New point Z: " + tileCartesian.zCoord);
                            Debug.Log();
                            Debug.Log("Distance between tested point and line point: " + MoonCalculator.GetDistanceBetweenCartesianPoints(currentLocationCheck, tileCartesian));
                            */

                            double testToLinePointDistance = MoonCalculator.GetDistanceBetweenCartesianPoints(currentLocationCheck, tileCartesian);

                            /*if (testToLinePointDistance < shortestDistanceFound) {
                                shortestDistanceFound = testToLinePointDistance;
                            }*/

                            if (testToLinePointDistance < 0.003535533906) {
                                int iterationLimit = 3;

                                if (tilesList.Count < iterationLimit) {
                                    iterationLimit = tilesList.Count;
                                }

                                for (int j = 1; j <= iterationLimit; j++) {
                                    double coveredX = tilesList[tilesList.Count - j].gridPosition.xCoord;
                                    double coveredY = tilesList[tilesList.Count - j].gridPosition.yCoord;

                                    if (newXCoord == coveredX && newYCoord == coveredY) {
                                        hasTileBeenFound = true;
                                        break;
                                    }
                                }

                                if (!hasTileBeenFound) {
                                    /*Debug.Log();
                                    Debug.Log("Deez");
                                    Debug.Log();*/

                                    GridCoordinates tile = new GridCoordinates(newXCoord, newYCoord);
                                    indexBound = Math.Max(newXCoord, newYCoord);
                                    GridCartesianPair tilePrecise = new GridCartesianPair(currentLocationCheck, tile);
                                    tilesList.Add(tilePrecise);
                                    hasTileBeenFound = true;

                                    /*Debug.Log("Most Recent Tile X: " + tilePrecise.gridPosition.xCoord);
                                    Debug.Log("Most Recent Tile Y: " + tilePrecise.gridPosition.yCoord);
                                    Debug.Log("Amount of tiles: " + tilesList.Count);
                                    Debug.Log();
                                    */

                                    break;
                                }
                            }
                        }
                    }
                } else {
                    hasTileBeenFound = true;
                }
            }

            //Debug.Log("Has a tile been found? " + hasTileBeenFound);
            //Debug.Log();

            parameter += 0.00003;
        }

        return tilesList;
    }

    public Line GetPlaneProjection(Plane LandingSite, Line line) {
        double arbitraryParameter = 0.0001;

        CartesianCoordinates normalVector = new CartesianCoordinates(LandingSite.xCoefficient, LandingSite.yCoefficient, LandingSite.zCoefficient);
        CartesianCoordinates point = line.GetPointOnLine(arbitraryParameter);

        double d = LandingSite.constant;
        double x = LandingSite.xCoefficient;
        double y = LandingSite.yCoefficient;
        double z = LandingSite.zCoefficient;
        double a = point.xCoord;
        double b = point.yCoord;
        double c = point.zCoord;

        double scalingFactor = (d - (a * x) - (b * y) - (c * z)) / (x * x + y * y + z * z);

        Line normalLine = new Line(point, normalVector);
        CartesianCoordinates foundPointOnPlane = normalLine.GetPointOnLine(scalingFactor);

        double xDistance = foundPointOnPlane.xCoord - line.positionVector.xCoord;
        double yDistance = foundPointOnPlane.yCoord - line.positionVector.yCoord;
        double zDistance = foundPointOnPlane.zCoord - line.positionVector.zCoord;

        /*Debug.Log("x: " + foundPointOnPlane.xCoord);
        Debug.Log("y: " + foundPointOnPlane.yCoord);
        Debug.Log("z: " + foundPointOnPlane.zCoord);
        Debug.Log();*/

        CartesianCoordinates planeProjectedVector = new CartesianCoordinates(xDistance, yDistance, zDistance);
        Line projectedLine = new Line(line.positionVector, planeProjectedVector);

        CartesianCoordinates testPoint = projectedLine.GetPointOnLine(0.000006);

        /*Debug.Log("test x: " + line.positionVector.xCoord);
        Debug.Log("test y: " + line.positionVector.yCoord);
        Debug.Log("test z: " + line.positionVector.zCoord);
        Debug.Log();*/

        double please = this.landingSite.xCoefficient * testPoint.xCoord + this.landingSite.yCoefficient * testPoint.yCoord + this.landingSite.zCoefficient * testPoint.zCoord;

        //Debug.Log("Point Line Plane Test = " + please);
        //Debug.Log();

        /*
         Projected Vector x: -3612741.526781921
         Projected Vector y: 5019.266472402691
         Projected Vector z: 25079.499025959973
         */

        return projectedLine;
    }

    public bool IsEarthVisible(GridCoordinates currentPosition) {
        double currentLongitude = this.longitudeMap[currentPosition.xCoord, currentPosition.yCoord];
        double currentLatitude = this.latitudeMap[currentPosition.xCoord, currentPosition.yCoord];

        PolarCoordinates currentPolarPosition = new PolarCoordinates(currentLongitude, currentLatitude);
        CartesianCoordinates currentCartesianPositionReal = MoonCalculator.GetSphericalToCartesianCoordinates(currentPolarPosition, heightMap[currentPosition.xCoord, currentPosition.yCoord]);
        CartesianCoordinates currentCartesianPositionFlattened = MoonCalculator.GetSphericalToCartesianCoordinates(currentPolarPosition, 0);

        double xDistance = this.EarthPosition.xCoord - currentCartesianPositionReal.xCoord;
        double yDistance = this.EarthPosition.yCoord - currentCartesianPositionReal.yCoord;
        double zDistance = this.EarthPosition.zCoord - currentCartesianPositionReal.zCoord;

        CartesianCoordinates DirectionVector = new CartesianCoordinates(xDistance, yDistance, zDistance);
        CartesianCoordinates[,] cartesianTileMap = new CartesianCoordinates[this.heightMap.GetLength(0), this.heightMap.GetLength(1)];

        Line positionToEarthLine = new Line(currentCartesianPositionReal, DirectionVector);
        Line pseudoPositionToEarthLine = new Line(currentCartesianPositionFlattened, DirectionVector);
        Line projectedLine = GetPlaneProjection(this.landingSite, pseudoPositionToEarthLine);

        CartesianCoordinates testPoint = projectedLine.GetPointOnLine(0.000006);

        double d = this.landingSite.xCoefficient * testPoint.xCoord + this.landingSite.yCoefficient * testPoint.yCoord + this.landingSite.zCoefficient * testPoint.zCoord;

        //Debug.Log("Point Line Plane Test = " + d);

        List<GridCartesianPair> tilesTotest = ConvertProjectedLineToTiles(projectedLine, currentPosition);

        for (int i = 0; i < tilesTotest.Count; i++) {
            //Debug.Log("Tile #" + i + ": " + tilesTotest[i].gridPosition.xCoord + ", " + tilesTotest[i].gridPosition.yCoord);
        }

        for (int i = 0; i < tilesTotest.Count; i++) {
            if (IsTileTooHigh(tilesTotest[i], this.landingSite, positionToEarthLine)) {
                return false;
            }
        }

        return true;

        /*
        for (int i = 0; i < heightMap.GetLength(0); i++) {
            for (int j = 0; j < heightMap.GetLength(1); j++) {
                PolarCoordinates polarTilePosition = new PolarCoordinates(longitudeMap[i, j], latitudeMap[i, j]);

                cartesianTileMap[i, j] = MoonCalculator.GetSphericalToCartesianCoordinates(polarTilePosition, heightMap[i, j]);

                if (i < 5 && j < 5) {
                    Debug.Log();
                    Debug.Log("Position: " + i + ", " + j);
                    Debug.Log("xCoord: " + cartesianTileMap[i, j].xCoord);
                    Debug.Log("yCoord: " + cartesianTileMap[i, j].yCoord);
                    Debug.Log("zCoord: " + cartesianTileMap[i, j].zCoord);
                }
            }
        }*/
    }

    //If coordinates not found, returns (0, 0) by default
    public GridCoordinates GetPolarToGridConversion(PolarCoordinates polarCoords) {
        GridCoordinates gridLocation = new GridCoordinates(0, 0);

        List<GridCoordinates> candidateLocations = new List<GridCoordinates>();

        for (int i = 0; i < this.longitudeMap.GetLength(0); i++) {
            for (int j = 0; j < this.longitudeMap.GetLength(1); j++) {
                if (this.longitudeMap[i, j] == polarCoords.longitude) {
                    //Debug.Log(i + ", " + j);

                    candidateLocations.Add(new GridCoordinates(i, j));
                }
            }
        }

        if (candidateLocations.Count > 1) {
            for (int i = 0; i < candidateLocations.Count; i++) {
                GridCoordinates testLocation = (GridCoordinates)candidateLocations[i];

                if (polarCoords.latitude == this.latitudeMap[testLocation.xCoord, testLocation.yCoord]) {
                    gridLocation = testLocation;
                    break;
                }
            }

        } else if (candidateLocations.Count == 1) {
            gridLocation = (GridCoordinates)candidateLocations[0];
        }

        /*Debug.Log();
        Debug.Log("Real coordinate:");
        Debug.Log(gridLocation.xCoord + ", " + gridLocation.yCoord);
        */

        return gridLocation;
    }

    public int CountElementsInLine(string source, char toFind) {
        int count = 1;

        foreach (var ch in source) {
            if (ch == toFind)
                count++;
        }

        return count;
    }

    static T[,] To2D<T>(T[][] source) {
        // T is serialization for type. 
        try {
            int FirstDim = source.Length;
            int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

            var result = new T[FirstDim, SecondDim];
            for (int i = 0; i < FirstDim; ++i)
                for (int j = 0; j < SecondDim; ++j)
                    result[i, j] = source[i][j];

            return result;
        } catch (InvalidOperationException) {
            throw new InvalidOperationException("The given jagged array is not rectangular.");
        }
    }
    public static double[,] CSVToArray(string filePath) {

        //I'm a piece of shit.
        double[][] fileData = File.ReadLines(filePath)
            .Select(x => x.Split(','))
            .Select(line =>
                 line
                    .Select(x => double.Parse(x))
                    .ToArray())
            .ToArray();

        return To2D<double>(fileData);


    }
    public static double[,] CSVToArrayOld(string filePath) {
        var path = @filePath;

        using (TextFieldParser csvParser = new TextFieldParser(path)) {
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = true;

            string[] currentLine = csvParser.ReadFields();

            int lineCount = File.ReadLines(path).Count();
            int lineLength = currentLine.Length;

            //Debug.Log("Row length of CSV: " + lineLength);
            //Debug.Log(csvParser.LineNumber);

            double[,] map = new double[lineLength, lineCount];

            for (int i = 0; i < currentLine.Length; i++) {
                map[i, 0] = Convert.ToDouble(currentLine[i]);
            }

            int lineNumber = 1;

            while (!csvParser.EndOfData) {
                // Read current line fields, pointer moves to the next line.
                currentLine = csvParser.ReadFields();

                for (int i = 0; i < currentLine.Length; i++) {
                    map[i, lineNumber] = Convert.ToDouble(currentLine[i]);
                }

                lineNumber++;
            }

            return map;
        }
    }

    public double GetDistanceBetweenPoints(GridCoordinates point1, GridCoordinates point2) {
        int xDistance = Math.Abs(point2.xCoord - point1.xCoord);
        int yDistance = Math.Abs(point2.yCoord - point1.yCoord);

        double distance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);

        return distance;
    }

    public GridCoordinates[] GenerateAllEightDirections() {
        GridCoordinates[] EightDirectionsList = new GridCoordinates[8];
        int index = 0;

        for (int yCoord = 1; yCoord > -2; yCoord--) {
            for (int xCoord = -1; xCoord < 2; xCoord++) {
                if (yCoord != 0 || xCoord != 0) {
                    EightDirectionsList[index] = new GridCoordinates(xCoord * 5, yCoord * 5);
                    index++;
                }
            }
        }

        return EightDirectionsList;
    }

    public int GetBiggerNumber(int num1, int num2) {
        if (num1 >= num2) {
            return num1;
        } else {
            return num2;
        }
    }

    public int GetMinimumPathLength(GridCoordinates startPosGrid, GridCoordinates targetPosGrid) {
        int xDif = Math.Abs(targetPosGrid.xCoord - startPosGrid.xCoord);
        int yDif = Math.Abs(targetPosGrid.yCoord - startPosGrid.yCoord);

        int distance = GetBiggerNumber(xDif, yDif);

        return distance;
    }

    public List<int> FindPath(GridCoordinates startPosGrid, GridCoordinates targetPosGrid, int prioritisation) {
        /* 'prioritisastion' argument notes
         * 0 = minimise distance
         * 1 = minimise slope
         * other = maximise Earth visibility
         */

        List<int> directionSequenceReal = new List<int>();
        GridCoordinates currentPos = new GridCoordinates(startPosGrid.xCoord, startPosGrid.yCoord);
        int sequenceIndex = 0;
        double initialDistance = GetDistanceBetweenPoints(currentPos, targetPosGrid);
        bool isOptimisingForEarthVision = false;

        double slopeWeight = 1;
        double distanceWeight = 1;
        double canSeeEarthWeight = 1;

        if (prioritisation == 0) {
            distanceWeight = 1;
            slopeWeight = 0;
        } else if (prioritisation == 1) {
            distanceWeight = 0.5;
            slopeWeight = 0.8;
        } else {
            isOptimisingForEarthVision = true;
            //distanceWeight = 0.8;
            distanceWeight = 0.5;
            //slopeWeight = 0.4;
            slopeWeight = 0.8;
            canSeeEarthWeight = 10;
        }

        Debug.Log("Current Distance: " + GetDistanceBetweenPoints(currentPos, targetPosGrid));
        Debug.Log("Current Slope: " + this.slopeMap[currentPos.xCoord, currentPos.yCoord]);
        Debug.Log("Current xCoord: " + currentPos.xCoord);
        Debug.Log("Current yCoord: " + currentPos.yCoord);
        Debug.Log();

        /*TODO
        In order to overcome looping issues for slope optimisation, I can factor in the amount of turns
        spent to minimise the slope weight
        */

        HashSet<string> visited = new HashSet<string>();
        while (GetDistanceBetweenPoints(currentPos, targetPosGrid) != 0) {
            /*Index Mapping Notes
            0 = Bottom Left
            1 = Bottom Middle
            2 = Bottom Right
            3 = Middle Left
            4 = Middle Right
            5 = Top Left
            6 = Top Middle
            7 = Top Right
            */
            double[] directionEvaluation = new double[8];
            GridCoordinates[] directionList = GenerateAllEightDirections();
            int directionToChoose = -1;

            for (int i = 0; i < 8; i++) {
                int xCoordToCheck = currentPos.xCoord + directionList[i].xCoord;
                int yCoordToCheck = currentPos.yCoord + directionList[i].yCoord;

                if (visited.Contains(xCoordToCheck + "," + yCoordToCheck)) {
                    continue;
                }

                GridCoordinates coordToCheck = new GridCoordinates(xCoordToCheck, yCoordToCheck);

                if (xCoordToCheck < 0 || yCoordToCheck < 0 || xCoordToCheck >= 3200 || yCoordToCheck >= 3200) {
                    directionEvaluation[i] = 1048575;
                } else {
                    double slopeToCheck = this.slopeMap[xCoordToCheck, yCoordToCheck];

                    if (slopeToCheck >= 15) {

                        directionEvaluation[i] = 1048575;

                    } else {
                        double distanceBetweenPoints = GetDistanceBetweenPoints(coordToCheck, targetPosGrid);

                        directionEvaluation[i] = distanceWeight * distanceBetweenPoints;
                        double iterationsThresholdFactor = 1;

                        if (GetMinimumPathLength(startPosGrid, targetPosGrid) < sequenceIndex) {
                            iterationsThresholdFactor = distanceBetweenPoints / initialDistance;
                        }

                        directionEvaluation[i] += slopeWeight * slopeToCheck * iterationsThresholdFactor;

                        if (isOptimisingForEarthVision) {
                            if (!IsEarthVisible(coordToCheck)) {
                                directionEvaluation[i] += canSeeEarthWeight;
                            }
                        }

                    }
                }

                if (directionToChoose == -1 || directionEvaluation[i] < directionEvaluation[directionToChoose]) {
                    directionToChoose = i;
                }

                //Array.Clear(directionEvaluation, 0, directionEvaluation.Length);
            }

            if (directionToChoose == -1) {
                return directionSequenceReal;
            }

            directionSequenceReal.Add(directionToChoose);
            visited.Add(currentPos.xCoord + "," + currentPos.yCoord);
            currentPos.xCoord += directionList[directionToChoose].xCoord;
            currentPos.yCoord += directionList[directionToChoose].yCoord;

            Debug.Log("Current Distance: " + GetDistanceBetweenPoints(currentPos, targetPosGrid));
            Debug.Log("Current Slope: " + this.slopeMap[currentPos.xCoord, currentPos.yCoord]);
            Debug.Log("Current xCoord: " + currentPos.xCoord);
            Debug.Log("Current yCoord: " + currentPos.yCoord);
            Debug.Log();

            sequenceIndex++;
        }
        
        Debug.Log("Amount of steps: " + directionSequenceReal.Count);

        return directionSequenceReal;
    }

    public List<int> FindPath(PolarCoordinates startingPos, PolarCoordinates targetPos, int prioritisation) {
        /* 'prioritisation' argument notes
         * 0 = minimise distance
         * 1 = minimise slope
         * other = maximise Earth visibility
         */

        GridCoordinates startPosGrid = GetPolarToGridConversion(startingPos);
        GridCoordinates targetPosGrid = GetPolarToGridConversion(targetPos);

        return FindPath(startPosGrid, targetPosGrid, prioritisation);
    }
}

class Constants {
    static MoonMapper moonMapper;
    public static MoonMapper getMoonMapper(string heightFilePath, string slopeFilePath, string latitudeFilePath, string longtitudeFilePath) {
        if (moonMapper == null) moonMapper = new MoonMapper(heightFilePath, slopeFilePath, latitudeFilePath, longtitudeFilePath);
        return moonMapper;


    }
}