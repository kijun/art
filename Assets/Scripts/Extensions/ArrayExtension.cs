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

	public static IEnumerator<T> GetRect<T>(this T[,] matrix, int x, int y, int width, int height) {
        for (int i = x; i < x + width; i++) {
            for (int j = y; j < y + height; j++) {
                yield return matrix[i, j];
            }
        }
	}

}
