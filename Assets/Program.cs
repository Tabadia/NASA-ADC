using System.IO;
using System;
using System.Collections;
using System.Globalization;
using System.Net.Http.Headers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class GridCoordinates
{
    public int xCoord;
    public int yCoord;

    public GridCoordinates(int xCoord, int yCoord)
    {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
    }
}
class CartesianCoordinates
{
    public double xCoord;
    public double yCoord;
    public double zCoord;

    public CartesianCoordinates(double xCoord, double yCoord, double zCoord)
    {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
        this.zCoord = zCoord;
    }
    public Vector3 vector3()
    {
        //miniscule precision loss.
        return new Vector3((float)xCoord, (float)yCoord, (float)zCoord);
    }
}


class PolarCoordinates
{
    public double longitude;
    public double latitude;
    public double distanceFromMoonCoreInKilometers;

    public PolarCoordinates(double longitude, double latitude)
    {
        this.longitude = longitude;
        this.latitude = latitude;
        this.distanceFromMoonCoreInKilometers = 0;
    }

    public PolarCoordinates(double longitude, double latitude, double distanceFromMoonCoreInKilometers)
    {
        this.longitude = longitude;
        this.latitude = latitude;
        this.distanceFromMoonCoreInKilometers = distanceFromMoonCoreInKilometers;
    }
}

class MoonCalculator
{
    public static CartesianCoordinates EarthCoordinates = new CartesianCoordinates(361000, 0, -42100);

    /*public static void EarthTest() {
        Console.WriteLine(EarthCoordinates.xCoord);
    }*/

    public static double DegreesToRadians(double angleInDegrees)
    {
        double angleInRadians = angleInDegrees * (Math.PI / 180);

        return angleInRadians;
    }
    public static double RadiansToDegrees(double angleInRadians)
    {
        double angleInDegrees = angleInRadians * (180 / Math.PI);

        return angleInDegrees;
    }

    public static CartesianCoordinates GetSphericalToCartesianCoordinates(PolarCoordinates polarCoordinates, double height)
    {
        //Both values in Kilometers
        const double lunarRadius = 1737.4;
        double radius = 0;

        if (polarCoordinates.distanceFromMoonCoreInKilometers == 0)
        {
            radius = lunarRadius + 0.001 * height;
        }
        else
        {
            radius = polarCoordinates.distanceFromMoonCoreInKilometers;
        }
        /*
        Debug.Log("longitude input: " + polarCoordinates.longitude);
        Debug.Log("latitude input: " + polarCoordinates.latitude);
        Debug.Log(height);
        Console.WriteLine();
        */

        double x = radius * (Cos(polarCoordinates.latitude) * Cos(polarCoordinates.longitude));
        double y = radius * (Cos(polarCoordinates.latitude) * Sin(polarCoordinates.longitude));
        double z = radius * (Sin(polarCoordinates.latitude));

        CartesianCoordinates CartesianCoordinates = new CartesianCoordinates(x, y, z);

        return CartesianCoordinates;
    }

    public static PolarCoordinates GetCartesianToSphericalCoordinates(CartesianCoordinates cartesianCoordinates)
    {
        double x = cartesianCoordinates.xCoord;
        double y = cartesianCoordinates.yCoord;
        double z = cartesianCoordinates.zCoord;
        double distanceFromMoonCoreInKilometers = Math.Sqrt(x * x + y * y + z * z);

        double longitude = Asin((y) / (Math.Sqrt(x * x + y * y)));
        double latitude = Asin((z) / (distanceFromMoonCoreInKilometers));

        return new PolarCoordinates(longitude, latitude, distanceFromMoonCoreInKilometers);
    }

    //This method might not be needed????
    public static double GetDistanceFromMoonCoreInKilometers(CartesianCoordinates cartesianCoordinates)
    {
        double x = cartesianCoordinates.xCoord;
        double y = cartesianCoordinates.yCoord;
        double z = cartesianCoordinates.zCoord;
        double distance = Math.Sqrt(x * x + y * y + z * z);

        return distance;
    }

    public static double Sin(double angleInDegrees)
    {
        double angleInRadians = DegreesToRadians(angleInDegrees);
        double sinOutput = Math.Sin(angleInRadians);

        return sinOutput;
    }

    public static double Cos(double angleInDegrees)
    {
        double angleInRadians = DegreesToRadians(angleInDegrees);
        double cosOutput = Math.Cos(angleInRadians);

        return cosOutput;
    }

    public static double Asin(double ratio)
    {
        double aSinOutput = RadiansToDegrees(Math.Asin(ratio));

        return aSinOutput;
    }

