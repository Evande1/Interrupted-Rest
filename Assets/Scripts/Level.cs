using System.Collections.Generic;
using System.Linq;
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
	[SerializeField, Tooltip("Maximum number of sleeping students.")]
	private int maxSleepingStudents = 5;

	[Header("Student Configuration")]

	[SerializeField, Tooltip("Student list.")]
	private List<Student> studentList = new List<Student>();
	[SerializeField, Tooltip("Number of students.")]
	private int numStudents = 5;
	[SerializeField, Tooltip("Per student sleeping rate per second.")]
	private float studentSleepChancePerSecond = 0.1f;
	[SerializeField, Tooltip("Student prefabs.")]
	private List<Student> studentPrefabs = new List<Student>();
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
	[SerializeField]
	private GameObject ScreenEndPanel;
	[SerializeField]
	private TextMeshProUGUI ScreenEndScoreTMP;

	#endregion

	#region Member Declaration

	/// <summary>
	/// The current learning rate per second, scales linearly by the number of students awake.
	/// </summary>
	private float currentLearningRate => (((float)numStudents - (float)numSleepingStudents) / (float)numStudents) * maxPassiveScore / startTimeLimit;

	public int numSleepingStudents => sleepingStudentList.Count;

	public float LessonProgress
	{
		get => lessonProgress;
		set
		{
			lessonProgress = value < 0 ? 0 : value;
		}
	}

	#endregion

	#region Monobehaviour

	private void Awake()
	{
	}

	void Update()
	{
		if (!isPlaying) return;

		if (timeLeft <= 0)
		{
			isPlaying = false;
			timeLeft = 0.00001f;
			ScreenEndPanel.SetActive(true);
			ScreenEndScoreTMP.text = (Mathf.Round(lessonProgress*100.0f)/100.0f).ToString() + "%";
		}

		Debug.Log(currentLearningRate);

		// Update timer
		timeLeft -= Time.deltaTime;

		// Update timer display
		timeLeftTMP.text = $"{Mathf.FloorToInt(timeLeft / 60.0f)}:{Mathf.FloorToInt(timeLeft % 60.0f).ToString().PadLeft(2, '0')}<size=14>.{Mathf.FloorToInt(timeLeft % 1 * 100)}</size>";

		// Update lesson progress
		LessonProgress += currentLearningRate * Time.deltaTime;

		// Update lesson progress bar display
		lessonProgressTMP.text = $"{Mathf.RoundToInt(LessonProgress)}%";
		progressBarSlider.value = LessonProgress / 100.0f * progressBarMaxValue;
	}

	#endregion

	#region Level Functions

	/// <summary>
	/// Initialised the game and starts it.
	/// </summary>
	public void StartGame()
	{
		// Set level variables
		timeLeft = startTimeLimit;
		levelNameTMP.text = levelName;
		LessonProgress = 0;

		// Annihilate all previous students if any
		foreach(Student student in studentList)
			Destroy(student.gameObject);
		studentList.Clear();
		sleepingStudentList.Clear();
		

		// Instantiate students
		
		// Randomize spawn location order
		List<int> spawnOrder = Enumerable.Range(0, studentSpawnLocations.Count).ToList();
		spawnOrder.Shuffle();
		for (int i = 0; i < numStudents; i++)
		{
			Student studentPrefab = studentPrefabs[Mathf.FloorToInt(Random.Range(0, studentPrefabs.Count))];
			Student student = Instantiate(
				studentPrefab,
				studentSpawnLocations[spawnOrder[i]].transform.position,
				Quaternion.identity,
				transform);
			// Attach listener to the student object
			student.sleepChancePerSecond = studentSleepChancePerSecond;
			student.OnSleepChangeEvent.AddListener(OnStudentStateChange);
			student.OnHitEvent.AddListener(OnStudentHit);
			student.OnWantToSleepEvent.AddListener(OnStudentWantSleep);
			studentList.Add(student);
		}

		// Update state
		isPlaying = true;
	}

	void OnStudentStateChange(Student student)
	{
		if (student.IsSleeping)
		{
			sleepingStudentList.Add(student);
		}
		else
		{
			sleepingStudentList.Remove(student);
		}
	}

	void OnStudentHit(Student student)
	{
		if (student.IsSleeping)
		{
			// Add Points
			LessonProgress += bonkScore;
		}
		else
		{
			// Penalty
			LessonProgress -= bonkPenalty;
		}
	}

	void OnStudentWantSleep(Student student)
	{
		if (numSleepingStudents < maxSleepingStudents)
			student.IsSleeping = true;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	#endregion
}
