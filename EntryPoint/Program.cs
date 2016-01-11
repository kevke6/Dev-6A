using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
  public static class Program
  {
    [STAThread]
    static void Main()
    {
      var fullscreen = false;
      read_input:
      switch (Microsoft.VisualBasic.Interaction.InputBox("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment", VirtualCity.GetInitialValue()))
      {
        case "1":
          using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen))
            game.Run();
          break;
        case "2":
          using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen))
            game.Run();
          break;
        case "3":
          using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
            game.Run();
          break;
        case "4":
          using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
            game.Run();
          break;
        case "q":
          return;
      }
      goto read_input;
    }

    private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
    {
            int[] arraytest = new int[50];
            int[,] multiarray = new int[50,3];
            int huis = 0;
         
            foreach (var value in specialBuildings) {
                float specialx = value.X;
                float specialy = value.Y;
                float huisx = house.X;
                float huisy = house.Y;

                float stellingx = huisx - specialx;
                float stellingy = huisy - specialy;

                int intstellingx = (int)Math.Ceiling(stellingx);
                int intstellingy = (int)Math.Ceiling(stellingy);
                int stellingc = (intstellingx * intstellingx) + (intstellingy * intstellingy);
                arraytest[huis] = stellingc;
                multiarray[huis,0] = stellingc;
                multiarray[huis, 1] = (int)Math.Ceiling(specialx);
                multiarray[huis, 2] = (int)Math.Ceiling(specialy);
                huis++;
            }
            MergeSort_Recursive(arraytest, 0, arraytest.Length - 1);
            List<Vector2> houseOrderList = new List<Vector2>();
            int i = 0;
            int j = 0;
            while (i < arraytest.Length) {
               
                if (multiarray[j, 0] == arraytest[i])
                {
                    houseOrderList.Add(new Vector2(multiarray[j, 1], multiarray[j, 2]));
                    i++;
                    j = 0;
                }
                else {
                    j++;
                }
            }
              return houseOrderList;
        }

        static public void DoMerge(int[] numbers, int left, int mid, int right)
        {
            int[] temp = new int[50];
            int i, left_end, num_elements, tmp_pos;

            left_end = (mid - 1);
            tmp_pos = left;
            num_elements = (right - left + 1);

            while ((left <= left_end) && (mid <= right))
            {
                if (numbers[left] <= numbers[mid])
                    temp[tmp_pos++] = numbers[left++];
                else
                    temp[tmp_pos++] = numbers[mid++];
            }

            while (left <= left_end)
                temp[tmp_pos++] = numbers[left++];

            while (mid <= right)
                temp[tmp_pos++] = numbers[mid++];

            for (i = 0; i < num_elements; i++)
            {
                numbers[right] = temp[right];
                right--;
            }
        }

        static public void MergeSort_Recursive(int[] numbers, int left, int right)
        {
            int mid;

            if (right > left)
            {
                mid = (right + left) / 2;
                MergeSort_Recursive(numbers, left, mid);
                MergeSort_Recursive(numbers, (mid + 1), right);

                DoMerge(numbers, left, (mid + 1), right);
            }
        }


        private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {
            float[,] specialBuildingsTree = new float [50,10];
            foreach (var special in specialBuildings)
            {
                var checker = 0;
                while (checker < 50)
                {
                    if (special.X == specialBuildingsTree[checker, 0])
                    {
                        var counter = 2;
                        while (counter < 10)
                        {
                            if (specialBuildingsTree[checker, counter] == 0)
                            {
                                specialBuildingsTree[checker, counter] = special.Y;
                                counter = 11;
                                checker = 51;
                            }
                            else
                            {
                                counter++;
                            }
                        }
                    }
                    else
                    {
                        if (specialBuildingsTree[checker, 0] == 0)
                        {
                            specialBuildingsTree[checker, 0] = special.X;
                            specialBuildingsTree[checker, 1] = special.Y;
                            checker = 51;
                        }
                        else { checker++; }
                    }
                }
            }

            float[,] houseArray = new float[5,2];
            int houseCounter = 0;
            foreach (var house in housesAndDistances)
            {
                float distance = house.Item2;
                houseArray[houseCounter, 0] = house.Item1.X;
                houseArray[houseCounter, 1] = house.Item1.Y;
            }

            var houseCounterHelper = 0;
            List<Vector2> houseInDistance = new List<Vector2>();
            foreach (var houseTest in housesAndDistances)
            {
                
                float distanceHelper = houseTest.Item2;
                var counter = 0;
                while (counter < 50)
                {
                    if (houseTest.Item1.X + distanceHelper >= specialBuildingsTree[counter, 0]
                        && houseTest.Item1.X - distanceHelper <= specialBuildingsTree[counter, 0])
                    {
                        var checker = 1;
                        while (checker < 10) {
                            Console.WriteLine(houseTest.Item1.Y + "dit is de Y");
                            Console.WriteLine(specialBuildingsTree[counter, checker]);
                            if (houseTest.Item1.Y + distanceHelper >= specialBuildingsTree[counter, checker]
                                && houseTest.Item1.Y - distanceHelper <= specialBuildingsTree[counter, checker])
                            {
                                houseInDistance.Add(new Vector2(specialBuildingsTree[counter,0],specialBuildingsTree[counter,checker]));
                                checker++;
                            }
                            else { checker++; }
                        }
                        counter++;
                    }
                    else {
                         if(specialBuildingsTree[counter,0] == 0) { counter = 51; } else { counter++; }
                    }
                }
                houseCounterHelper++;
            }

           // yield return specialBuildings;
            yield return houseInDistance;
            //return
            //    from h in housesAndDistances
            //    select
            //      from s in specialBuildings
            //      where Vector2.Distance(h.Item1, s) <= h.Item2
            //      select s;
        }

        private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
      List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
      var prevRoad = startingRoad;
      for (int i = 0; i < 30; i++)
      {
        prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
        fakeBestPath.Add(prevRoad);
      }
      return fakeBestPath;
    }

    private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, 
      IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
      foreach (var d in destinationBuildings)
      {
        var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
        List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
        var prevRoad = startingRoad;
        for (int i = 0; i < 30; i++)
        {
          prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
          fakeBestPath.Add(prevRoad);
        }
        result.Add(fakeBestPath);
      }
      return result;
    }
  }
#endif
}
