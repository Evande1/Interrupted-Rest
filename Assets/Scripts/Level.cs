using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
	#region Serializables

	[Header("Level Configuration")]

	[SerializeField, Tooltip("Level display name.")]
	private string levelName = "Lv1";
	[SerializeField, Tooltip("Starting time limit (time to bell) in seconds.")]
	private float startTimeLimit = 90;
	[SerializeField, Tooltip("Theoretical maximum score attainable if all students are awake at all time. This affects the passive learning rate contributed by each awake student.")]
	private float maxPassiveScore = 120;
	[SerializeField, Tooltip("Learning progress contribution on bonking a sleeping student.")]
	private float bonkScore = 3;
	[SerializeField, Tooltip("Learning progress penalty on bonking an awake student.")]
	private float bonkPenalty = 5;

	[Header("Student Configuration")]

	[SerializeField, Tooltip("Student list.")]
	private List<Student> studentList = new List<Student>();
	[SerializeField, Tooltip("Number of students.")]
	private int numStudents = 5;
	[SerializeField]
	private List<GameObject> studentSpawnLocations = new List<GameObject>();
	[SerializeField]
	private List<Student> sleepingStudentList = new List<Student>();

	[Header("Teacher Configuration")]

	[SerializeField, Tooltip("Teacher list.")]
	private List<Teacher> teacherList = new List<Teacher>();

	[Header("Game Status")]

	[SerializeField, Tooltip("Whether the game is progressing.")]
	public bool isPlaying = false;
	[SerializeField, Tooltip("Lesson progress. 0-100, can go beyond 100 as bonus progress.")]
	private float lessonProgress = 0;
	[SerializeField, Tooltip("Time left (time to bell) in seconds.")]
	private float timeLeft = 90;

	[Header("References")]

	[SerializeField]
	private TextMeshProUGUI levelNameTMP;
	[SerializeField]
	private TextMeshProUGUI timeLeftTMP;
	[SerializeField]
	private TextMeshProUGUI lessonProgressTMP;
	[SerializeField]
	private float progressBarMaxValue = 20;
	[SerializeField]
	private Slider progressBarSlider;

	#endregion

	#region Member Declaration

	/// <summary>
	/// The current learning rate per second, scales linearly by the number of students awake.
	/// </summary>
	//private float currentLearningRate => (studentList.Count - sleepingStudentList.Count) / studentList.Count * maxPassiveScore / startTimeLimit;
	private float currentLearningRate => maxPassiveScore/startTimeLimit;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		StartGame();
	}

	void Update()
	{
		if (!isPlaying) return;

		// Update timer
		timeLeft -= Time.deltaTime;

		// Update timer display
		timeLeftTMP.text = $"{Mathf.FloorToInt(timeLeft / 60.0f)}:{Mathf.FloorToInt(timeLeft % 60.0f).ToString().PadLeft(2, '0')}<size=14>.{Mathf.FloorToInt(timeLeft % 1 * 100)}</size>";

		// Update lesson progress
		lessonProgress += currentLearningRate * Time.deltaTime;

		// Update lesson progress bar display
		lessonProgressTMP.text = $"{Mathf.RoundToInt(lessonProgress)}%";
		progressBarSlider.value = lessonProgress / 100.0f * progressBarMaxValue;
	}

	#endregion

	#region Level Functions

	/// <summary>
	/// Initialised the game and starts it.
	/// </summary>
	void StartGame()
	{
		timeLeft = startTimeLimit;
		levelNameTMP.text = levelName;
		lessonProgress = 0;
		isPlaying = true;
	}

	#endregion
}
