using System;
using System.Collections.Generic;

namespace cycles;
internal class Program
{
    static void Main(string[] args)
    {
        for (int N = 1; N < 10; N++)
            FindCyclesAndPerms(N);
        Console.ReadKey();
    }

    private static void FindCyclesAndPerms(int N)
    {
        int totalCycles = 0;
        int totalOrderedCycles = 0;
        int totalPermutations = 0;
        int totalOrderedPermutations = 0;
        foreach (int[] arr in GetPermutationsOfN_Elements(N))
        {
            int permsMult = 1;
            int cyclesThisPerm = 0;
            Flag flags = new(N);
            for (int i = 0; i < N; i++)
            {
                if (flags[i]) continue;
                while (!flags[i])
                {
                    flags[i] = true;
                    i = arr[i];
                    if (!flags[i]) permsMult *= 2;
                }
                cyclesThisPerm++;
            }
            totalCycles += cyclesThisPerm;
            totalOrderedCycles += cyclesThisPerm * permsMult;
            totalPermutations++;
            totalOrderedPermutations += permsMult;
        }

        Console.WriteLine($"{totalCycles} / {totalPermutations} unordered or");
        Console.WriteLine($"{totalOrderedCycles} / {totalOrderedPermutations} ordered permutations of length {N}");
    }

    public static IEnumerable<int[]> GetPermutationsOfN_Elements(int[] arr)
    {
        int N = arr.Length;
        int[] indices = new int[N];
        for (int i = 0; i < N; i++) indices[i] = i;
        int up = 0;
        int[] rearranged()
        {
            int[] temp = new int[N];
            for (int i = 0;i<N; i++)
            {
                temp[i] = arr[indices[i]];
            }
            return temp;
        }
        yield return rearranged();

        void swap(int a, int b) => (indices[a], indices[b]) = (indices[b], indices[a]);
        while (up < N - 1)
        {
            if (up == 0)
            {
                swap(0, 1);
                up = 1;
                while (
                    up < N - 1 &&
                    indices[up] > indices[up + 1]
                    ) ++up;
            } else
            {
                if (indices[up + 1] > indices[0]) swap(up + 1, 0);
                else
                {
                    int start = 0, end = up, mid = (start + end) / 2, tVal = indices[up + 1];
                    while (!(indices[mid] < tVal && indices[mid - 1] > tVal))
                    {
                        if (indices[mid] < tVal) end = mid - 1;
                        else start = mid + 1;
                        mid = (start + end) / 2;
                    }
                    swap(up + 1, mid);
                }
                for (int i = 0; i <= up / 2; ++i) swap(i, up - i);
                up = 0;
            }
            yield return rearranged();
        }
    }
    public static IEnumerable<int[]> GetPermutationsOfN_Elements(int n)
    {
        int[] arr = new int[n];
        for (int i = n - 1; i >= 0; i--) arr[i] = i;
        return GetPermutationsOfN_Elements(arr);
    }
}
struct Flag(int n)
{
    const uint U1 = 1;
    internal uint[] values = new uint[n / 32 + ((n % 32 == 0) ? 0 : 1)];

    public readonly bool Value(int position)
    {
        return (values[position / 32] & ((uint)1 << (position % 32))) == U1;
    }
    public readonly bool this[int n]
    {
        get => (values[n / 32] & (1 << (n % 32))) != 0;
        set => values[n / 32] = values[n / 32] & ~(U1 << (n % 32)) | (uint)(value ? 1 : 0) << (n % 32);
    }
}

