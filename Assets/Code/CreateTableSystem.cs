using UnityEngine;
using System.Collections;
using Unity.Jobs;
using Unity.Entities;
using Unity.Mathematics;

public class CreateTableSystem : JobComponentSystem
{

    public struct InitializeTable : IJob
    {

        const int prime1 = 73856093;
        const int prime2 = 19349663;
        const int prime3 = 83492791;

        public void Execute()
        {
            Debug.Log("EL SIGUIENTE PRIMO ES -> " + FindNextPrime());
        }

        public int FindNextPrime()
        {
            bool found = false;
            int prime = GameBootstrapper.Settings.number;

            while (!found)
            {
                bool searching = true;
                prime++;

                if (prime == 2 || prime == 3)
                {
                    found = true;
                    return prime;
                }

                if (prime % 2 == 0 || prime % 3 == 0)
                {
                    found = false;
                    searching = false;
                }


                int divisor = 6;

                while (searching && divisor * divisor - 2 * divisor + 1 <= prime)
                {
                    if (prime % (divisor - 1) == 0 || prime % (divisor + 1) == 0)
                    {
                        found = false;
                        searching = false;
                    }


                    divisor += 6;
                }

                if (searching)
                {
                    found = true;
                }

            }

            return prime;
        }
    }
}
