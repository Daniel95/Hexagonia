using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InputHelper
{

    public class ListenerData
    {
        public KeyCode KeyCode;
        public InputListenerKeyType KeyType;
        public InputListenerStopType StopType;
        public Coroutine ListenerCoroutine;
        public Dictionary<string, Action> OnInputEventsById = new Dictionary<string, Action>();
    }

    public static GameObject MouseOverGameObject
    {
        get
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return CheckGameObjectAroundPosition(mouseWorldPosition);
        }
    }

    private const float MAX_MOUSE_HIT_RAY_RADIUS = 0.5f;
    private const float MAX_MOUSE_HIT_RAY_DISTANCE = 100f;

    private static List<ListenerData> listenerDataContainer = new List<ListenerData>();

    /// <summary>
    /// Start a new listener and that listens to the identified keycode, and executes it's onInput event when the right input is detected.
    /// Each listener requires a unique ID to be able to stop the listener later on.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keyCode"></param>
    /// <param name="onInput"></param>
    /// <param name="stopType"></param>
    public static void StartListening(string id, KeyCode keyCode, Action onInput, InputListenerKeyType keyType = InputListenerKeyType.GetKey, InputListenerStopType stopType = InputListenerStopType.Manual)
    {
        ListenerData foundListenerDataWithIdenticalID = GetListenerData(id);
        if (foundListenerDataWithIdenticalID != null)
        {
            Debug.LogError("A listener with id " + id + " already exists!");
            return;
        }

        ListenerData foundListenerData = GetListenerData(keyCode, keyType, stopType);

        if (foundListenerData == null)
        {
            Dictionary<string, Action> onInputEventsById = new Dictionary<string, Action>();
            onInputEventsById.Add(id, onInput);

            ListenerData listenerData = new ListenerData()
            {
                KeyCode = keyCode,
                KeyType = keyType,
                StopType = stopType,
                OnInputEventsById = onInputEventsById,
            };

            listenerDataContainer.Add(listenerData);

            Coroutine listenerCoroutine = CoroutineHelper.Start(ListenToInput(keyCode, keyType, stopType));
            listenerData.ListenerCoroutine = listenerCoroutine;
        }
        else
        {
            foundListenerData.OnInputEventsById.Add(id, onInput);
        }
    }

    /// <summary>
    /// Stop listening to the input specified by the ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="keyCode"></param>
    /// <param name="stopType"></param>
    /// <param name="debug"></param>
    public static void StopListening(string id, bool debug = false)
    {
        ListenerData foundListenerData = GetListenerData(id);
        if (foundListenerData == null)
        {
            if (debug)
            {
                Debug.LogError("Can't stop listener with id " + id);
            }
            return;
        }

        foundListenerData.OnInputEventsById[id] = null;
        foundListenerData.OnInputEventsById.Remove(id);

        if (foundListenerData.OnInputEventsById.Count > 0) { return; }

        CoroutineHelper.Stop(foundListenerData.ListenerCoroutine);
        listenerDataContainer.Remove(foundListenerData);
    }

    /// <summary>
    /// Check if a listener with ID exists.
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="stopType"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool ContainsListenerWithId(string id)
    {
        ListenerData foundListenerData = GetListenerData(id);
        if (foundListenerData == null) { return false; }
        if (!foundListenerData.OnInputEventsById.ContainsKey(id)) { return false; }
        return true;
    }

    private static IEnumerator ListenToInput(KeyCode keyCode, InputListenerKeyType keyType, InputListenerStopType stopType)
    {
        Func<KeyCode, bool> keyCheck = null;

        switch (keyType)
        {
            case InputListenerKeyType.GetKeyDown:
                keyCheck = x => Input.GetKeyDown(x);
                break;
            case InputListenerKeyType.GetKey:
                keyCheck = x => Input.GetKey(x);
                break;
            case InputListenerKeyType.GetKeyUp:
                keyCheck = x => Input.GetKeyUp(x);
                break;
        }

        while (true)
        {
            while (!keyCheck(keyCode))
            {
                yield return null;
            }

            ListenerData listenerData = GetListenerData(keyCode, keyType, stopType);
            List<Action> onInputs = listenerData.OnInputEventsById.Values.ToList();
            foreach (Action onInput in onInputs)
            {
                if (onInput != null)
                {
                    onInput();
                }
            }

            if (stopType == InputListenerStopType.Automatic)
            {
                break;
            }
            yield return null;
        }
    }

    private static ListenerData GetListenerData(KeyCode keyCode, InputListenerKeyType keyType, InputListenerStopType stopType)
    {
        ListenerData foundListenerData = listenerDataContainer.Find(listenerData =>
        {
            bool match = keyCode == listenerData.KeyCode && keyType == listenerData.KeyType && stopType == listenerData.StopType;
            return match;
        });
        return foundListenerData;
    }

    private static ListenerData GetListenerData(string id)
    {
        ListenerData foundListenerData = listenerDataContainer.Find(listenerData => listenerData.OnInputEventsById.ContainsKey(id));
        return foundListenerData;
    }

    private static GameObject CheckGameObjectAroundPosition(Vector2 position)
    {
        List<GameObject> gameObjectsOnPosition = CheckGameObjectsAroundPosition(position);
        GameObject closestGameObject = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject gameObject in gameObjectsOnPosition)
        {
            float distanceToGameObject = Vector2.Distance(position, gameObject.transform.position);
            if (distanceToGameObject < closestDistance)
            {
                closestDistance = distanceToGameObject;
                closestGameObject = gameObject;
            }
        }

        return closestGameObject;
    }

    private static List<GameObject> CheckGameObjectsAroundPosition(Vector2 position)
    {
        RaycastHit2D[] hits;
        hits = Physics2D.CircleCastAll(position, MAX_MOUSE_HIT_RAY_RADIUS, Vector3.forward, MAX_MOUSE_HIT_RAY_DISTANCE);

        List<GameObject> gameObjectsOnScreenPosition = new List<GameObject>();

        foreach (RaycastHit2D hit in hits)
        {
            gameObjectsOnScreenPosition.Add(hit.transform.gameObject);
        }

        return gameObjectsOnScreenPosition;
    }

}

public enum InputListenerKeyType
{
    GetKeyDown,
    GetKey,
    GetKeyUp,
}

public enum InputListenerStopType
{
    Automatic,
    Manual,
}