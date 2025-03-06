using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GradingManager : MonoBehaviour
{
    private List<int> grades = new List<int>();

    [SerializeField]
    private TMP_Text gradeText;

    public void RegisterGrade(int grade)
    {
        grades.Add(grade);
    }

    public int GetOverallGrade()
    {
        float averageGrade = GetAverageGrade();

        if(averageGrade > 2.5f)
        {
            return 5;
        }
        if(averageGrade >= 1.5f)
        {
            return 4;
        }
        if(averageGrade >= 0.5f)
        {
            return 3;
        }

        return 2;
    }

    private float GetAverageGrade()
    {
        if(grades.Count == 0) return 0;

        float sum = 0;

        foreach(int grade in grades)
        {
            sum += grade;
        }

        return sum / grades.Count;
    }

    public void ResetGrades()
    {
        grades.Clear();
    }

    public void DisplayGrade()
    {
        //gradeText.text = GetOverallGrade();
    }
}
