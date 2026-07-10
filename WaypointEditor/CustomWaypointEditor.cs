#if UNITY_EDITOR
using System;
using Enemy.EnemyData;
using Enemy.States;
using Enemy.Structs;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace WaypointEditor
{
    [CustomEditor(typeof(EnemyWaypointNav))]
    public class CustomWaypointEditor : Editor
    {
        private EnemyWaypointNav _target;
        private WaypointData _data;
        private bool _addMode = false;

        private void OnEnable()
        {
            _target = (EnemyWaypointNav)target;
            _data = _target.Data; 
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_data == null)
            {
                EditorGUILayout.HelpBox("Assign waypoint asset", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField($"Waypoints: {_data.NAV_waypoints.Count}", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            GUI.backgroundColor = _addMode ? Color.green : Color.white;
            if (GUILayout.Button(
                    _addMode
                        ? "Addition mode: ON"
                        : "Addition mode: OFF",
                    GUILayout.Height(30)))
            {
                _addMode = !_addMode;
                SceneView.RepaintAll();
            }

            GUI.backgroundColor = Color.white;

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("[!] Remove Waypoints"))
            {
                Undo.RecordObject(_data, "Clear Waypoints");
                _data.NAV_waypoints.Clear();
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
            }

            GUI.backgroundColor = Color.white;
        }

        private void OnSceneGUI()
        {
            if (_data == null) return;
            if (_addMode) HandleAddMode();

            for (int i = 0; i < _data.NAV_waypoints.Count; i++)
                DrawWaypointHandle(i);
        }

        private void HandleAddMode()
        {
            
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);

            Event e = Event.current;

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 position = hit.point;

                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                        position = navHit.position;

                    if (e.control) //ctrl lmb delete
                    {
                        RemoveNearestWaypoint(position);
                    }
                    else //lmb add point
                    {
                        Undo.RecordObject(_data, "Add Waypoint");
                        _data.NAV_waypoints.Add(new NavWaypoint(position));
                        EditorUtility.SetDirty(_data);
                        AssetDatabase.SaveAssets();
                    }

                    e.Use();
                }
            }

            
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
            {
                _addMode = false;
                e.Use();
                Repaint();
            }

          
            Handles.BeginGUI();
            var style = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = Color.white }
            };
            GUI.Label(new Rect(10, 10, 400, 25), "LMB Add / ESC exit", style);
            Handles.EndGUI();
        }

        private void RemoveNearestWaypoint(Vector3 position)
        {
            float minDist = float.MaxValue;
            int nearestIndex = -1;

            for (int i = 0; i < _data.NAV_waypoints.Count; i++)
            {
                float dist = Vector3.Distance(_data.NAV_waypoints[i].position, position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestIndex = i;
                }
            }

            if (nearestIndex >= 0 && minDist < 2f) 
            {
                Undo.RecordObject(_data, "Remove Waypoint");
                _data.NAV_waypoints.RemoveAt(nearestIndex);
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
            }
        }

        private void DrawWaypointHandle(int index)
        {
            NavWaypoint waypoint = _data.NAV_waypoints[index];
            Handles.color = _data.WaypointColor;
            Handles.Label(waypoint.position + Vector3.up * 0.7f, $"#{index}");

          
            Handles.SphereHandleCap(0, waypoint.position, Quaternion.identity,
                _data.gizmoRadius, EventType.Repaint);


            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(waypoint.position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_data, "Move Waypoint");
                var updated = _data.NAV_waypoints[index];
                updated.position = newPos;
                _data.NAV_waypoints[index] = updated;
                EditorUtility.SetDirty(_data);
            }


            if (_data.ShowConnections && index < _data.NAV_waypoints.Count - 1)
            {
                Handles.color = new Color(1f, 1f, 1f, 0.5f);
                Handles.DrawDottedLine(_data.NAV_waypoints[index].position,
                    _data.NAV_waypoints[index + 1].position, 4f);
            }
        }
    }
}
#endif