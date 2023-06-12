using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Quest : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public string Name;
        public Sprite Icon;
        public string Description;
    }

    [Header("Info")] public Info Information;

    [System.Serializable]
    public struct Stat
    {
        public int Money;
    }

    [Header("Reward")] public Stat Reward;
    
    public bool Completed { get; private set; }
    public QuestCompletedEvent QuestCompleted;

    public abstract class QuestGoal : ScriptableObject
    {
        protected string _name;
        protected string Description;
        public float CurrentAmount { get; protected set; }
        public float RequiredAmount = 1f;
        
        public bool Completed { get; protected set; }
        [HideInInspector] public UnityEvent GoalCompleted;

        public virtual string GetDescription()
        {
            return Description;
        }

        public virtual string GetName()
        {
            return _name;
        }

        public virtual void Initialize()
        {
            CurrentAmount = 0;
            Completed = false;
            GoalCompleted = new UnityEvent();
        }

        public void Evaluate()
        {
            if (CurrentAmount >= RequiredAmount)
            {
                Complete();
            }
        }

        private void Complete()
        {
            Completed = true;
            GoalCompleted?.Invoke();
            GoalCompleted?.RemoveAllListeners();
        }
    }

    public List<QuestGoal> Goals;

    public void Initialize()
    {
        Completed = false;
        QuestCompleted = new QuestCompletedEvent();

        foreach (var goal in Goals)
        {
            goal.Initialize();
            goal.GoalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }

    public void Evaluate()
    {
        foreach (var goal in Goals)
        {
            goal.Evaluate();
        }
    }

    private void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
        if (Completed)
        {
            QuestCompleted.Invoke(this);
            QuestCompleted.RemoveAllListeners();
        }
    }
}

public class QuestCompletedEvent : UnityEvent<Quest> { }

#if UNITY_EDITOR
[CustomEditor(typeof(Quest))]
public class QuestEditor: Editor
{
    private SerializedProperty m_questInfoProperty;
    private SerializedProperty m_questStatProperty;

    private List<string> m_questGoalType;
    private SerializedProperty m_questGoalListProperty;

    [MenuItem("Assets/Quest", priority = 0)]
    public static void CreateQuest()
    {
        var newQuest = CreateInstance<Quest>();
        
        ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
    }

    private void OnEnable()
    {
        m_questInfoProperty = serializedObject.FindProperty(nameof(Quest.Information));
        m_questStatProperty = serializedObject.FindProperty(nameof(Quest.Reward));

        m_questGoalListProperty = serializedObject.FindProperty(nameof(Quest.Goals));

        var lookup = typeof(Quest.QuestGoal);
        
        // Loads all classes that inherit from QuestGoal class
        m_questGoalType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        var child = m_questInfoProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);
        
        // Display Quest Info
        EditorGUILayout.LabelField("Quest Info", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        child = m_questStatProperty.Copy();
        depth = child.depth;
        child.NextVisible(true);
        
        // Display Quest Reward
        EditorGUILayout.LabelField("Quest Reward", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        int choice = EditorGUILayout.Popup("Add new Quest Goal", -1, m_questGoalType.ToArray());

        // Add Quest Goals
        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_questGoalType[choice]);
            
            AssetDatabase.AddObjectToAsset(newInstance, target);
            
            m_questGoalListProperty.InsertArrayElementAtIndex(m_questGoalListProperty.arraySize);
            m_questGoalListProperty.GetArrayElementAtIndex(m_questGoalListProperty.arraySize - 1)
                .objectReferenceValue = newInstance;
        }

        // This part is for removing the quest goals at the press of a button
        Editor ed = null;
        int toDelete = -1;

        for (int i = 0; i < m_questGoalListProperty.arraySize; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_questGoalListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);
            
            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);
            
            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (toDelete != -1)
        {
            var item = m_questGoalListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);
            
            m_questGoalListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif