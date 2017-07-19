using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtension {
	public static T[] GetRow<T>(this T[,] matrix, int row)
	{
		var columns = matrix.GetLength(0);
		var array = new T[columns];
		for (int i = 0; i < columns; ++i)
			array[i] = matrix[i, row];
		return array;
	}

	public static T[] GetCol<T>(this T[,] matrix, int col)
	{
		var rows = matrix.GetLength(1);
		var array = new T[rows];
		for (int i = 0; i < rows; ++i)
			array[i] = matrix[col, i];
		return array;
	}

	public static IEnumerable<T> GetRect<T>(this T[,] matrix, int x, int y, int width, int height) {
        for (int i = x; i < x + width; i++) {
            for (int j = y; j < y + height; j++) {
                yield return matrix[i, j];
            }
        }
	}

    public static T[] Shuffle<T>(this T[] toShuffle) {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < toShuffle.Length; t++ )
        {
            T tmp = toShuffle[t];
            int r = Random.Range(t, toShuffle.Length);
            toShuffle[t] = toShuffle[r];
            toShuffle[r] = tmp;
        }

        return toShuffle;
    }
}