    public static double GetAzimuthAnglePolarInputs(PolarCoordinates polarCoordinatesA, PolarCoordinates polarCoordinatesB)
    {
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

    public static double GetAzimuthAngleOneCartesianInput(PolarCoordinates polarCoordinatesA, CartesianCoordinates cartesianCoordinates)
    {
        PolarCoordinates polarCoordinatesB = GetCartesianToSphericalCoordinates(cartesianCoordinates);

        return GetAzimuthAnglePolarInputs(polarCoordinatesA, polarCoordinatesB);
    }

    public static double GetElevationAngle(PolarCoordinates polarCoordRef, double heightRef, CartesianCoordinates targetLocation)
    {
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
}






class MoonMapper
{
    public double[,] heightMap;
    public double[,] slopeMap;
    public double[,] latitudeMap;
    public double[,] longitudeMap;
    public CartesianCoordinates EarthCoordinates;

    public MoonMapper(string heightFilePath, string slopeFilePath, string latitudeFilePath, string longitudeFilePath)
    {
        this.heightMap = CSVToArray(heightFilePath);
        this.slopeMap = CSVToArray(slopeFilePath);
        this.latitudeMap = CSVToArray(latitudeFilePath);
        this.longitudeMap = CSVToArray(longitudeFilePath);
        this.EarthCoordinates = new CartesianCoordinates(361000, 0, -42100);
    }

    //If coordinates not found, returns (0, 0) by default
    public GridCoordinates GetPolarToGridConversion(PolarCoordinates polarCoords)
    {
        GridCoordinates gridLocation = new GridCoordinates(0, 0);

        List<GridCoordinates> candidateLocations = new List<GridCoordinates>();

        for (int i = 0; i < this.longitudeMap.GetLength(0); i++)
        {
            for (int j = 0; j < this.longitudeMap.GetLength(1); j++)
            {
                if (this.longitudeMap[i, j] == polarCoords.longitude)
                {
                    //Console.WriteLine(i + ", " + j);

                    candidateLocations.Add(new GridCoordinates(i, j));
                }
            }
        }

        if (candidateLocations.Count > 1)
        {
            for (int i = 0; i < candidateLocations.Count; i++)
            {
                GridCoordinates testLocation = (GridCoordinates)candidateLocations[i];

                if (polarCoords.latitude == this.latitudeMap[testLocation.xCoord, testLocation.yCoord])
                {
                    gridLocation = testLocation;
                    break;
                }
            }

        }
        else if (candidateLocations.Count == 1)
        {
            gridLocation = (GridCoordinates)candidateLocations[0];
        }

        /*Console.WriteLine();
        Console.WriteLine("Real coordinate:");
        Console.WriteLine(gridLocation.xCoord + ", " + gridLocation.yCoord);
        */

        return gridLocation;
    }


    public int CountElementsInLine(string source, char toFind)
    {
        int count = 1;

        foreach (var ch in source)
        {
            if (ch == toFind)
                count++;
        }

        return count;
    }
    static T[,] To2D<T>(T[][] source)
    {
        // T is serialization for type. 
        try
        {
            int FirstDim = source.Length;
            int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

            var result = new T[FirstDim, SecondDim];
            for (int i = 0; i < FirstDim; ++i)
                for (int j = 0; j < SecondDim; ++j)
                    result[i, j] = source[i][j];

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("The given jagged array is not rectangular.");
        }
    }
    public static double[,] CSVToArray(string filePath)
    {

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
    /*
    public static double[,] CSVToArray(string filePath) {
        var path = @filePath;

        using (TextFieldParser csvParser = new TextFieldParser(path)) {
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = true;

            string[] currentLine = csvParser.ReadFields();

            int lineCount = File.ReadLines(path).Count();
            int lineLength = currentLine.Length;

            //Console.WriteLine("Row length of CSV: " + lineLength);
            //Console.WriteLine(csvParser.LineNumber);

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
    */
    public double GetDistanceBetweenPoints(GridCoordinates point1, GridCoordinates point2)
    {
        int xDistance = Math.Abs(point2.xCoord - point1.xCoord);
        int yDistance = Math.Abs(point2.yCoord - point1.yCoord);

        double distance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);

        return distance;
    }

    public GridCoordinates[] GenerateAllEightDirections()
    {
        GridCoordinates[] EightDirectionsList = new GridCoordinates[8];
        int index = 0;

        for (int yCoord = 1; yCoord > -2; yCoord--)
        {
            for (int xCoord = -1; xCoord < 2; xCoord++)
            {
                if (yCoord != 0 || xCoord != 0)
                {
                    EightDirectionsList[index] = new GridCoordinates(xCoord, yCoord);
                    index++;
                }
            }
        }

        return EightDirectionsList;
    }
    //what???? Math.Max pls
    public int GetBiggerNumber(int num1, int num2)
    {
        if (num1 >= num2)
        {
            return num1;
        }
        else
        {
            return num2;
        }
    }

    public int GetMinimumPathLength(GridCoordinates startPosGrid, GridCoordinates targetPosGrid)
    {
        int xDif = Math.Abs(targetPosGrid.xCoord - startPosGrid.xCoord);
        int yDif = Math.Abs(targetPosGrid.yCoord - startPosGrid.yCoord);

        int distance = GetBiggerNumber(xDif, yDif);

        return distance;
    }

    public List<int> FindPath(GridCoordinates startPosGrid, GridCoordinates targetPosGrid, int prioritisation)
    {
        /* 'prioritisastion' argument notes
         * 0 = minimise distance
         * 1 = minimise slope
         * other = maximise Earth visibility
         */

        List<int> directionSequenceReal = new List<int>();
        //GridCoordinates currentPos = startPosGrid; IT PASSES AN OBJECT REFERENCE, NOT A DEEP COPY
        GridCoordinates currentPos = new GridCoordinates(startPosGrid.xCoord, startPosGrid.yCoord);
        int sequenceIndex = 0;
        double initialDistance = GetDistanceBetweenPoints(currentPos, targetPosGrid);

        double slopeWeight = 0.3;
        double distanceWeight = 1;

        if (prioritisation == 0)
        {
            distanceWeight = 1;
            slopeWeight = 0;
        }
        else if (prioritisation == 1)
        {
            distanceWeight = 0.5;
            slopeWeight = 1;
        }
        else
        {
            distanceWeight += 1;
            slopeWeight = 0;
        }
        /*
        Debug.Log("Current Distance: " + GetDistanceBetweenPoints(currentPos, targetPosGrid));
        Debug.Log(this.slopeMap[0, currentPos.yCoord]);
        Debug.Log("Current xCoord: " + currentPos.xCoord);
        Debug.Log("Current yCoord: " + currentPos.yCoord);
        Debug.Log(currentPos.yCoord >= 0 && currentPos.xCoord >= 0 && currentPos.yCoord < this.slopeMap.Length && currentPos.xCoord < this.slopeMap.Length); //true
        Debug.Log("Current Slope: " + this.slopeMap[currentPos.xCoord, currentPos.yCoord]); //Index Out Of Bounds??????
        */

        /*TODO
        In order to overcome looping issues for slope optimisation, I can factor in the amount of turns
        spent to minimise the slope weight
        */

        while (GetDistanceBetweenPoints(currentPos, targetPosGrid) != 0)
        {
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
            int directionToChoose = 0;

            for (int i = 0; i < 8; i++)
            {
                int xCoordToCheck = currentPos.xCoord + directionList[i].xCoord;
                int yCoordToCheck = currentPos.yCoord + directionList[i].yCoord;
                GridCoordinates coordToCheck = new GridCoordinates(xCoordToCheck, yCoordToCheck);

                if (xCoordToCheck < 0 || yCoordToCheck < 0 || xCoordToCheck >= 3200 || yCoordToCheck >= 3200)
                {
                    directionEvaluation[i] = 1048575;
                }
                else
                {
                    //double slopeToCheck = this.slopeMap[(int)(xCoordToCheck*10.24), (int)(yCoordToCheck * 10.24)];
                    double slopeToCheck = this.slopeMap[(int)(xCoordToCheck / 10.24), (int)(yCoordToCheck / 10.24)];
                    if (slopeToCheck >= 15)
                    {
                        directionEvaluation[i] = 1048575;
                    }
                    else
                    {
                        double distanceBetweenPoints = GetDistanceBetweenPoints(coordToCheck, targetPosGrid);

                        directionEvaluation[i] = distanceWeight * distanceBetweenPoints;
                        double iterationsThresholdFactor = 1;

                        if (GetMinimumPathLength(startPosGrid, targetPosGrid) < sequenceIndex)
                        {
                            iterationsThresholdFactor = distanceBetweenPoints / initialDistance;
                        }

                        directionEvaluation[i] += slopeWeight * slopeToCheck * iterationsThresholdFactor;
                    }
                }

                if (i > 0)
                {
                    if (directionEvaluation[i] < directionEvaluation[directionToChoose])
                    {
                        directionToChoose = i;
                    }
                }

                //Array.Clear(directionEvaluation, 0, directionEvaluation.Length);
            }

            directionSequenceReal.Add(directionToChoose);
            currentPos.xCoord += directionList[directionToChoose].xCoord;
            currentPos.yCoord += directionList[directionToChoose].yCoord;
            /*
            Debug.Log("Current Distance: " + GetDistanceBetweenPoints(currentPos, targetPosGrid));

            Debug.Log("Current xCoord: " + currentPos.xCoord);
            Debug.Log("Current yCoord: " + currentPos.yCoord);
            Debug.Log("Current Slope: " + this.slopeMap[currentPos.xCoord, currentPos.yCoord]);
            */
            sequenceIndex++;
        }

        return directionSequenceReal;
    }

    public List<int> FindPath(PolarCoordinates startingPos, PolarCoordinates targetPos, int prioritisation)
    {
        /* 'prioritisastion' argument notes
         * 0 = minimise distance
         * 1 = minimise slope
         * other = maximise Earth visibility
         */

        GridCoordinates startPosGrid = GetPolarToGridConversion(startingPos);
        GridCoordinates targetPosGrid = GetPolarToGridConversion(targetPos);

        return FindPath(startPosGrid, targetPosGrid, prioritisation);
    }
}
class Constants
{
    static MoonMapper moonMapper;
    public static MoonMapper getMoonMapper(string heightFilePath, string slopeFilePath, string latitudeFilePath, string longtitudeFilePath)
    {
        if(moonMapper == null) moonMapper = new MoonMapper(heightFilePath, slopeFilePath, latitudeFilePath, longtitudeFilePath);
        return moonMapper;


    }
}
