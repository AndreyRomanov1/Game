using System;
using System.Collections.Generic;
using System.Linq;


public static class RandomExtension
{
    public static T ProbabilisticRandom<T>(this Random random, Dictionary<T, int> coefficientDictionary)
    {
        var maxValue = coefficientDictionary.Values.Sum();
        var value = random.Next(maxValue);
        var accumulatedAmount = 0;
        foreach (var key in coefficientDictionary.Keys)
        {
            accumulatedAmount += coefficientDictionary[key];
            if (accumulatedAmount >= value)
                return key;
        }

        throw new Exception();
    }
}
